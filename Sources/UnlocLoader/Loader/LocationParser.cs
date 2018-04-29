using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnlocLoader.Core;
using UnlocLoader.Model;

namespace UnlocLoader.Loader
{
    public class LocationParser : ILocationParser
    {
        public Location Parse(string source, out string message)
        {
            message = null;
            var tokens = source.Split(',').Select(t => t.Trim('"')).ToArray();

            if (string.IsNullOrWhiteSpace(tokens[2]) && string.IsNullOrWhiteSpace(tokens[6]))
                return null; // This is a country.

            var regex = new Regex(@"^\d{4}[NS] \d{5}[EW]$");

            var countryId = tokens[1];
            var locationId = countryId + tokens[2];
            var changeReason = ParseChangeReason(tokens[0], out message);
            var changeDetails = ParseChangeDetails(tokens[11], out var remarks);

            var posToken = tokens.FirstOrDefault(regex.IsMatch);

            var lat = PositionParser.ParseLatitude(posToken);
            var lng = PositionParser.ParseLongitude(posToken);

            if (locationId.Length != 5)
            {
                message = $"Location with invalid UN/LOCODE found: {locationId}";
            }

            var location = new Location
            {
                UNLOC = locationId,
                CountryId = countryId,
                Name = tokens[3],
                SpellingName = tokens[4],
                ChangeReason = changeReason,
                ChangeDetails = changeDetails,
                Remarks = remarks
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

        private static ChangeDetails[] ParseChangeDetails(string token, out string remarks)
        {
            token = token.Trim().Trim('"');
            remarks = token;

            var reasons = new List<ChangeDetails>();

            if (token.Contains("@Coo"))
                reasons.Add(ChangeDetails.Coordinates);

            if (token.Contains("@Fun"))
                reasons.Add(ChangeDetails.Function);

            if (token.Contains("@Nam"))
                reasons.Add(ChangeDetails.LocationName);

            if (token.Contains("@Spe"))
                reasons.Add(ChangeDetails.SpellingName);

            if (token.Contains("@Sta"))
                reasons.Add(ChangeDetails.Status);

            if (token.Contains("@Sub"))
                reasons.Add(ChangeDetails.SubdivisionCode);

            return reasons.ToArray();
        }

        private static ChangeReason ParseChangeReason(string token, out string message)
        {
            message = null;
            token = token.Trim().Trim('"');

            if (string.IsNullOrWhiteSpace(token))
                return ChangeReason.None;

            switch (token)
            {
                case "X":
                    return ChangeReason.MarkedForDeletion;
                case "#":
                    return ChangeReason.ChangeLocationName;
                case "|":
                    return ChangeReason.ChangeOther;
                case "+":
                    return ChangeReason.Added;
                case "=":
                    return ChangeReason.Reference;
                case "!":
                    return ChangeReason.Retained;
                default:
                    message = $"Invalid change reason: {token}";
                    return ChangeReason.None;
            }
        }
    }
}
