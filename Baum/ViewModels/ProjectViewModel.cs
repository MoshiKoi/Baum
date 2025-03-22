using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using NotifyGenerator;
using Baum.DB;

namespace Baum.ViewModels;

public partial class ProjectViewModel : ViewModelBase
{
    public string? FilePath { get; set; }
    public string TempFilePath { get; set; }

    [Notify]
    public partial string TextBoxContent { get; set; } = string.Empty;

    public ObservableCollection<WordViewModel> Words { get; set; } = [];

    public ProjectViewModel(string? filePath = null)
    {
        FilePath = filePath;
        TempFilePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());

        if (FilePath != null)
        {
            File.Copy(FilePath, TempFilePath);
        }
        LoadWords();
    }

    void LoadWords()
    {
        Words.Clear();

        using var context = new ProjectContext(TempFilePath);

        foreach (var word in context.Words)
        {
            Words.Add(new WordViewModel { Form = word.Form });
        }
    }

    [RelayCommand]
    public void AddWord()
    {
        Words.Add(new WordViewModel { Form = TextBoxContent });
    }
}
