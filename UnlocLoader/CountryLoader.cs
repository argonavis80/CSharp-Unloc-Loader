using System.Collections.Generic;

namespace UnlocLoader
{
    public class CountryLoader : LoaderBase
    {
        public  Dictionary<string, Country> Load(string folder)
        {
            EmitInfo("Load countries from source.");

            var countries = new Dictionary<string, Country>();
            
            return countries;
        }
    }
}