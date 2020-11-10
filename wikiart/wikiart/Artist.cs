namespace wikiart
{
    public class Artist
    {
        public long contentId { get; set; }
        public string artistName { get; set; }
        public string url { get; set; }
        public string lastNameFirst { get; set; }
        public string birthDay { get; set; }
        public string deathDay { get; set; }
        public string birthDayAsString { get; set; }
        public string deathDayAsString { get; set; }
        public string image { get; set; }
        public string wikipediaUrl { get; set; }
        public long[] dictonaries { get; set; }
    }
}
