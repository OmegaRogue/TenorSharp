# TenorSharp
![Build Status][4]
[![GNU LGPL licensed][2]][3]
[![Nuget][5]][6]
[![Discord][7]][8]

[1]: https://img.shields.io/github/license/OmegaRogue/TenorSharp
[2]: https://img.shields.io/github/license/OmegaRogue/TenorSharp
[3]: LICENSE
[4]: https://img.shields.io/github/workflow/status/OmegaRogue/TenorSharp/.NET%20Core
[5]: https://img.shields.io/nuget/v/TenorSharp
[6]: https://www.nuget.org/packages/TenorSharp/
[7]: https://img.shields.io/discord/569206809693257728
[8]: https://discord.gg/sWwzJeG

TenorSharp is a C# Library for the [TenorAPI](https://tenor.com/gifapi). TenorSharp was made with dotNET Core 2.1

TenorSharp can be used to retrieve Content from Tenor without directly interfacing with the Tenor REST API.

## Getting Started

For Development, fork and Clone this GitHub Repo.

For Usage, Refer to [Installing](#installing)

If you need more help, refer to the Documentation in IntelliSense, or [join the Discord Server](https://discord.gg/sWwzJeG).

### Dependencies

TenorSharp is dependent on RestSharp and Newtonsoft.Json


### Installing

Use NuGet to Install [this Package](https://www.nuget.org/packages/TenorSharp/1.0.0)

## Usage
Refer to the [Tenor API Attribution Guidelines](https://tenor.com/gifapi/documentation#attribution) to properly attribute Content retrieved from Tenor.

### Usage Example

A Basic Tenor Search

```csharp
public static void Main(string[] args)
		{
			var tenor = new TenorClient([API Key](https://tenor.com/gifapi));
			Console.WriteLine(tenor.Search("test").GifResults[0].Shares);
		}
```

## Built With

* [dotNET Core 3.1](https://dotnet.microsoft.com/download/dotnet-core/3.1) - The framework used
* [dotNET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0) - The framework used


## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on the code of conduct, and the process for submitting pull requests.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/OmegaRogue/TenorSharp/tags).

## Authors

* **OmegaRogue** - *Initial work* - [OmegaRogue](https://github.com/OmegaRogue)

~~See also the list of [contributors](https://github.com/OmegaRogue/TenorSharp/contributors) who participated in this project.~~

## License

This project is licensed under the GNU LGPL v3.0 License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* Hat tip to anyone whose code was used
* Powered by Tenor

![Tenor](TENOR.png)
