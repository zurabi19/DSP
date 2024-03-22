namespace DSP.Models
{
    public class YoutubeSearchListResponse
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public string nextPageToken { get; set; }
        public string regionCode { get; set; }
        public PageInfo pageInfo { get; set; }
        public List<SearchResult> items { get; set; }
    }
}
