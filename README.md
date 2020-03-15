# Blazor Server Size with Service Stack

.NET Core 3.1 Blazor Server Size with Service Stack

Based on [selfhost template](https://github.com/NetCoreTemplates/selfhost)

This Template Provide a **ServiceStackComponentBase** you can set your razor components to inherit from it, this way you can access IServiceGateway to call your ServiceStack Services from your components and also a Db property to access OrmLite Directly if you wish to do so.
There is also a ServiceStackAuthenticationStateProvider which enable direct integration of ServiceStack Authentication System replacing the asp.net one so you don't require to use entityframework for authentication.

As an example i have converted the Forecast Service class in a ServiceStack Service which require authentication, so when you navigate to Fetch Data page it require you to login with the sample user.


![](https://raw.githubusercontent.com/nukedbit/blazor-servicestack/master/blazor-servicestack.jpg)


> Browse [source code]https://github.com/nukedbit/blazor-servicestack/), and install with [dotnet-new](https://docs.servicestack.net/dotnet-new):

    $ dotnet tool install -g x

    $ x new nukedbit/blazor-servicestack ProjectName

