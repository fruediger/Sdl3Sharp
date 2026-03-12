# SDL3#

[![GitHub Release](https://img.shields.io/github/v/release/fruediger/Sdl3Sharp?logo=github&label=GitHub%20Release)](https://github.com/fruediger/Sdl3Sharp/releases/latest)

[![NuGet Version](https://img.shields.io/nuget/v/Sdl3Sharp?logo=nuget&label=NuGet%20Package)](https://www.nuget.org/packages/Sdl3Sharp/latest)

[![SDL Native Library](https://img.shields.io/badge/dynamic/json?url=https%3A%2F%2Fraw.githubusercontent.com%2Ffruediger%2FSdl3Sharp%2Frefs%2Fheads%2Fmain%2Fmake.json&query=%24.runtimesVersion&prefix=v&logo=data%3Aimage%2Fsvg%2Bxml%3Bbase64%2CPD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4KPHN2ZyB2ZXJzaW9uPSIxLjEiIHZpZXdCb3g9IjAgMCA0OCA0OCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48cGF0aCB0cmFuc2Zvcm09Im1hdHJpeCguMDY1MSAwIDAgLjA2NTEgLTQ3LjMgLTUuNjcpIiBkPSJtMTMyNCAyNzFjLTAuMTYzIDAgMi45NyAxLjc4IDYuOTcgMy45NiAxMS45IDYuNTIgMjIuOCAxMy41IDMyLjggMjAuNy0wLjE3OSAwLjMxMi0wLjMgMC42NDgtMC4zIDEuMDIgMCAxLjI4IDEuMTkgMi4zNCAyLjY3IDIuMzQgMC41ODQgMCAxLjA5LTAuMjA2IDEuNTMtMC40OCA2Ljg3IDUuMTMgMTMuMyAxMC40IDE5LjIgMTUuOS0wLjM3MiAwLjUyOS0wLjYzIDEuMTEtMC42MyAxLjc3IDAgMS45MyAxLjggMy41MSA0LjAyIDMuNTEgMC42MjEgMCAxLjE5LTAuMTQyIDEuNzEtMC4zNiA2LjAxIDUuOTIgMTEuNSAxMiAxNi40IDE4LjMtMC44NTcgMC45MDQtMS40NCAyLjAxLTEuNDQgMy4yNyAwIDIuODkgMi43IDUuMjUgNi4wMyA1LjI1IDAuNTc4IDAgMS4wOS0wLjE2OSAxLjYyLTAuMyAyLjMzIDMuMzYgNC41OCA2Ljc1IDYuNjEgMTAuMiAzLjE3IDUuNCA1Ljg2IDEwLjkgOC4yIDE2LjQtMi40OCAxLjI4LTQuMjMgMy41My00LjIzIDYuMjIgMCA0LjA1IDMuOCA3LjMzIDguNDcgNy4zMyAwLjIwMyAwIDAuMzcxLTAuMDc3OSAwLjU3LTAuMDkwMSAzLjM4IDExLjQgNS4wOCAyMy4xIDUuMDEgMzQuNy02LjA1IDAuNDUzLTEwLjggNC44LTEwLjggMTAuMiAwIDQuODIgMy44NCA4LjgzIDkuMDEgOS45NC0yLjE0IDEyLjEtNi4xNiAyNC4xLTEyIDM1LjgtMS41LTAuNDI1LTMuMDgtMC43MjEtNC43NC0wLjcyMS04LjUgMC0xNS40IDUuOTktMTUuNCAxMy40IDAgNC4yNCAyLjMzIDcuOTcgNS44OCAxMC40LTEuMTUgMS41NS0yLjI0IDMuMTItMy40NSA0LjY1LTcuMzkgOS4zNC0xNi4xIDE4LjMtMjUuNiAyNi45LTEuOTctMS4yNi00LjM5LTIuMDQtNy4wMy0yLjA0LTYuNTQgMC0xMS44IDQuNi0xMS44IDEwLjMgMCAyLjEyIDAuNzQ1IDQuMDcgMi4wMSA1LjcgMC4wMDUyIDAuMDA2ODEtMC4wMDUzIDAuMDIzMiAwIDAuMDMtMTEuNSA4LjczLTI0LjMgMTYuOS0zOCAyNC42LTEuOTItMS42Ni00LjU4LTIuNjctNy41MS0yLjY3LTUuODggMC0xMC42IDQuMTQtMTAuNiA5LjI1IDAgMC45MTcgMC4xOTQgMS43OCAwLjQ4IDIuNjEtMi4xOSAxLjA3LTQuMzEgMi4yLTYuNTUgMy4yNC0xMS42IDUuMzktMjMuNiAxMC4zLTM2LjEgMTQuOC0xLjczLTIuMTctNC42Mi0zLjYtNy45LTMuNi01LjMgMC05LjYxIDMuNzItOS42MSA4LjMyIDAgMC40IDAuMDg3MiAwLjc4NiAwLjE1IDEuMTctMTUuNSA0Ljk4LTMxLjQgOS40LTQ3LjkgMTMuMS0xLjQtMS42MS0zLjUzLTIuNzMtNi4wNC0yLjczLTMuNzUgMC02LjcxIDIuMzYtNy4zOSA1LjQzLTE2LjYgMy4yNy0zMy42IDUuOS01MC45IDcuODEtMS4xLTEuNDMtMi44OC0yLjQzLTUuMDEtMi40My0yLjY1IDAtNC44OCAxLjQ3LTUuNzMgMy41MS0xNy42IDEuNjMtMzUuMyAyLjQ4LTUyLjkgMi42Ny0wLjY2LTEuNjgtMi40NC0yLjg4LTQuNTktMi44OC0yLjE4IDAtMy45NSAxLjIzLTQuNTkgMi45NC0xNy43LTAuMDYyMi0zNS40LTAuODA3LTUyLjgtMi4zMS0wLjMyNC0xLjYtMS44NS0yLjgyLTMuNzgtMi44Mi0xLjY1IDAtMy4wNSAwLjg4MS0zLjYzIDIuMTMtMTYuNi0xLjY0LTMzLjEtMy45Ni00OS4yLTYuOTEtMC4wNzU2LTEuNDUtMS40NC0yLjYxLTMuMTItMi42MS0xLjIxIDAtMi4yMSAwLjYwNi0yLjczIDEuNDctNC4yOC0wLjgzLTguNTktMS42LTEyLjgtMi41Mi0xMS40LTIuNDgtMjIuNy01LjQtMzMuOC04LjY1IDAuMDE2MS0wLjA5OTcgMC4wNi0wLjE5NyAwLjA2LTAuMyAwLTEuMi0xLjExLTIuMTYtMi40OS0yLjE2LTAuOTIzIDAtMS43MiAwLjQzOS0yLjE2IDEuMDgtMjUuMy03LjY1LTQ5LjMtMTcuMS02OS44LTI3LjUtMi41LTEuMjgtNC41My0yLjIyLTQuNTMtMi4xMyAwIDAuMjYgMTIuOSA3LjQyIDE4LjggMTAuNCA2Mi43IDMyLjEgMTQzIDUxLjMgMjI4IDU0LjIgMjUuNSAwLjg3MyA1NC45IDAuMTI3IDgwLTIuMDcgOTIuNC04LjEgMTc2LTM1LjQgMjM0LTc2LjYgMzUuOC0yNS4zIDYwLjMtNTQuNiA3Mi4zLTg2LjQgOC45LTIzLjYgMTAuMi01MC4zIDMuNDgtNzQuNC03LjIyLTI2LTIyLTQ5LjQtNDUuNS03Mi4xLTE4LjctMTguMS00MC4xLTMzLjMtNjcuMS00Ny42LTUuNTItMi45My0yMy4yLTExLjQtMjMuNy0xMS40em0tNTAzIDE5LjJjLTI2LjYgMC00OC44IDYuMTctNjYuNiAxOC41LTE3LjggMTIuNC0yNi43IDI4LjUtMjYuNyA0OC41IDAgMTQuOCAzLjc1IDI3LjEgMTEuMyAzNi45IDcuNTMgOS44MiAyMCAxNy43IDM3LjQgMjMuNSA3LjU0IDIuNDggMTUuNyA0LjU2IDI0LjMgNi4yOCA4LjY5IDEuNzIgMTYuOCAzLjU2IDI0LjUgNS40NiA2LjEyIDEuNTMgMTEuNCAzLjk2IDE1LjggNy4zIDQuNDIgMy4zNCA2LjY0IDcuNjkgNi42NCAxMyAwIDQuNzctMS4zNyA4LjU5LTQuMDggMTEuNS0yLjcxIDIuOTEtNi4wMiA1LjE5LTkuOTQgNi44Mi0zLjIxIDEuNDMtNy40NiAyLjQ4LTEyLjcgMy4xNS01LjI3IDAuNjY3LTkuNTIgMC45OTEtMTIuNyAwLjk5MS0xMi44IDAtMjUuOS0yLjQzLTM5LjUtNy4zLTEzLjYtNC44Ny0yNi0xMS44LTM3LjItMjAuOWgtNS4xM3Y1MC43YzExLjEgNC42OCAyMy4yIDguNTkgMzYuMiAxMS43IDEzIDMuMTUgMjguMSA0Ljc0IDQ1LjMgNC43NHYtMC4wM2MyOS44IDAgNTMuNS02LjQzIDcxLTE5LjMgMTcuNS0xMi45IDI2LjMtMjkuOSAyNi4zLTUxLjEgMC0xNC44LTMuNzktMjYuNy0xMS40LTM1LjctNy41OS05LjAyLTE5LjItMTYuMi0zNC43LTIxLjYtNy45NC0yLjY3LTE1LjQtNC43OC0yMi40LTYuMzEtNi45OS0xLjUzLTE0LjItMy4wOS0yMS44LTQuNzEtMTEuNi0yLjU4LTE5LjUtNS40NS0yMy42LTguNjUtNC4wNy0zLjE5LTYuMS03LjUzLTYuMS0xMyAwLTMuNzIgMS4yOC03LjA4IDMuODQtMTAuMXM1LjYxLTUuMjYgOS4xMy02Ljc5YzQuMzItMS45MSA4LjQyLTMuMiAxMi4zLTMuODcgMy45Mi0wLjY2NyA4LjM1LTAuOTkxIDEzLjMtMC45OTEgMTIuNyAwIDI1IDIuMzIgMzcgNyAxMiA0LjY4IDIyLjIgMTAuNCAzMC41IDE3LjJoNC45NXYtNDguN2MtMTAuNC00LjM5LTIyLjMtNy44Ny0zNS41LTEwLjQtMTMuMi0yLjU4LTI2LjUtMy44Ny0zOS44LTMuODd6bTk0LjQgMy45M3YyMTNoNzAuMmMxNS41IDAgMzAuMS0xLjAxIDQzLjgtMy4wNiAxMy43LTIuMDUgMjYuNC02LjY1IDM4LTEzLjggMTQtOC40IDI1LjUtMjAuNCAzNC43LTM2LjEgOS4xMy0xNS43IDEzLjctMzMuNSAxMy43LTUzLjN2LTAuMDNjLTAuMDA0Mi0yMC42LTQuMjYtMzguNS0xMi44LTUzLjctOC41NC0xNS4yLTIwLjItMjcuMy0zNS0zNi4zLTEyLjEtNy4zNC0yNS0xMi0zOC43LTEzLjktMTMuOC0xLjkxLTI4LjUtMi44NS00NC4zLTIuODVoLTY5LjZ6bTIxNSAwLjA2djIxM2gxNTB2LTQwLjZoLTkzLjR2LTE3MmgtNTYuNHptLTE1OSAzOS43aDEuMDVjMTEuNiAwIDIxLjcgMC4xODkgMzAuMSAwLjU3IDguMzkgMC4zODIgMTYuNCAyLjYgMjQgNi42MSAxMC42IDUuNjMgMTguMyAxMy40IDIzLjQgMjMuNCA1LjA3IDkuOTcgNy42MyAyMiA3LjYzIDM2LTFlLTMgMTQtMi40MSAyNS45LTcuMjQgMzUuNi00LjgyIDkuNzMtMTEuOSAxNy40LTIxLjEgMjIuOS03Ljg0IDQuNjgtMTYuMSA3LjIzLTI0LjggNy42Ni04LjY5IDAuNDMtMTkuNCAwLjYzMS0zMiAwLjYzMWgtMS4wNXYtMTMzeiIgZmlsbD0iI2ZmZiIvPjwvc3ZnPgo%3D&label=SDL%20Native%20Library)](https://github.com/fruediger/SDL-native)

SDL3# is a C# language binding for [SDL3](https://www.libsdl.org/).

In contrast to the [promoted C# bindings (SDL3-CS)](https://github.com/flibitijibibo/SDL3-CS), SDL3# is entirely hand-crafted, with no auto-generation of API code based on the native library.\
Every part of the API is deliberately designed to feel native to C#, translating SDL's functionality into idiomatic, well thought-out counterparts that existing C# users should feel right at home with.\
It makes heavy use of modern C# features to provide an API that is expressive and comfortable to work with from managed code, while still exposing the full breadth of SDL's functionality.

> [!WARNING]
> This project is a work in progress and is not yet complete or usable in production. The public .NET API is subject to change at any time and in any form without prior notice. Use at your own risk.

## Building

For instructions on how to build the project from source, see [BUILDING.md](BUILDING.md).

## Usage

### Requirements

- C# 14 or later
- .NET 10 or later

### Adding SDL3# as a dependency

#### Via your IDE

*(Example: Visual Studio)*

Open the NuGet Package Manager, search for `Sdl3Sharp`, check "Include prerelease", and install the latest version.

#### Via `.csproj`

```xml
<PackageReference Include="Sdl3Sharp" Version="*-*" />
```

#### Via a file-based C# app

Add the following directive at the top of your file:

```csharp
#:package Sdl3Sharp@*-*
```

### Choosing a package variant

The default `Sdl3Sharp` package includes pre-built native SDL3 binaries for all supported platforms and is the easiest way to get started.

If you only need to target specific platforms, you can reference the corresponding RID-specific packages instead, for example `Sdl3Sharp.win-x64` or `Sdl3Sharp.linux-x64`, rather than pulling in binaries for every platform.

If you do not want any native binaries bundled at all, reference `Sdl3Sharp.Core` instead. In that case you are responsible for providing the native SDL3 and libffi binaries yourself, placed alongside the resulting executable of your application.

### Examples

#### Hello World

```csharp
MessageBox.TryShowSimple(MessageBoxFlags.Information, "Hello World", "Hello World from SDL3#!");
```

#### Rendering a triangle

```csharp
using var sdl = new Sdl(static builder => builder
    .SetAppName("Simple SDL3# Triangle example")
    .InitializeSubSystems(SubSystems.Video)
);

return sdl.Run(new App(), args);

class App : AppBase
{
    private Window mWindow = default!;
    private Renderer mRenderer = default!;

    protected override AppResult OnInitialize(Sdl sdl, string[] args)
    {
        if (!Window.TryCreateWithRenderer("Hello World", 800, 600, out mWindow!, out mRenderer!))
        {
            return Failure;
        }

        return Continue;
    }

    protected override AppResult OnIterate(Sdl sdl)
    {
        mRenderer.DrawColorFloat = (0, 0, 0, 1);
        mRenderer.TryClear();

        mRenderer.TryRenderGeometry([
            new Vertex(position: (400, 150), color: (1, 0, 0, 1), texCoord: default),
            new Vertex(position: (200, 450), color: (0, 1, 0, 1), texCoord: default),
            new Vertex(position: (600, 450), color: (0, 0, 1, 1), texCoord: default)
        ]);

        mRenderer.TryRenderPresent();

        return Continue;
    }

    protected override AppResult OnEvent(Sdl sdl, ref Event @event)
    {
        if (@event.Type is EventType.WindowCloseRequested)
        {
            return Success;
        }

        return Continue;
    }

    protected override void OnQuit(Sdl sdl, AppResult result)
    {
        mRenderer?.Dispose();
        mRenderer = default!;

        mWindow?.Dispose();
        mWindow = default!;
    }
}
```

The example above makes use of the `AppBase` lifetime model, where SDL3# manages the main loop for you. You implement `OnInitialize`, `OnIterate`, and `OnEvent` as callbacks, and SDL3# takes care of the rest.

If you prefer to manage the main loop yourself, you can do so: you still initialize SDL via `new Sdl(...)` and pump events on your own, without using `AppBase` at all.

## A note on AI usage

In the spirit of transparency, and in line with the [contributing guidelines](CONTRIBUTING.md), here is an overview of how AI was used in this project:

- **Documentation.** AI was used to help write and improve documentation. The content itself comes from the author, but AI was used to clarify and clean up the writing.
- **API design.** AI was used as a sounding board for API design decisions. Sometimes the options to consider came from the author, sometimes the AI had useful proposals to offer. All decisions were ultimately made by the author.
- **Infrastructural documents.** AI was used to help write project documents such as this [README.md](README.md), the [BUILDING.md](BUILDING.md), the [CONTRIBUTING.md](CONTRIBUTING.md), and the [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md).
- **Code review.** AI was used to review some of the author's code, and occasionally this turned out to be fruitful, catching bugs that might otherwise have been overlooked.
- **Functional code.** No AI was used to write any functional code. All code in this project was written by the author.

## License

SDL3# is licensed under the [MIT License](./LICENSE.md). See [NOTICE.md](./NOTICE.md) for third-party notices.
