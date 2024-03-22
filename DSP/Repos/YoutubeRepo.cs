using DSP.Models;
using System.Text.Json;

namespace DSP.Repos
{
    public class YoutubeRepo :IYoutubeRepo
    {
        HttpClient _httpClient;
        string key;
        Uri Uri = new Uri("https://www.googleapis.com/youtube/v3/");

        public YoutubeRepo()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = Uri;
            key = "my youtube key";
        }

        public async Task<YoutubeSearchListResponse?> GetVideoId(string keyword)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"search?part=id&q={keyword}&key={key}&maxResults=20");
            string json = await response.Content.ReadAsStringAsync();
            
            YoutubeSearchListResponse? videoList = JsonSerializer.Deserialize<YoutubeSearchListResponse>(json);
            return videoList;
        }
    }
}
