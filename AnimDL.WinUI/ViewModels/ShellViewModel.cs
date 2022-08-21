﻿using AnimDL.UI.Core.ViewModels;
using AnimDL.WinUI.Contracts;
using Microsoft.UI.Xaml.Navigation;

namespace AnimDL.WinUI.ViewModels;

public partial class ShellViewModel : ReactiveObject
{
    [Reactive] public object Selected { get; set; }
    [Reactive] public bool IsBackEnabled { get; set; }
    [Reactive] public bool IsAuthenticated { get; set; }

    public IWinUINavigationService NavigationService { get; }
    public INavigationViewService NavigationViewService { get; set; }

    public ShellViewModel(IWinUINavigationService navigationService,
                          INavigationViewService navigationViewService,
                          IMessageBus messageBus)
    {
        NavigationService = navigationService;
        NavigationService.Navigated.Subscribe(OnNavigated);
        NavigationViewService = navigationViewService;

        messageBus.Listen<MalAuthenticatedMessage>()
                  .ObserveOn(RxApp.MainThreadScheduler)
                  .Subscribe(_ => IsAuthenticated = true);
    }

    private void OnNavigated(NavigationEventArgs e)
    {
        IsBackEnabled = NavigationService.CanGoBack;
        var vmType = NavigationService.Frame.GetPageViewModel().GetType();

        if (vmType == typeof(SettingsViewModel))
        {
            Selected = NavigationViewService.SettingsItem;
            return;
        }

        var selectedItem = NavigationViewService.GetSelectedItem(vmType);
        if (selectedItem != null)
        {
            Selected = selectedItem;
        }
    }
}
