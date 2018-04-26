using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UnlocLoader
{
    public class CountryLoader : LoaderBase
    {
        public List<Country> Load(string folder)
        {
            EmitInfo("Load countries from source.");

            var directory = new DirectoryInfo(folder);

            var files = directory.EnumerateFiles("*UNLOCODE*.csv").ToList();

            if (!files.Any())
                throw new InvalidDataException("Download does not contain any UN/LOCODE files.");

            var countries = new List<Country>();

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

                            if (!string.IsNullOrWhiteSpace(tokens[2]) || !string.IsNullOrWhiteSpace(tokens[6]))
                                continue; // This is a location. Skip.

                            var country = ParseCountry(tokens);

                            if (country != null)
                            {
                                countries.Add(country);
                                EmitTrace($"{country.Id} : {country.Name}");
                            }
                        }
                        catch (Exception ex)
                        {
                            EmitWarn($"Failed to parse country: {ex.Message}");
                        }
                    }
                }
            }

            return countries;
        }

        private Country ParseCountry(IReadOnlyList<string> tokens)
        {
            var regex = new Regex(@"^\d{4}[NS] \d{5}[EW]$");

            var countryId = tokens[1];
            
            var posToken = tokens.FirstOrDefault(regex.IsMatch);

            var lat = ParseLatitude(posToken);
            var lng = ParseLongitude(posToken);

            if (countryId.Length != 2)
            {
                EmitWarn($"Country with invalid UN/LOCODE found: {countryId}");
            }

            var country = new Country
            {
                Id = countryId,
                Name = tokens[3].Trim('.')
            };

            if (lat != null && lng != null)
            {
                var position = new Position
                {
                    Lat = lat.Value,
                    Lng = lng.Value
                };

                country.Position = position;
            }

            return country;
        }
    }
}