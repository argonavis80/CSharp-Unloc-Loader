using System.Collections.Generic;
using UnlocLoader.Model;

namespace UnlocLoader
{
    /// <summary>
    /// Wrapper for parsed <see cref="Location"/> and <see cref="Country"/>.
    /// </summary>
    public class LoaderResult
    {
        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="locations">List of locations.</param>
        /// <param name="countries">List of countries.</param>
        internal LoaderResult(List<Location> locations, List<Country> countries)
        {
            Locations = locations;
            Countries = countries;
        }

        /// <summary>
        /// List of parsed <see cref="Location"/>
        /// </summary>
        public List<Location> Locations { get; }

        /// <summary>
        /// List of parse <see cref="Country"/>
        /// </summary>
        public List<Country> Countries { get; }
    }
}