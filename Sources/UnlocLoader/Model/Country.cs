namespace UnlocLoader.Model
{
    /// <summary>
    /// A country
    /// </summary>
    public class Country
    {
        /// <summary>
        /// Two letter code of country.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Name of country.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Position of country if available.
        /// </summary>
        public virtual Position Position { get; set; }
        
        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Id}: {Name}";
        }
    }
}