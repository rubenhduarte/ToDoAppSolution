# Etapa base: imagen para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:9.0.0-noble-amd64 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa build: compila y restaura el proyecto
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar los archivos de proyecto necesarios
COPY ["TodoApp.Domain/TodoApp.Domain.csproj", "TodoApp.Domain/"]
COPY ["TodoApp.Infrastructure/TodoApp.Infrastructure.csproj", "TodoApp.Infrastructure/"]
COPY ["TodoApp.Application/TodoApp.Application.csproj", "TodoApp.Application/"]
COPY ["TodoApp.WebApi/TodoApp.WebApi.csproj", "TodoApp.WebApi/"]

# Restaurar dependencias
RUN dotnet restore "TodoApp.WebApi/TodoApp.WebApi.csproj"

# Copiar todo el código
COPY . .

# Compilar el proyecto Web API en modo Release
WORKDIR "/src/TodoApp.WebApi"
RUN dotnet build "TodoApp.WebApi.csproj" -c Release -o /app/build

# Etapa publish: publicar la aplicación
FROM build AS publish
WORKDIR "/src/TodoApp.WebApi"
RUN dotnet publish "TodoApp.WebApi.csproj" -c Release -o /app/publish

# Etapa final: crear la imagen de ejecución
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApp.WebApi.dll"]