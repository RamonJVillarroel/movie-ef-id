# movie-ef-mvc

Aplicación web Razor Pages para gestionar y mostrar información de películas con integración a un servicio LLM (OpenAI) para generar resúmenes y pequeños spoilers. El proyecto utiliza Entity Framework Core para persistir datos y está desarrollado con .NET 9 y C# 13.

## Descripción

La aplicación permite ver un catálogo de películas, añadirlas a favoritos y reseñarlas. Además, el administrador puede realizar operaciones CRUD sobre películas, categorías y plataformas.

## Características principales

- Interfaz Razor Pages para CRUD de películas.
- Persistencia con Entity Framework Core.
- Servicio `LlmService` que consulta la API de OpenAI para generar:
  - Resúmenes breves (`ObtenerResumenAsync`), sin spoilers importantes.
  - Pequeños spoilers controlados (`ObtenerSpoilerAsync`).

## Requisitos

- .NET 9 SDK
- Visual Studio 2022 (recomendado) o `dotnet` CLI
- Clave de API de OpenAI

## Configuración

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/RamonJVillarroel/movie-ef-id.git
   cd movie-ef-id
   ```

2. Configurar la clave de OpenAI como variable de entorno (requerido por `LlmService`):
   - Windows (PowerShell):
     ```powershell
     setx OPENAI_API_KEY "tu_clave_aqui"
     ```
   - macOS / Linux (bash):
     ```bash
     export OPENAI_API_KEY="tu_clave_aqui"
     ```
 
   Alternativa en desarrollo: usar _user secrets_ de .NET o `appsettings.Development.json` (no subir nunca claves a control de versiones).

3. Configurar la cadena de conexión en `appsettings.json` o `appsettings.Development.json` bajo `ConnectionStrings:DefaultConnection`.

   Ejemplo (LocalDB SQL Server):
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MovieDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

## Migraciones y base de datos (EF Core)

Desde la carpeta del proyecto que contiene el `DbContext`:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

En Visual Studio: usar la _Package Manager Console_ con los mismos comandos.

## Ejecutar la aplicación

- Visual Studio 2022: abrir la solución, establecer el proyecto como startup y ejecutar (F5) o `Ctrl+F5`.
- dotnet CLI:
   ```bash
   cd src/<ProjectFolder>
   dotnet run
   ```
  
  La aplicación escuchará en los puertos configurados en `Properties/launchSettings.json`.

## Uso del servicio `LlmService`

- `LlmService` obtiene la clave desde `OPENAI_API_KEY` y usa por defecto el modelo `gpt-4o-mini`.
- Métodos:
  - `ObtenerResumenAsync(string tituloPelicula)` — resumen breve sin spoilers.
  - `ObtenerSpoilerAsync(string tituloPelicula)` — pequeño spoiler (2-3 oraciones).

- Errores comunes:
  - Si `OPENAI_API_KEY` no está configurada, el servicio lanza `InvalidOperationException`.
  - Manejar límites de la API y errores de red en producción.

## Buenas prácticas y seguridad

- No subir claves ni secretos al repositorio.
- Usar variables de entorno o _user secrets_ en desarrollo.
- Revisar y limitar el uso de la API (costos y rate limits).