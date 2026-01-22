# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY DuckSimulatorApp.csproj ./
RUN dotnet restore

COPY . ./

# Publish to multiple runtimes
RUN dotnet publish -c Release -r linux-x64   --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true -o /out/linux-x64
RUN dotnet publish -c Release -r osx-arm64   --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true -o /out/osx-arm64
RUN dotnet publish -c Release -r win-x64     --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true -o /out/win-x64

# ---- Artifact stage ----
FROM alpine:3.20
WORKDIR /artifacts
COPY --from=build /out ./out
CMD ["sh", "-c", "find /artifacts/out -maxdepth 2 -type f | sed 's|^|ARTIFACT: |'"]
