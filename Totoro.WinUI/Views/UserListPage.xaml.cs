﻿using CommunityToolkit.Labs.WinUI;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using ReactiveMarbles.ObservableEvents;
using Totoro.Core.ViewModels;
using Totoro.WinUI.Helpers;

namespace Totoro.WinUI.Views;

public class UserListPageBase : ReactivePage<UserListViewModel> { }
public sealed partial class UserListPage : UserListPageBase
{
    public const string DataGridSettingsKey = "UserListDataGridSettings";

    public List<AnimeStatus> Statuses { get; set; } = new List<AnimeStatus>
    {
        AnimeStatus.Watching,
        AnimeStatus.PlanToWatch,
        AnimeStatus.OnHold,
        AnimeStatus.Completed,
        AnimeStatus.Dropped
    };

    public static List<int?> Scores { get; } = new() { null, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    public static string ToStatusString(AnimeStatus status)
    {
        return status switch
        {
            AnimeStatus.Watching => "Watching",
            AnimeStatus.PlanToWatch => "Plan to watch",
            AnimeStatus.OnHold => "On Hold",
            AnimeStatus.Completed => "Completed",
            AnimeStatus.Dropped => "Dropped",
            _ => throw new ArgumentException(null, nameof(status))
        };
    }

    public UserListPage()
    {
        InitializeComponent();

        this.WhenActivated(d =>
        {
            this.WhenAnyValue(x => x.ViewModel.Filter.ListStatus)
                .Subscribe(x =>
                {
                    switch (x)
                    {
                        case AnimeStatus.Watching:
                            WatchingFlyoutToggle.IsChecked = true;
                            break;
                        case AnimeStatus.PlanToWatch:
                            PtwFlyoutToggle.IsChecked = true;
                            break;
                        case AnimeStatus.Completed:
                            CompletedFlyoutToggle.IsChecked = true;
                            break;
                        case AnimeStatus.OnHold:
                            OnHoldFlyoutToggle.IsChecked = true;
                            break;
                        case AnimeStatus.Dropped:
                            DroppedFlyoutToggle.IsChecked = true;
                            break;
                    }
                })
                .DisposeWith(d);

            QuickAdd
            .Events()
            .Click
            .Do(_ => ViewModel.ClearSearch())
            .Subscribe(_ => QuickAddPopup.IsOpen ^= true)
            .DisposeWith(d);

            GenresButton
            .Events()
            .Click
            .Subscribe(_ => GenresTeachingTip.IsOpen ^= true);

            QuickAddResult
            .Events()
            .ItemClick
            .Select(args => args.ClickedItem as IAnimeModel)
            .Do(_ => QuickAddPopup.IsOpen = false)
            .SelectMany(model => ViewModel.UpdateAnime(model))
            .Subscribe()
            .DisposeWith(d);
        });
    }

    private void GenresButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        GenresTeachingTip.IsOpen = true;
    }

    private void TokenView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ViewModel.Filter.Genres = new ObservableCollection<string>(((TokenView)sender).SelectedItems.Cast<string>());
    }

    private void AiringStatusClicked(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        var btn = (RadioMenuFlyoutItem)sender;
        ViewModel.Filter.AiringStatus = btn.Tag is AiringStatus status ? status : (AiringStatus?)null;
    }

    private void OnSaveColumnWidths(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        foreach (var column in ListDataGrid.Columns)
        {
            if (ViewModel.DataGridSettings.Columns.FirstOrDefault(x => x.Name == (string)column.Tag) is not { } model)
            {
                continue;
            }

            model.Width = column.ActualWidth;
        }
    }

    private void OnLoadingRow(object sender, CommunityToolkit.WinUI.UI.Controls.DataGridRowEventArgs e)
    {
        e.Row.ContextFlyout = Converters.AnimeToFlyout(e.Row.DataContext as AnimeModel);
    }
}

public class ScoreConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is null)
        {
            return "(0) No Score";
        }

        if(value is not int score)
        {
            return DependencyProperty.UnsetValue;
        }

        return score switch
        {
            1 => "(1) Appalling",
            2 => "(2) Horrible",
            3 => "(3) Very Bad",
            4 => "(4) Bad",
            5 => "(5) Average",
            6 => "(6) Fine",
            7 => "(7) Good",
            8 => "(8) Very Good",
            9 => "(9) Great",
            10 => "(10) Masterpiece",
            _ => "(0) No Score"
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}

public class AiringStatusToBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if(value is not AiringStatus status)
        {
            return DependencyProperty.UnsetValue;
        }

        return status switch
        {
            AiringStatus.CurrentlyAiring => new SolidColorBrush(Colors.LimeGreen),
            AiringStatus.FinishedAiring => new SolidColorBrush(Colors.MediumSlateBlue),
            AiringStatus.NotYetAired => new SolidColorBrush(Colors.LightSlateGray),
            _ => new SolidColorBrush(Colors.Navy),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}



