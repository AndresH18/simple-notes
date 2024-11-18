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

    protected override void OnAppearing()
    {
        _model.RefreshAsyncCommand.Execute(null);
        ItemsCollectionView.SelectedItem = null;
    }
}