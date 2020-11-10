using System;
namespace wikiart
{
    public class Artwork
    {
        public string image { get; set; }
        public string rawImage
        {
            get
            {
                int resolutionLocation = image.LastIndexOf('!');
                if (resolutionLocation == -1)
                    return image;
                else
                    return image.Substring(0, resolutionLocation);
            }
        }
    }
}
