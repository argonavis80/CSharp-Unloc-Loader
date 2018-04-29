[![Build status](https://ci.appveyor.com/api/projects/status/jaswv2uct51xt70v?svg=true)](https://ci.appveyor.com/project/argonavis80/csharp-unloc-loader)

# CSharp-Unloc-Loader
This C# library performs these steps:
1. Download latest CSV files from [UNECE servers](http://www.unece.org/cefact/codesfortrade/codes_index.html)  with official UN location codes.
1. Extract the files into a temporary directory.
1. Parses and returns the locations and countries.
1. Cleans up the working directory.

## Example usage
The below code snippet downloads and parses the latest UN/LOCODE files from UNECE servers and writes all ports on the console.

```csharp
var loader = new UnlocLoader();

loader.OnWarn += (sender, s) => Console.WriteLine($"Warn: {s}");
loader.OnInfo += (sender, s) => Console.WriteLine($"Info: {s}");

// Download and parse latest UN/LOCODE files from UNECE server.
var result = loader.LoadFromUnece(); 

var ports = result.Locations
    .Where(l => l.Functions.Contains(Function.Port))
    .ToList();

foreach (var port in ports)
{
    Console.WriteLine(port.ToString());
}
```

Instead of loading the location files from the UNECE servers, an existing (zipped) location file can be parsed locally like this:
```csharp
var result = loader.LoadFromFile(@"C:\UnlocFile.zip");
```

## Parsed data
The following items are parsed for each location:

| Property name   | Description |
|-----------------|-------------|
| `UNLOC`         | Five-letter UN location code |
| `CountryId`     | Two-letter UN country code |
| `Name`          | Accepted name of location  |  
| `SpellingName`  | Name of location without diacritic signs |
| `Function`      | List of functions of the location (e.g. port) |
| `ChangeReason`  | Reason of change in location details (if any) |
| `ChangeDetails` | List of details what has changed (if any) |
| `Position`      | Position of location (if available) |
| `Remarks`       | Remarks for location (if available) |

## Remarks
1. Multiple entries with the same UN/LOCODE exists for different names of the same location.