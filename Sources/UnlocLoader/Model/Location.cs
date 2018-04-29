namespace UnlocLoader.Model
{
    /// <summary>
    /// A location
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Change reason of the entry.
        /// </summary>
        public ChangeReason ChangeReason { get; set; }

        /// <summary>
        /// Details of change.
        /// </summary>
        public ChangeDetails[] ChangeDetails { get; set; }

        /// <summary>
        /// Official UN/LOCODE of location (five-letter code).
        /// </summary>
        public string UNLOC { get; set; }

        /// <summary>
        /// Name of location.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Spelling name of location.
        /// </summary>
        public string SpellingName { get; set; }

        /// <summary>
        /// Function of location.
        /// </summary>
        public Function[] Functions { get; set; }

        /// <summary>
        /// Two-letter code of country.
        /// </summary>
        public string CountryId { get; set; }

        /// <summary>
        /// Country of location.
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        /// Position of location if available.
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// Remarks
        /// </summary>
        public string Remarks { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name} [{UNLOC}]";
        }
    }
}