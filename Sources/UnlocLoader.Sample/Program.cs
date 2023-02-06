using System;
using System.Linq;
using UnlocLoader.Model;

namespace UnlocLoader.Sample
{
    internal class Program
    {
        private static void Main()
        {
            var loader = new UnlocLoader("https://service.unece.org/trade/locode/loc222csv.zip");

            loader.OnWarn += (sender, s) => Console.WriteLine($"Warn: {s}");
            loader.OnInfo += (sender, s) => Console.WriteLine($"Info: {s}");

            var result = loader.LoadFromUnece();
            
            var ports = result.Locations
                .Where(l => l.Functions.Contains(Function.Port))
                .ToList();

            foreach (var port in ports)
            {
                Console.WriteLine(port.ToString());
            }
        }
    }
}
