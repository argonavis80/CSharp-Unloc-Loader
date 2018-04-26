using System;

namespace UnlocLoader.App
{
    internal class Program
    {
        private static void Main()
        {
            var loader = new Loader();

            loader.OnWarn += (sender, s) => Console.WriteLine($"Warn: {s}");
            loader.OnInfo += (sender, s) => Console.WriteLine($"Info: {s}");
            loader.OnTrace += (sender, s) => Console.WriteLine($"Trace: {s}");

            var result = loader.Load();

            foreach (var location in result.Locations)
            {
                Console.WriteLine(location.ToString());
            }

            foreach (var country in result.Countries)
            {
                Console.WriteLine(country.ToString());
            }
        }
    }
}
