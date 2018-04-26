using System.Collections.Generic;

namespace UnlocLoader
{
    public class UnlocLoaderResult
    {
        internal UnlocLoaderResult(List<Location> locations, List<Country> countries)
        {
            Locations = locations;
            Countries = countries;
        }

        public List<Location> Locations { get; }

        public List<Country> Countries { get; }
    }
}