using UnlocLoader.Model;

namespace UnlocLoader.Loader
{
    public interface ILocationParser
    {
        Location Parse(string source, out string message);
    }
}