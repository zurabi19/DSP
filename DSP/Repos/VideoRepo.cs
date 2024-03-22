using AngleSharp.Dom;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace DSP.Repos
{
    public class VideoRepo:IVideoRepo
    {
        YoutubeClient _youtube;
        public VideoRepo()
        {
            _youtube = new YoutubeClient();
        }


        public async Task DowloandMp3(string videoId, string keyword )
        {
            string correctKeyWord = SanitizeKeyword( keyword );
            var videoUrl = $"https://www.youtube.com/watch?v={videoId}";
            var streamManifest = await _youtube.Videos.Streams.GetManifestAsync(videoUrl);
            var streamInfo = streamManifest.GetAudioOnlyStreams().TryGetWithHighestBitrate();
            await _youtube.Videos.Streams.DownloadAsync(streamInfo, $"C:\\Users\\zukit\\Music\\{correctKeyWord}.mp3");
        }

        private string SanitizeKeyword(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return string.Empty;
            }

            var invalidChars = Path.GetInvalidFileNameChars();

            string sanitizedKeyword = new string(keyword.Where(c => !invalidChars.Contains(c)).ToArray());

            return sanitizedKeyword;
        }

    }
}
