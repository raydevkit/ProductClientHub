using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ProductClientHub.App.Services;

public interface IErrorNotifier
{
    Task ShowError(string message);
    Task ShowErrors(IEnumerable<string> messages);
}

public class ErrorNotifier : IErrorNotifier
{
    public async Task ShowError(string message)
    {
        if (string.IsNullOrWhiteSpace(message)) return;
        try
        {
            var toast = Toast.Make(message, ToastDuration.Short);
            await toast.Show(CancellationToken.None);
        }
        catch
        {
            // Fallback to dialog
            var page = Application.Current?.Windows.FirstOrDefault()?.Page;
            if (page != null)
            {
                await page.DisplayAlertAsync("Error", message, "OK");
            }
        }
    }

    public async Task ShowErrors(IEnumerable<string> messages)
    {
        var list = messages?.Where(m => !string.IsNullOrWhiteSpace(m)).ToList();
        if (list is null || list.Count == 0) return;

        // Show first as toast, others concatenated if needed
        await ShowError(list[0]);
        if (list.Count > 1)
        {
            var rest = string.Join("\n", list.Skip(1));
            await ShowError(rest);
        }
    }
}
