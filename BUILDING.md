# Building SDL3#

This project uses [make.cs](https://github.com/fruediger/make.cs) as its build tool. All build and packaging tasks should be performed through the provided wrapper scripts rather than invoking `dotnet build` directly.

## Requirements

- .NET 10 SDK or later
- The `dotnet` CLI must be available in your PATH

## Running the build tool

Invoke the build tool using one of the following wrapper scripts, depending on your platform:

**Unix-like systems:**

```shell
./make.sh <subcommand> [options]
```

If you get a permission error, run `chmod +x make.sh` first.

**PowerShell:**

```shell
./make.ps1 <subcommand> [options]
```

**Windows CMD:**

```shell
make.cmd <subcommand> [options]
```

All wrapper scripts forward their arguments directly to `make.cs`.

## Native library binaries

By default, the build tool downloads pre-built SDL3 native library binaries from the [SDL-native](https://github.com/fruediger/SDL-native) releases. This is configured via the `runtimesUrl` property in `make.json`, or equivalently via the `--runtimes-url` CLI option.

If you want to use a custom build of SDL3 instead, pass `--runtimes-url` with a URL pointing to your own archive, or update the `runtimesUrl` property in `make.json` accordingly. Local paths are supported using `file://` URLs.

> [!NOTE]
> `cacheDir` and `tempDir` in `make.json` must remain subdirectories of `./src` for `Directory.Build.props` and `Directory.Build.targets` to work correctly. Avoid changing these unless you know what you are doing.

## Commands

For a full list of available options, run `./make.sh --help` or `./make.sh <subcommand> --help`. You can also refer to the [make.cs repository](https://github.com/fruediger/make.cs) for further documentation.

### build

Builds the managed project.

```shell
./make.sh build
```

| CLI option  | Config property | Description                                      |
|-------------|-----------------|--------------------------------------------------|
| `--project` | `project`       | Path to a `.csproj` or directory containing one. |
| `--no-logo` | `noLogo`        | Suppress the startup banner.                     |

### pack

Produces NuGet packages: a core package, RID-specific packages containing the native binaries, and a meta package.

```shell
./make.sh pack
```

| CLI option           | Config property   | Description                                                       |
|----------------------|-------------------|-------------------------------------------------------------------|
| `--output-dir`       | `outputDir`       | Output directory (default: `./build`).                            |
| `--runtimes-version` | `runtimesVersion` | Version of the native runtime assets to download.                 |
| `--runtimes-url`     | `runtimesUrl`     | URL or format string for the runtime archive. Supports `file://`. |
| `--targets`          | (no config)       | Which flavors to pack: `core`, `meta`, specific RIDs, or `all`.   |
| `--no-symbols`       | (no config)       | Skip generating a symbols package for the core package.           |
| `--strict`           | (no config)       | Fail if a requested RID has no matching runtime archive.          |