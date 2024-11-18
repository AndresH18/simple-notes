using CommunityToolkit.Mvvm.Input;

namespace SimpleNotes.PageModel;

public partial class EditNotePageModel(Repository repository) : BasePageModel, IQueryAttributable
{
    private bool _isNewNote = true;
    public Note Note { get; private set; } = new();

    [RelayCommand]
    private void SaveNote()
    {
        if (_isNewNote)
            repository.Create(Note);
        else
            repository.Update(Note);
    }

    [RelayCommand]
    private async Task DeleteNote()
    {
        if (!_isNewNote)
        {
            repository.Delete(Note.Id);
        }

        await Shell.Current.GoToAsync("..");
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Id", out var value) && value is int id)
        {
            _isNewNote = false;
            Note = repository.Get(id) ?? new Note();
            OnPropertyChanged(nameof(Note));
        }
    }
}