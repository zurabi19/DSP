using DSP.Models;
using DSP.Repos;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace DSP.Controllers
{
    [Route("[controller]/[action]")]
    public class MainController : Controller
    {
        private readonly IYoutubeRepo youtubeRepo;
        private readonly IVideoRepo videoRepo;
        private readonly ISpotifyRepo spotifyRepo;

        public MainController(IYoutubeRepo youtubeRepo, IVideoRepo videoRepo, ISpotifyRepo spotifyRepo)
        {
            this.youtubeRepo = youtubeRepo;
            this.videoRepo = videoRepo;
            this.spotifyRepo = spotifyRepo;
        }

        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task Download([FromQuery]string url)
        {

            string? playlistId = spotifyRepo.ExtractPlaylistId(url);
            List<string> names = await spotifyRepo.GetPlaylist(playlistId);

            foreach (string name in names)
            {
                YoutubeSearchListResponse respone = await youtubeRepo.GetVideoId(name);
                if (respone is null)
                {
                    continue;
                }
                for(int i = 0; i <respone.items.ToArray().Length;i++)
                {
                    if (respone.items[i].id.kind == "youtube#video")
                    {
                        await videoRepo.DowloandMp3(respone.items[i].id.videoId, name);
                        break;
                    }
                }
                
            }
        }
    }
}
