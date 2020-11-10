using System;
namespace wikiart
{
    public class WikiArtSession
    {
        public string SessionKey { get; set; }
        public int MaxRequestsPerHour { get; set; }
        public int MaxSessionsPerHour { get; set; }
        public string SessionKeyQueryString
        {
            get
            {
                return string.Format("&authSessionKey = {0}", SessionKey);
            }
        }
    }
}
