using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UnlocLoader
{
    public class LocationLoader : LoaderBase
    {
        public List<Location> Load(string folder, Dictionary<string, Country> countries)
        {
            var directory = new DirectoryInfo(folder);

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
                            var tokens = line.Split(',').Select(t => t.Trim('"')).ToArray();

                            if (string.IsNullOrWhiteSpace(tokens[2]) && string.IsNullOrWhiteSpace(tokens[6]))
                                continue; // This line is a country. Skip.

                            //? This controls if only locations marked as port should be considered.
                            //x if (tokens[6].Substring(0, 1) == "-")
                            //x    continue;

                            var location = ParsePort(tokens, countries);

                            if (location != null)
                            {
                                locations.Add(location);
                                EmitTrace($"{location.UNLOC} : {location.Name}");
                            }
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

        private Location ParsePort(IReadOnlyList<string> tokens, IReadOnlyDictionary<string, Country> countries)
        {
            var regex = new Regex(@"^\d{4}[NS] \d{5}[EW]$");

            var countryId = tokens[1];
            var locationId = countryId + tokens[2];

            var posToken = tokens.FirstOrDefault(regex.IsMatch);

            var lat = ParseLatitude(posToken);
            var lng = ParseLongitude(posToken);

            Country country = null;

            if (countries.ContainsKey(countryId))
            {
                country = countries[countryId];
            }
            else
            {
                EmitWarn($"Country with ID {countryId} does not exists.");
            }

            if (locationId.Length != 5)
            {
                EmitWarn($"Location with invalid UN/LOCODE found: {locationId}");
            }

            var location = new Location
            {
                Country = country,
                UNLOC = locationId,
                Name = tokens[3]
            };

            if (lat != null && lng != null)
            {
                var position = new Position
                {
                    Lat = lat.Value,
                    Lng = lng.Value
                };

                location.Position = position;
            }

            return location;
        }
    }
}
