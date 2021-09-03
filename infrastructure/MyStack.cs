using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;
using ManagedServiceIdentityType = Pulumi.AzureNative.Web.ManagedServiceIdentityType;

class MyStack : Stack
{
    public MyStack()
    {
        var resourceGroup = new ResourceGroup("sd-invoicing");

        var sdInvoicingPlan = new AppServicePlan("sd-invoicing-plan", new AppServicePlanArgs
        {
            Kind = "linux",
            Location = "North Europe",
            Name = "plan-production",
            PerSiteScaling = false,
            Reserved = true,
            ResourceGroupName = "production",
            Sku = new SkuDescriptionArgs
            {
                Capacity = 1,
                Name = "B1",
            },
        }, new CustomResourceOptions { Protect = true });

        var webApp = new WebApp("invoicing-service", new WebAppArgs
        {
            ResourceGroupName = resourceGroup.Name,
            ServerFarmId = sdInvoicingPlan.Id,
            Kind = "app,linux,container",
            SiteConfig = new SiteConfigArgs
            {
                AppSettings = new[]
                {
                    new NameValuePairArgs
                    {
                        Name = "DOCKER_REGISTRY_SERVER_URL",
                        Value = "https://ghcr.io"
                    },
                    new NameValuePairArgs
                    {
                        Name = "DOCKER_REGISTRY_SERVER_USERNAME",
                        Value = "@Microsoft.KeyVault(VaultName=kv-sd-software;SecretName=github-username)"
                    },
                    new NameValuePairArgs
                    {
                        Name = "DOCKER_REGISTRY_SERVER_PASSWORD",
                        Value = "@Microsoft.KeyVault(VaultName=kv-sd-software;SecretName=github-packages-pat-token)"
                    },
                },
                LinuxFxVersion = "DOCKER",
                AlwaysOn = true
            },
            Identity = new ManagedServiceIdentityArgs
            {
                Type = ManagedServiceIdentityType.SystemAssigned
            }
        }, new CustomResourceOptions { DependsOn = sdInvoicingPlan });

        InvoicingServiceName = webApp.Name;
    }

    [Output] public Output<string> InvoicingServiceName { get; set; }
}
