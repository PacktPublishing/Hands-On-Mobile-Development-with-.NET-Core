FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["NetCore.Web.UsersApi/NetCore.Web.UsersApi.csproj", "NetCore.Web.UsersApi/"]
COPY ["NetCore.Data.Cosmos/NetCore.Data.Cosmos.csproj", "NetCore.Data.Cosmos/"]
COPY ["NetCore.Data/NetCore.Data.csproj", "NetCore.Data/"]
RUN dotnet restore "NetCore.Web.UsersApi/NetCore.Web.UsersApi.csproj"
COPY . .
WORKDIR "/src/NetCore.Web.UsersApi"
RUN dotnet build "NetCore.Web.UsersApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NetCore.Web.UsersApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NetCore.Web.UsersApi.dll"]