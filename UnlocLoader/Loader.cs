using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace UnlocLoader
{
    public class Loader : LogEmiter
    {
        private const string UnlocFileUrl = "http://www.unece.org/fileadmin/DAM/cefact/locode/loc172csv.zip";

        private readonly CountryLoader _countryLoader;
        private readonly LocationLoader _locationLoader;
        private readonly FileDownloader _fileDownloader;

        public Loader()
        {
            _countryLoader = new CountryLoader();
            _locationLoader = new LocationLoader();
            _fileDownloader = new FileDownloader();

            _countryLoader.OnInfo += (sender, s) => OnInfo?.Invoke(sender, s);
            _countryLoader.OnWarn += (sender, s) => OnWarn?.Invoke(sender, s);
            _countryLoader.OnTrace += (sender, s) => OnTrace?.Invoke(sender, s);

            _locationLoader.OnInfo += (sender, s) => OnInfo?.Invoke(sender, s);
            _locationLoader.OnWarn += (sender, s) => OnWarn?.Invoke(sender, s);
            _locationLoader.OnTrace += (sender, s) => OnTrace?.Invoke(sender, s);
        }

        public UnlocLoaderResult Load()
        {
            using (var workspace = new Workspace())
            {
                var folder = workspace.WorkingFolder;

                DownloadAndExtract(UnlocFileUrl, folder);

                var countries = _countryLoader.Load(folder);
                var locations = _locationLoader.Load(folder, countries);

                return new UnlocLoaderResult(locations, countries.Select(kvp => kvp.Value).ToList());
            }
        }

        private void DownloadAndExtract(string url, string folder)
        {
            EmitInfo("Download UN/LOCODE and extract.");

            var file = _fileDownloader.DownloadTemp(url);

            try
            {
                ZipFile.ExtractToDirectory(file, folder);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("Failed to extract UN/LOCODE file.", ex);
            }
        }
    }
}