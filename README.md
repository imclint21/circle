<p align="center">
  <p align="center">
    <img src="https://user-images.githubusercontent.com/5221349/78506525-4ffe4200-777a-11ea-9666-b73e2779aa23.png" height="100" alt="Circle" />
  </p>
  <h3 align="center">
    About Circle
  </h3>
  <p align="center">
    .Net Core 3 Manager Background Service.
  </p>
</p>

## Introduction

Circle is a .Net Core 3 Manager Background Service, that expose an API to handle it.

You simply need to add `AddCircle()` in the services collection.

### Startup.cs

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddCircle(options =>
    {
        options.Period = TimeSpan.FromSeconds(5);
        options.UseHandler<Work>();
    });
}
```

### Create a Job

You need to create a new class that inherits from `IWorkHandler`, and write the task that need to be executed.

```csharp
public class Work : IWorkHandler
  {
      public void DoWork()
      {
          Console.WriteLine("Test!");
      }
  }
  ```
