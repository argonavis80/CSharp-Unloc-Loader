[![Build status](https://ci.appveyor.com/api/projects/status/jaswv2uct51xt70v?svg=true)](https://ci.appveyor.com/project/argonavis80/csharp-unloc-loader)

# CSharp-Unloc-Loader
This C# library performs these steps:
1. Download latest CSV files from [UNECE servers](http://www.unece.org/cefact/codesfortrade/codes_index.html)  with official UN location codes.
1. Extract the files into a temporary directory.
1. Parses and returns the locations and countries.
1. Cleans up the working directory.

## Example usage
```csharp
var loader = new UnlocLoader();

loader.OnWarn += (sender, s) => Console.WriteLine($"Warn: {s}");
loader.OnInfo += (sender, s) => Console.WriteLine($"Info: {s}");
loader.OnTrace += (sender, s) => Console.WriteLine($"Trace: {s}");

var result = loader.Load();

foreach (var location in result.Locations)
{
    Console.WriteLine(location.ToString());
}

foreach (var country in result.Countries)
{
    Console.WriteLine(country.ToString());
}
```