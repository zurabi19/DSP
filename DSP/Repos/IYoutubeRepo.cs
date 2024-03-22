using DSP.Models;

namespace DSP.Repos
{
    public interface IYoutubeRepo
    {
        public Task<YoutubeSearchListResponse> GetVideoId(string keyword);
    }
}
