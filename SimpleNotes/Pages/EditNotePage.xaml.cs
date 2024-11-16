using SimpleNotes.PageModel;

namespace SimpleNotes.Pages;

public partial class EditNotePage : ContentPage
{
    public EditNotePage(EditNotePageModel model)
    {
        InitializeComponent();
        BindingContext = model;
    }
}