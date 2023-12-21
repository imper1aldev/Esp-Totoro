using FlyleafLib;
using FlyleafLib.MediaPlayer;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;


namespace Totoro.WinUI.Media.Flyleaf;

public sealed partial class FlyleafMediaPlayerElement : UserControl
{
    public Player Player
    {
        get { return (Player)GetValue(PlayerProperty); }
        set { SetValue(PlayerProperty, value); }
    }

    public static readonly DependencyProperty PlayerProperty =
        DependencyProperty.Register("Player", typeof(Player), typeof(FlyleafMediaPlayerElement), new PropertyMetadata(null));

    public FlyleafMediaPlayerElement()
    {
        this.InitializeComponent();

        Engine.Start(new EngineConfig()
        {
            FFmpegPath = @"E:\FFmpeg",
            FFmpegDevices = false,    // Prevents loading avdevice/avfilter dll files. Enable it only if you plan to use dshow/gdigrab etc.

#if RELEASE
            FFmpegPath = @"FFmpeg",
            FFmpegLogLevel      = FFmpegLogLevel.Quiet,
            LogLevel            = LogLevel.Quiet,

#else
            FFmpegLogLevel = FFmpegLogLevel.Warning,
            LogLevel = LogLevel.Debug,
            LogOutput = ":debug",
            //LogOutput         = ":console",
            //LogOutput         = @"C:\Flyleaf\Logs\flyleaf.log",                
#endif

            //PluginsPath       = @"C:\Flyleaf\Plugins",

            UIRefresh = false,    // Required for Activity, BufferedDuration, Stats in combination with Config.Player.Stats = true
            UIRefreshInterval = 250,      // How often (in ms) to notify the UI
            UICurTimePerSecond = true,     // Whether to notify UI for CurTime only when it's second changed or by UIRefreshInterval
        });
    }

    private void FSC_PointerMoved(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
    {
        TransportControls.Show();
    }
}
