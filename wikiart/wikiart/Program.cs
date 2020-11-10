using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Diagnostics;

namespace wikiart
{
    class Program
    {
        //Get access and secret code from https://www.wikiart.org/en/App/GetApi/GetKeys
        static private string accessCode = "";
        static private string secretCode = "";
        static private WikiArtSession wikiArtSession;
        static private IEnumerable<Artist> artists;

        static void Main(string[] args)
        {
            Console.WriteLine("Start wikiart data collection");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            GetWikiArtData();
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }

        static void GetWikiArtData()
        {
            var wikiArtUrl = string.Format("https://www.wikiart.org/en/Api/2/login?accessCode={0}&secretCode={1}", accessCode, secretCode);
            var wikiArtUri = new Uri(wikiArtUrl);
            GetWikiArtSession(wikiArtUri).Wait();
            Console.WriteLine("retrieved wikiart session");
            wikiArtUrl = string.Format("http://www.wikiart.org/en/App/Artist/AlphabetJson?v=new&inPublicDomain=true{0}", wikiArtSession.SessionKeyQueryString);
            wikiArtUri = new Uri(wikiArtUrl);
            GetArtists(wikiArtUri).Wait();
            Console.WriteLine("retrieved wikiart artists");
            foreach (var artist in artists)
            {
                wikiArtUrl = string.Format("http://www.wikiart.org/en/App/Painting/PaintingsByArtist?artistUrl={0}&json=2", artist.url);
                wikiArtUri = new Uri(wikiArtUrl);
                GetArt(wikiArtUri, artist.url).Wait();
            }
        }

        static async Task GetWikiArtSession(Uri uri)
        {
            string responseContent = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(uri).ConfigureAwait(false);
                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    responseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            wikiArtSession = JsonSerializer.Deserialize<WikiArtSession>(responseContent);
        }

        static async Task GetArtists(Uri uri)
        {
            string responseContent = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(uri).ConfigureAwait(false);
                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    responseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            artists = JsonSerializer.Deserialize<IEnumerable<Artist>>(responseContent);
        }

        static async Task GetArt(Uri uri, string folderName)
        {
            string responseContent = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                var httpResponseMessage = await httpClient.GetAsync(uri).ConfigureAwait(false);
                if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                    responseContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            var artworks = JsonSerializer.Deserialize<IList<Artwork>>(responseContent);
            if (artworks.Count < 250) return;
            var path = string.Format("/Volumes/ART_DATA/images/{0}", folderName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            foreach (var artwork in artworks)
            {
                var imageUri = new Uri(artwork.rawImage);
                var localFile = string.Format("{0}/{1}", path, Path.GetFileName(imageUri.LocalPath));
                if (!File.Exists(localFile))
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        var httpResponseMessage = await httpClient.GetAsync(imageUri).ConfigureAwait(false);
                        if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var imageContent = await httpResponseMessage.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
                            using (var memoryStream = new MemoryStream(imageContent))
                            {
                                var bitmap = new Bitmap(memoryStream);
                                if (bitmap.PixelFormat.ToString().ToLower().Contains("rgb"))
                                {
                                    Console.WriteLine("downloading {0}", artwork.rawImage);
                                    bitmap.Save(localFile);
                                }
                                else
                                    Console.WriteLine("skipping {0} due to pixel format of {1}", artwork.rawImage, bitmap.PixelFormat.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine("{0} return code for {1}", httpResponseMessage.StatusCode, artwork.rawImage);
                        }
                    }
                }
            }
        }
    }
}
