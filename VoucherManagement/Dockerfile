FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
RUN apt-get update
RUN apt-get install -y apt-utils
RUN apt-get install -y libgdiplus
RUN ln -s /usr/lib/libgdiplus.so /usr/lib/gdiplus.dll
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["VoucherManagement/NuGet.Config", "."]
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