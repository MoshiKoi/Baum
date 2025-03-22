using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using NotifyGenerator;
using Baum.DB;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace Baum.ViewModels;

public partial class ProjectViewModel : ViewModelBase, ISavableViewModel
{
    public string? FilePath { get; set; }
    public string TempFilePath { get; set; }

    [Notify]
    public partial ViewModelBase Content { get; set; }
    public ProjectViewModel(string? filePath = null)
    {
        FilePath = filePath;
        TempFilePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());

        Content = new ProjectForestViewModel(this, TempFilePath);
    }

    [RelayCommand]
    public void Save()
    {
        if (FilePath != null)
        {
            File.Copy(TempFilePath, FilePath, overwrite: true);
        }
    }

    public void OpenLanguage(int languageId)
    {
        Content = new LanguageViewModel(TempFilePath, languageId);
    }
}
