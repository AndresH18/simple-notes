using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimpleNotes.PageModel;

public partial class BasePageModel : ObservableObject
{
    private bool _isInitialized;
    private long _isBusy;

    protected BasePageModel()
    {
        InitializeAsyncCommand =
            new AsyncRelayCommand(async () =>
            {
                if (_isInitialized)
                    return;
                await IsBusyFor(InitializeAsync);
                IsInitialized = true;
            }, AsyncRelayCommandOptions.None);
        RefreshAsyncCommand =
            new AsyncRelayCommand(async () =>
            {
                if (_isInitialized)
                {
                    await IsBusyFor(RefreshAsync);
                }
                else
                {
                    await IsBusyFor(InitializeAsync);
                    IsInitialized = true;
                }
            }, AsyncRelayCommandOptions.None);
    }

    public IAsyncRelayCommand InitializeAsyncCommand { get; }
    public IAsyncRelayCommand RefreshAsyncCommand { get; }

    public bool IsBusy => Interlocked.Read(ref _isBusy) > 0;


    public bool IsInitialized
    {
        get => _isInitialized;
        private set => SetProperty(ref _isInitialized, value);
    }

    protected virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual Task RefreshAsync()
    {
        return Task.CompletedTask;
    }


    protected async Task IsBusyFor(Func<Task> unitOfWork)
    {
        Interlocked.Increment(ref _isBusy);
        OnPropertyChanged(nameof(IsBusy));
        try
        {
            await unitOfWork();
        }
        finally
        {
            Interlocked.Decrement(ref _isBusy);
            OnPropertyChanged(nameof(IsBusy));
        }
    }
}