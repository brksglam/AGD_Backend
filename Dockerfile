# .NET SDK ile uygulamayı build et
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Fresh.Api/Fresh.Api.csproj", "Fresh.Api/"]
RUN dotnet restore "Fresh.Api/Fresh.Api.csproj"

COPY . .
WORKDIR /src/Fresh.Api
RUN dotnet publish -c Release -o /app

# Runtime ortamı
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
CMD ["dotnet", "Fresh.Api.dll"]
