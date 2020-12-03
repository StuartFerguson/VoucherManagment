FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["VoucherManagement/VoucherManagement.csproj", "VoucherManagement/"]
RUN dotnet restore "VoucherManagement/VoucherManagement.csproj"
COPY . .
WORKDIR "/src/VoucherManagement"
RUN dotnet build "VoucherManagement.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VoucherManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VoucherManagement.dll"]