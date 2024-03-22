using DSP.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;


//https://localhost:7299/main/download?url=https%3A%2F%2Fopen.spotify.com%2Fplaylist%2F5tndK3VcqKJoIC8V9zvwQG%3Fsi%3D46aea98994af486e

namespace DSP.Repos
{
    public class SpotifyRepo : ISpotifyRepo
    {
        private HttpClient _spotify = new HttpClient();
        string? token = null;
        string clientId = "my client Id";
        string clientSecret = "my client secret";


        private async Task GetToken()
{
    if (token is null)
    {
        string authorization = $"{clientId}:{clientSecret}";
        var base64EncodedAuthorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(authorization));

        var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

        requestMessage.Headers.Add("Authorization", $"Basic {base64EncodedAuthorization}");

        var requestContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });
        requestMessage.Content = requestContent;
        
        HttpResponseMessage message = await _spotify.SendAsync(requestMessage);

        string json = await message.Content.ReadAsStringAsync();

        Token? deserializedToken = JsonSerializer.Deserialize<Token>(json);

        this.token = deserializedToken?.access_token ?? token;
    }
    return;
}



        public async Task<List<string>> GetPlaylist(string playlist_id)
        {
            await GetToken();
            List<string> playlistNames = new List<string>();
            _spotify.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.token}");

            HttpResponseMessage message = await _spotify.GetAsync($"https://api.spotify.com/v1/playlists/{playlist_id}");

            string json = await message.Content.ReadAsStringAsync();
            SpotifyPlaylistResponse? response = JsonSerializer.Deserialize<SpotifyPlaylistResponse>(json);

            if (response != null && response.tracks != null && response.tracks.items != null)
            {
                foreach (var n in response.tracks.items)
                {
                    if (n != null && n.track != null && n.track.name != null)
                    {
                        playlistNames.Add(n.track.name);
                    }
                }
            }

            return playlistNames;
        }

        public string? ExtractPlaylistId(string url)
        {
            const string playlistPrefix = "https://open.spotify.com/playlist/";
            int prefixIndex = url.IndexOf(playlistPrefix);
            if (prefixIndex != -1)
            {
                int startIndex = prefixIndex + playlistPrefix.Length;
                int endIndex = url.IndexOf('?', startIndex);
                if (endIndex == -1)
                {
                    endIndex = url.Length;
                }
                return url.Substring(startIndex, endIndex - startIndex);
            }
            return null;
        }

    }
}
