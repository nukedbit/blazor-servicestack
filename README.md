# Blazor Server Size with Service Stack

.NET 6 Blazor Server Size with Service Stack.

This template has been updated for .NET 6 preview 4, and there is a new way to interact with ServiceStack services.
On this template there is a prototype of a ServiceStack Client Proxy which emulate IRequest by providing a fake request, to make AutoCrud work.


Based on [selfhost template](https://github.com/NetCoreTemplates/selfhost)

This Template is pre configured to work with ServiceStack, it provide an extension method which allow you to make authenticated call using a ServiceStack client.

This project use ServiceStack integrated authentication bridged over asp.net auth, so it can work seamless with asp.net core.

As an example i have converted the Forecast Service class in a ServiceStack Service which require authentication, so when you navigate to Fetch Data page it require you to login with the sample user.


![](https://raw.githubusercontent.com/nukedbit/blazor-servicestack/master/blazor-servicestack.jpg)


> Browse [source code]https://github.com/nukedbit/blazor-servicestack/), and install with [dotnet-new](https://docs.servicestack.net/dotnet-new):

    $ dotnet tool install -g x

    $ x new nukedbit/blazor-servicestack ProjectName

