using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ProductClientHub.App.Services;

public interface IErrorNotifier
{
    Task ShowError(string message);
    Task ShowErrors(IEnumerable<string> messages);
    Task ShowSuccess(string message);
    Task ShowInfo(string message);
}

public class ErrorNotifier : IErrorNotifier
{
    public async Task ShowError(string message)
    {
        await ShowToast(message, ToastDuration.Short);
    }

    public async Task ShowErrors(IEnumerable<string> messages)
    {
        var list = messages?.Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
        if (list is null || list.Count == 0) return;

        // Show all errors concatenated
        var combinedMessage = string.Join("\n", list);
        await ShowToast(combinedMessage, ToastDuration.Long);
    }

    public async Task ShowSuccess(string message)
    {
        await ShowToast(message, ToastDuration.Short);
    }

    public async Task ShowInfo(string message)
    {
        await ShowToast(message, ToastDuration.Short);
    }

    private static async Task ShowToast(string message, ToastDuration duration)
    {
        if (string.IsNullOrWhiteSpace(message)) return;

        try
        {
            var toast = Toast.Make(message, duration, 14);
            await toast.Show(CancellationToken.None);
        }
        catch
        {
            // Fallback to dialog if toast fails
            await ShowAlertFallback(message);
        }
    }

    private static async Task ShowAlertFallback(string message)
    {
        var windows = Application.Current?.Windows;
        var page = windows is { Count: > 0 } ? windows[0]?.Page : null;
        if (page != null)
        {
            await page.DisplayAlertAsync("Notification", message, "OK");
        }
    }
}
