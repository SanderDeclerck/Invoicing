FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY sd-invoicing.sln sd-invoicing.sln

COPY src/services/invoicing/Invoicing.Api/Invoicing.Api.csproj src/services/invoicing/Invoicing.Api/Invoicing.Api.csproj
COPY infrastructure/sd-invoicing.csproj infrastructure/sd-invoicing.csproj

RUN dotnet restore

COPY . .

WORKDIR /source/src/services/invoicing/Invoicing.Api
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /source/src/services/invoicing/Invoicing.Api/out ./
EXPOSE 80
ENTRYPOINT ["dotnet", "Invoicing.Api.dll"]