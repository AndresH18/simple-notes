using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using SimpleNotes.Pages;

namespace SimpleNotes.PageModel;

public partial class MainPageModel(Repository repository) : BasePageModel
{
    public ObservableCollection<Note> Notes { get; } = [];

    protected override Task InitializeAsync()
    {
        var notes = repository.GetAll();

        MainThread.BeginInvokeOnMainThread(() => notes.ForEach(Notes.Add));
        notes.ForEach(Notes.Add);
        return Task.CompletedTask;
    }

    protected override Task RefreshAsync()
    {
        var notes = repository.GetAll();
        notes = notes.Where(n => !Notes.Contains(n)).ToList();
        MainThread.BeginInvokeOnMainThread(() => notes.ForEach(Notes.Add));

        return Task.CompletedTask;
    }

    [RelayCommand]
    private async Task AddNote()
    {
        await Shell.Current.GoToAsync(nameof(EditNotePage));
    }

    [RelayCommand]
    private async Task ViewNote(SelectedItemChangedEventArgs args)
    {
        if (args.SelectedItem is not Note note)
            return;

        // await Shell.Current.GoToAsync(nameof(EditNotePage),
        //     new Dictionary<string, object> { [nameof(Note)] = note });
    }
}