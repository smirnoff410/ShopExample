FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT Production

COPY "ShopExample.sln" "ShopExample.sln"

COPY "Common/Common.csproj" "Common/Common.csproj"

COPY "Services/Catalog/Catalog.csproj" "Services/Catalog/Catalog.csproj"

COPY "Services/Basket/Basket.csproj" "Services/Basket/Basket.csproj"

COPY "Services/User/User.csproj" "Services/User/User.csproj"

COPY "Gateway/Gateway.csproj" "Gateway/Gateway.csproj"

COPY "UnitTests/UnitTests.csproj" "UnitTests/UnitTests.csproj"

RUN dotnet restore ShopExample.sln

COPY "./Common" "./Common"
COPY "./Services/Catalog" "./Services/Catalog"
COPY "./Services/Basket" "./Services/Basket"
COPY "./Services/User" "./Services/User"
COPY "./Gateway" "./Gateway"
COPY "./UnitTests" "./UnitTests"

RUN dotnet publish --no-restore -c Release -o out

COPY "./Services/Basket/appsettings.Production.json" "out/appsettings.Production.json"

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Basket.dll"]