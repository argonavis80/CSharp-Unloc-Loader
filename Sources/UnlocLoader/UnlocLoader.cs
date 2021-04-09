using System;
using System.IO;
using System.IO.Compression;
using UnlocLoader.Core;
using UnlocLoader.Loader;
using UnlocLoader.Model;

namespace UnlocLoader
{
    /// <summary>
    /// Fetches the UN/LOCODE files from UN servers and parses the content.
    /// </summary>
    public class UnlocLoader : LogEmiter
    {
        private const string UnlocFileUrl = "https://unece.org/sites/default/files/2020-12/loc202csv.zip";

        private readonly CountryLoader _countryLoader;
        private readonly LocationLoader _locationLoader;
        private readonly FileDownloader _fileDownloader;

        /// <summary>
        /// Create a new instance of <see cref="UnlocLoader"/>.
        /// </summary>
        public UnlocLoader()
        {
            var locationParser = new LocationParser();

            _countryLoader = new CountryLoader();
            _locationLoader = new LocationLoader(locationParser);
            _fileDownloader = new FileDownloader();

            _countryLoader.OnInfo += (sender, s) => OnInfo?.Invoke(sender, s);
            _countryLoader.OnWarn += (sender, s) => OnWarn?.Invoke(sender, s);
            _countryLoader.OnTrace += (sender, s) => OnTrace?.Invoke(sender, s);

            _locationLoader.OnInfo += (sender, s) => OnInfo?.Invoke(sender, s);
            _locationLoader.OnWarn += (sender, s) => OnWarn?.Invoke(sender, s);
            _locationLoader.OnTrace += (sender, s) => OnTrace?.Invoke(sender, s);
        }

        /// <summary>
        /// Fetches the UN/LOCODE files from UNECE server and parses the content.
        /// </summary>
        /// <returns>
        /// An object containing a list of <see cref="Location"/> and <see cref="Country"/>.
        /// </returns>
        public LoaderResult LoadFromUnece()
        {
            using (var workspace = new Workspace())
            {
                var folder = workspace.WorkingFolder;

                EmitInfo("Download UN/LOCODE and extract.");

                var file = _fileDownloader.DownloadTemp(UnlocFileUrl);

                Extract(file, folder);

                var countries = _countryLoader.Load(folder);
                var locations = _locationLoader.Load(folder, countries);

                return new LoaderResult(locations, countries);
            }
        }

        /// <summary>
        /// Parses the UN/LOCODE from a local file.
        /// </summary>
        /// <returns>
        /// An object containing a list of <see cref="Location"/> and <see cref="Country"/>.
        /// </returns>
        public LoaderResult LoadFromFile(string file)
        {
            using (var workspace = new Workspace())
            {
                var folder = workspace.WorkingFolder;

                Extract(file, folder);

                var countries = _countryLoader.Load(folder);
                var locations = _locationLoader.Load(folder, countries);

                return new LoaderResult(locations, countries);
            }
        }

        private static void Extract(string file, string folder)
        {
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