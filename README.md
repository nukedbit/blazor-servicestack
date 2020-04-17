# Blazor Server Size with Service Stack

.NET Core 3.1 Blazor Server Size with Service Stack

Based on [selfhost template](https://github.com/NetCoreTemplates/selfhost)

This Template is pre configured to work with ServiceStack, it provide an extension method which allow you to make authenticated call using a ServiceStack client.

This project use ServiceStack integrated authentication bridged over asp.net auth, so it can work seamless with asp.net core.

As an example i have converted the Forecast Service class in a ServiceStack Service which require authentication, so when you navigate to Fetch Data page it require you to login with the sample user.


![](https://raw.githubusercontent.com/nukedbit/blazor-servicestack/master/blazor-servicestack.jpg)


> Browse [source code]https://github.com/nukedbit/blazor-servicestack/), and install with [dotnet-new](https://docs.servicestack.net/dotnet-new):

    $ dotnet tool install -g x

    $ x new nukedbit/blazor-servicestack ProjectName

