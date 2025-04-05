using Totoro.Plugins.Anime.Contracts;
using Totoro.Plugins.Anime.Models;

namespace Totoro.Plugins.Anime.AnimeFlv;

public class StreamProvider : IAnimeStreamProvider
{
    public Task<int> GetNumberOfStreams(string url)
    {
        return Task.FromResult(0);
    }

    public IAsyncEnumerable<VideoStreamsForEpisode> GetStreams(string url, Range episodeRange)
    {
        throw new NotImplementedException();
    }
}
