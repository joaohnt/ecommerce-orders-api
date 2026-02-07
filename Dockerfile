FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .

FROM build AS publish-api
WORKDIR /src/Ecommerce.Api
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish/api

FROM build AS publish-worker
WORKDIR /src/Ecommerce.Worker
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish/worker

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS api
WORKDIR /app
EXPOSE 8080
COPY --from=publish-api /app/publish/api .
ENTRYPOINT ["dotnet", "Ecommerce.Api.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS worker
WORKDIR /app
COPY --from=publish-worker /app/publish/worker .
ENTRYPOINT ["dotnet", "Ecommerce.Worker.dll"]
