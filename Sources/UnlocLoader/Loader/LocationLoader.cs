using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MoreLinq;
using UnlocLoader.Core;
using UnlocLoader.Model;

namespace UnlocLoader.Loader
{
    public class LocationLoader : LogEmiter
    {
        private readonly LocationParser _locationParser;

        public LocationLoader(LocationParser locationParser)
        {
            _locationParser = locationParser;
        }

        public List<Location> Load(string folder, List<Country> countries)
        {
            var directory = new DirectoryInfo(folder);

            var countryDict = countries
                .DistinctBy(c => c.Id)
                .ToDictionary(c => c.Id, c => c);

            EmitInfo("Parse locations.");

            var files = directory.EnumerateFiles("*UNLOCODE*.csv").ToList();

            if (!files.Any())
                throw new InvalidDataException("Download does not contain any UN/LOCODE files.");

            var locations = new List<Location>();

            foreach (var fileInfo in files)
            {
                using (var reader = new StreamReader(fileInfo.FullName, Encoding.Default))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        try
                        {
                            var location = _locationParser.Parse(line, out var message);

                            if (location == null)
                                continue;

                            if (countryDict.ContainsKey(location.CountryId))
                            {
                                location.Country = countryDict[location.CountryId];
                            }
                            else
                            {
                                EmitWarn($"Country with ID {location.CountryId} does not exists.");
                            }

                            locations.Add(location);
                            EmitTrace($"{location.UNLOC} : {location.Name} -> {message}");
                        }
                        catch (Exception ex)
                        {
                            EmitWarn($"Failed to parse location: {ex.Message}");
                        }
                    }
                }
            }

            return locations;
        }
    }
}
