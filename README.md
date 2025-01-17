# AACore Web Backend

AACore.Web's Backend.  
Built with .NET 9 and ASP.NET Core Minimal API, and compiled with Native AOT for blazingly fast launch time, small executable file (<20MB) size and memory footprint(~20MB).  

## How to compile?

```powershell
dotnet publish -p:PublishProfile=Native-win-x64
```

> [!WARN]  
> You can run directly using `dotnet run` or debug in your favorite IDE.  
> Publish in Visual Studio may not work. Use the command above instead.  

> [!INFO]  
> You may need following Visual Studio 2022 workload to compile this project with Native AOT:  
> - ASP.NET and web development  
> - .NET desktop development  
> - Desktop development with C++  

## How to use it?

Compile & open `.\AACore.Web\publish\win-x64\AACore.Web.exe`.  
The backend will listen on `http://localhost:7843` (This can be configured in appsettings.json)  
This backend has auto-generated OpenAPI document, and an integrated Scalar for directly browse and test the api set.  

```
http://localhost:7843/openapi/v1.json   # Auto-generated OpenAPI document
http://localhost:7843/scalar/v1         # Scalar
```