using CommunityToolkit.Mvvm.Input;
using NotifyGenerator;

namespace Baum.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [Notify]
    public partial ViewModelBase Content { get; set; }

    public MainWindowViewModel()
    {
        var model = new HomeViewModel();

        Content = model;
    }

    [RelayCommand]
    void OpenProject()
    {
        Content = new ProjectViewModel();
    }

    bool CanSave()
    {
        return Content is ISavableViewModel;
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    void Save()
    {
        ((ISavableViewModel)Content).SaveCommand.Execute(null);
    }
}