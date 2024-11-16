using SimpleNotes.PageModel;

namespace SimpleNotes.Pages;

public partial class MainPage : ContentPage
{
    private readonly MainPageModel _model;

    public MainPage(MainPageModel model)
    {
        InitializeComponent();
        BindingContext = _model = model;
    }

    protected override async void OnAppearing()
    {
        await _model.RefreshAsyncCommand.ExecuteAsync(null);
    }

    private async void ListView_OnItemSelected(object? sender, SelectedItemChangedEventArgs e)
    {
        await _model.ViewNoteCommand.ExecuteAsync(e);
    }
}