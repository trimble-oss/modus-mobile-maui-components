# 🔨👷 Contributing

---

## Table of Contents

- [Dependencies](#dependencies)
- [Getting Started](#getting-started)
- [Submitting Issues](#submitting-issues)
- [Changelog](#changelog)
- [Queries](#queries)

## Dependencies
In your dotnet MAUI application, right click on the Dependencies -> Manage Nuget Packages and search and choose the Trimble.Modus.Components and click the Add Package
Here’s the direct link: Trimble.Modus.Components

Add the following using statement in your MauiProgram:
using Trimble.Modus.Components.Hosting;
Then register the handlers to use the Modus components
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
Now you can use the controls in your app.
## Getting Started
- Install Git - https://git-scm.com/book/en/v2/Getting-Started-Installing-Git
- Install GitHub Desktop - https://docs.github.com/en/desktop/installing-and-configuring-github-desktop/installing-and-authenticating-to-github-desktop/installing-github-desktop
- Install Visual Studio - [https://visualstudio.microsoft.com/vs/](https://learn.microsoft.com/en-us/dotnet/maui/get-started/installation?tabs=visual-studio-code)
- Install .Net - https://dotnet.microsoft.com/en-us/download/dotnet/7.0
- Run the Demo App - https://dotnet.microsoft.com/en-us/learn/maui/first-app-tutorial/device-setup
## Submitting Issues
Whether you're contributing directly to the code or have suggestions, submitting an issue through GitHub is needed for referencing changes. Please submit change items as an Issue [here](https://github.com/trimble-oss/modus-mobile-maui-components/issues).

If the issue's considered a bug, add the 'bug' label to the issue.

Also add a priority level label to the issue. The priority options are low, medium, and high. If you are unsure of its priority, reach out to one of the developers for their opinion.

## Changelog

The release notes are present in this [Google Drive](https://drive.google.com/drive/u/0/folders/1cv7LPemnfGGndV1-adQfvpe_09iiz8sW) page.

## Queries

For queries contact via [modus-mobile-contributors-ug@trimble.com]
