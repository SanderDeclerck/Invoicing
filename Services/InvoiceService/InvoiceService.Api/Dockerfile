# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY Invoicing.sln .
COPY Services/InvoiceService/InvoiceService.Api/*.csproj ./Services/InvoiceService/InvoiceService.Api/
COPY Services/InvoiceService/InvoiceService.Data/*.csproj ./Services/InvoiceService/InvoiceService.Data/
COPY Services/InvoiceService/InvoiceService.Domain/*.csproj ./Services/InvoiceService/InvoiceService.Domain/
COPY Services/InvoiceService/InvoiceService.Pdf/*.csproj ./Services/InvoiceService/InvoiceService.Pdf/
COPY Shared/Invoicing.SharedKernel/*.csproj ./Shared/Invoicing.SharedKernel/
RUN dotnet restore

# copy everything else and build app
COPY Services/InvoiceService/InvoiceService.Api/. ./Services/InvoiceService/InvoiceService.Api/
COPY Services/InvoiceService/InvoiceService.Data/. ./Services/InvoiceService/InvoiceService.Data/
COPY Services/InvoiceService/InvoiceService.Domain/. ./Services/InvoiceService/InvoiceService.Domain/
COPY Services/InvoiceService/InvoiceService.Pdf/. ./Services/InvoiceService/InvoiceService.Pdf/
COPY Shared/Invoicing.SharedKernel/. ./Shared/Invoicing.SharedKernel/
WORKDIR /source/Services/InvoiceService/InvoiceService.Api
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "InvoiceService.Api.dll"]