FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

COPY Munt.Advances.Api ./Munt.Advances.Api

RUN dotnet restore Munt.Advances.Api/Munt.Advances.Api.csproj
RUN dotnet publish Munt.Advances.Api/Munt.Advances.Api.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /out ./
EXPOSE 5000
ENTRYPOINT ["dotnet", "Munt.Advances.Api.dll"]