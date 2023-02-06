using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnlocLoader.Core;
using UnlocLoader.Model;

namespace UnlocLoader.Loader
{
    /// <summary>
    /// Location Parser
    /// </summary>
    public class LocationParser : ILocationParser
    {
        /// <summary>
        /// Parse string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Location Parse(string source, out string message)
        {
            message = null;

            var tokens = ExtractTokens(source);

            if (tokens.Length <= 1 || string.IsNullOrWhiteSpace(tokens[2]) && string.IsNullOrWhiteSpace(tokens[6]))
                return null; // This is a country.

            var regex = new Regex(@"^\d{4}[NS] \d{5}[EW]$");

            var countryId = tokens[1];
            var locationId = countryId + tokens[2];
            var functions = ParseFunction(tokens[6]);
            var changeReason = ParseChangeReason(tokens[0], out message);
            var changeDetails = ParseChangeDetails(tokens[11], out var remarks);

            var posToken = tokens.FirstOrDefault(regex.IsMatch);

            var lat = PositionParser.ParseLatitude(posToken);
            var lng = PositionParser.ParseLongitude(posToken);

            if (locationId.Length != 5)
                message = $"Location with invalid UN/LOCODE found: {locationId}";

            var location = new Location
            {
                UNLOC = locationId,
                CountryId = countryId,
                Name = tokens[3],
                SpellingName = tokens[4],
                Functions = functions,
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

        private static string[] ExtractTokens(string source)
        {
            var count = 0;
            var matches = new List<Match>();
            var regex = new Regex("\"[^\"]*\"");

            var tokenizedSource = regex.Replace(source, match =>
            {
                matches.Add(match);
                return $"{count++}";
            });

            var tokens = tokenizedSource.Split(',');
            char[] charsToTrim = { '\'', '"', ' ' };

            for (var i = 0; i < tokens.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(tokens[i].Trim(charsToTrim)))
                    continue;

                var tokenIndex = int.Parse(tokens[i].Trim(charsToTrim));

                tokens[i] = matches[tokenIndex].Value.Trim(charsToTrim);
            }

            return tokens;
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

        private static Function[] ParseFunction(string token)
        {
            token = token.Trim().Trim('"');

            var reasons = new List<Function>();

            if (token.Contains("0"))
                reasons.Add(Function.Unknown);

            if (token.Contains("1"))
                reasons.Add(Function.Port);

            if (token.Contains("2"))
                reasons.Add(Function.RailTerminal);

            if (token.Contains("3"))
                reasons.Add(Function.RoadTerminal);

            if (token.Contains("4"))
                reasons.Add(Function.Airport);

            if (token.Contains("5"))
                reasons.Add(Function.PostalExchangeOffice);

            if (token.Contains("6"))
                reasons.Add(Function.MultimodalFunction);

            if (token.Contains("7"))
                reasons.Add(Function.FixedTransportFunction);

            if (token.Contains("B"))
                reasons.Add(Function.BorderCrossing);

            return reasons.ToArray();
        }
    }
}
