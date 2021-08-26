using Pulumi;
using Pulumi.AzureNative.Resources;
using Pulumi.AzureNative.Web;
using Pulumi.AzureNative.Web.Inputs;

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
            Kind = "app,linux,container"
        }, new CustomResourceOptions { DependsOn = sdInvoicingPlan });
    }
}
