namespace UnlocLoader.Model
{
    public class Location
    {
        public string UNLOC { get; set; }

        public string Name { get; set; }

        public virtual Country Country { get; set; }

        public virtual Position Position { get; set; }

        public override string ToString()
        {
            return $"{Name} [{UNLOC}]";
        }
    }
}