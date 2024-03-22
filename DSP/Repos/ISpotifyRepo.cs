namespace DSP.Repos
{
    public interface ISpotifyRepo
    {
        Task<List<string>> GetPlaylist(string playlist_id);
        string? ExtractPlaylistId(string url);
    }

}
