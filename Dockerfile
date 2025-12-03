FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore

RUN dotnet tool install --global dotnet-ef --version 9.0.0

RUN dotnet publish ASPNET-PROJECT.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

COPY --from=build /src/Migrations ./Migrations

COPY --from=build /root/.dotnet/tools /root/.dotnet/tools
ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["sh", "-c", "dotnet ef database update --no-build && dotnet ASPNET-PROJECT.dll"]

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000