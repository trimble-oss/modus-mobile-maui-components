<p align="center">
  <a href="https://www.nuget.org/packages/Trimble.Modus.Components/">
    <img src="https://img.shields.io/nuget/v/Trimble.Modus.Components" alt/>
  </a>
</p>

The [Trimble Modus Design System](https://modus.trimble.com/) describes the UX that Trimble wants to provide in its UI across its many applications. The benefits of using Modus include rapid prototyping, development efficiency, and UX consistency.

Modus includes:

- Components

This library provides Modus Elements as reusable, encapsulated mobile UI components. These can be implemented in any mobile app built with .NET MAUI. The modus-mobile-components library was built using the latest mobile UX specs and guidelines from Figma.

# Looking for documentation?

You can check out [Modus documention](https://modus-mobile.trimble.com) for the library's latest developer documentation.

You can also check out [Mobile Components](https://modus.trimble.com/components/mobile/) for styleguide/recommendataion and UX related information about all the mobile components

# Getting Started

### Installing Modus Mobile Components

1. In your dotnet MAUI application, right click on the Dependencies -> Manage Nuget Packages and search and choose the `Trimble.Modus.Components` and click the Add Package
   Here’s the direct link: [Trimble.Modus.Components](https://www.nuget.org/packages/Trimble.Modus.Components)

1. Add the following using statement in your MauiProgram:

   `using Trimble.Modus.Components.Hosting;`

1. Then register the handlers to use the Modus components

```
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseTrimbleModus()
            // register other handlers
        return builder.Build();
    }
}
```

Now you can use the controls in your app.\
For more information check out the [Getting Started page](https://modus-mobile.trimble.com/getting-started/introduction/).

# Contributing

Curious about contributing? We've got a [contributing guide](CONTRIBUTING.md) to help get you going.

# Release a new version

1. Update the CHANGELOG
2. Open the Trimble.Modus.Components.csproj file and edit the version number and save the project
3. Build the components project in release mode using the following command
   `dotnet build -c:Release`
   This will generate a .nupkg file in the `/Trimble.Modus.Components/bin/Release` location
4. Open the nuget.org site and upload the created nupkg file
