using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimpleNotes.PageModel;

public partial class EditNotePageModel(Repository repository) : BasePageModel, IQueryAttributable
{
    public Note Note { get; private set; } = new();

    [RelayCommand]
    private async Task SaveNote(Note note)
    {
        if (repository.CreateOrReplace(note))
        {
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "There was an error saving the note", "OK");
        }
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(Note), out var val) && val is Note note)
        {
            Note = note;
            OnPropertyChanged(nameof(Note));
        }
    }
}