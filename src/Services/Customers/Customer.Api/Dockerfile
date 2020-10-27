FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

COPY Invoicing.sln Invoicing.sln

COPY src/Base/Base.Ddd/Base.Ddd.csproj src/Base/Base.Ddd/Base.Ddd.csproj
COPY src/Base/Base.Infrastructure/Base.Infrastructure.csproj src/Base/Base.Infrastructure/Base.Infrastructure.csproj
COPY src/Services/Customers/Customer.Api/Customer.Api.csproj src/Services/Customers/Customer.Api/Customer.Api.csproj
COPY src/Services/Customers/Customer.Domain/Customers.Domain.csproj src/Services/Customers/Customer.Domain/Customers.Domain.csproj
COPY src/Services/Customers/Customer.Infrastructure/Customers.Infrastructure.csproj src/Services/Customers/Customer.Infrastructure/Customers.Infrastructure.csproj
COPY src/Services/Customers/Customer.UnitTests/Customers.UnitTests.csproj src/Services/Customers/Customer.UnitTests/Customers.UnitTests.csproj
COPY src/Services/Identity/Identity.Service/Identity.Service.csproj src/Services/Identity/Identity.Service/Identity.Service.csproj

RUN dotnet restore

COPY . .

WORKDIR /source/src/Services/Customers/Customer.Api
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /source/src/Services/Customers/Customer.Api/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "Customer.Api.dll"]
