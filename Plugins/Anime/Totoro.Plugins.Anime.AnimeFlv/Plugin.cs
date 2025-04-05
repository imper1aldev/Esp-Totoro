using System.Reflection;
using Totoro.Plugins.Anime.Contracts;
using Totoro.Plugins.Contracts;

namespace Totoro.Plugins.Anime.AnimeFlv;

public class Plugin : Plugin<AnimeProvider, Config>
{
    public override AnimeProvider Create() => new()
    {
        Catalog = new Catalog(),
        StreamProvider = new StreamProvider(),
    };

    public override PluginInfo GetInfo() => new()
    {
        DisplayName = "AnimeFlv",
        Name = "animeflv",
        Version = Assembly.GetExecutingAssembly().GetName().Version!,
        Icon = typeof(Plugin).Assembly.GetManifestResourceStream("Totoro.Plugins.Anime.AnimeFlv.animeflv-icon.png"),
        Description = "Access media from your self hosted instance"
    };
}
