using System;

namespace UnlocLoader
{
    public class LoaderBase : LogEmiter
    {
        protected internal double? ParseLatitude(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                    return null;

                var latString = token.Split(' ')[0];

                var latDeg = latString.Substring(0, 2);
                var latMin = latString.Substring(2, 2);
                var latHs = latString.Substring(4, 1);

                var deg = double.Parse(latDeg);
                var min = double.Parse(latMin);

                var result = deg + min / 60;

                result = latHs == "N" ? result : -result;

                return result;
            }
            catch (Exception ex)
            {
                EmitWarn($"Failed to parse latitude: {ex.Message}");
                return null;
            }
        }

        protected internal double? ParseLongitude(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                    return null;

                var lngString = token.Split(' ')[1];

                var lngDeg = lngString.Substring(0, 3);
                var lngMin = lngString.Substring(3, 2);
                var lngHs = lngString.Substring(5, 1);

                var deg = double.Parse(lngDeg);
                var min = double.Parse(lngMin);

                var result = deg + min / 60;

                result = lngHs == "E" ? result : -result;

                return result;
            }
            catch (Exception ex)
            {
                EmitWarn($"Failed to parse longitude: {ex.Message}");
                return null;
            }
        }
    }
}