using System.Collections.Generic;
using UnlocLoader.Model;

namespace UnlocLoader
{
    public class LoaderResult
    {
        internal LoaderResult(List<Location> locations, List<Country> countries)
        {
            Locations = locations;
            Countries = countries;
        }

        public List<Location> Locations { get; }

        public List<Country> Countries { get; }
    }
}