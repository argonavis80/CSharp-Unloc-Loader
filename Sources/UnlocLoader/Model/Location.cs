namespace UnlocLoader.Model
{
    /// <summary>
    /// A location
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Official UN/LOCODE of location.
        /// </summary>
        public string UNLOC { get; set; }

        /// <summary>
        /// Name of location.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Country of location.
        /// </summary>
        public virtual Country Country { get; set; }

        /// <summary>
        /// Position of location if available.
        /// </summary>
        public virtual Position Position { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name} [{UNLOC}]";
        }
    }
}