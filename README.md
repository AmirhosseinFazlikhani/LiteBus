# BasicBus
![.NET 5 CI](https://github.com/mshfzd/BasicBus/workflows/.NET%20Core/badge.svg)


A lightweight and easy to use in-process mediator to implement CQRS

* Written in .NET 5
* No dependencies
* Supports both asynchronous and synchronous command/query handling
* Supports streaming query (IAsyncEnumerable)
* Supports command handling with command result

## Installation and Configuration in ASP.NET Core 

Install with NuGet:

```
Install-Package BasicBus
Install-Package BasicBus.Extensions.Microsoft.DependencyInjection
```

or with .NET CLI:

```
dotnet add package BasicBus
dotnet add package BasicBus.Extensions.Microsoft.DependencyInjection
```

and configure the BasicBus as below in the `ConfigureServices` method of `Startup.cs`:

```c#
services.AddBasicBus(builder =>
{
    builder.RegisterHandlers(typeof(Startup).Assembly);
});
```
