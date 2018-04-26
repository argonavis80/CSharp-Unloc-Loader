namespace UnlocLoader.Model
{
    public class Position
    {
        public double Lat { get; set; }

        public double Lng { get; set; }

        public override string ToString()
        {
            return $"{Lat:F4}; {Lng:F4}";
        }
    }
}