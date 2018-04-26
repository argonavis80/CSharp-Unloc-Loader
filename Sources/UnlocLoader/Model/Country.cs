namespace UnlocLoader.Model
{
    public class Country
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public virtual Position Position { get; set; }

        public override string ToString()
        {
            return $"{Id}: {Name}";
        }
    }
}