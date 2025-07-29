# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /app

# Copy solution and restore
COPY *.sln .
COPY ConsoleApp/*.csproj ./ConsoleApp/
COPY Application/*.csproj ./Application/
COPY Domain/*.csproj ./Domain/
COPY Infrastructure/*.csproj ./Infrastructure/
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish ConsoleApp/ConsoleApp.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:9.0-preview
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "ConsoleApp.dll"]
