using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SimpleNotes.Pages;

namespace SimpleNotes.PageModel;

public partial class MainPageModel(Repository repository, ILogger<MainPageModel> logger) : BasePageModel
{
    public ObservableCollection<Note> Notes { get; } = [];

    protected override Task InitializeAsync()
    {
        var notes = repository.GetAll();

        MainThread.BeginInvokeOnMainThread(() => notes.ForEach(Notes.Add));
        return Task.CompletedTask;
    }

    protected override Task RefreshAsync()
    {
        var notes = repository.GetAll();
        MainThread.BeginInvokeOnMainThread(() =>
        {
            Notes.Clear();
            notes.ForEach(Notes.Add);
        });

        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task CreateNote()
    {
        await Shell.Current.GoToAsync(nameof(EditNotePage));
    }

    [RelayCommand]
    private async Task ViewNote(SelectionChangedEventArgs args)
    {
        logger.LogInformation("Entered View Note method");
        if (args.CurrentSelection.Count != 0 && args.CurrentSelection[0] is Note note)
        {
            await Shell.Current.GoToAsync($"{nameof(EditNotePage)}?Id={note.Id}");
        }
    }
}