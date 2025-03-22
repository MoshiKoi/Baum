using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Baum.DB;
using CommunityToolkit.Mvvm.Input;
using NotifyGenerator;

namespace Baum.ViewModels;

public partial class ProjectForestViewModel : ViewModelBase
{
    ProjectViewModel Project { get; }
    public string TempFilePath { get; }

    [Notify]
    public partial string TextBoxContent { get; set; } = string.Empty;

    public ObservableCollection<LanguageItemViewModel> Languages { get; set; } = [];

    public ProjectForestViewModel(ProjectViewModel project, string tempFilePath)
    {
        Project = project;
        TempFilePath = tempFilePath;
        LoadLanguages();
    }

    void LoadLanguages()
    {
        Languages.Clear();

        using var context = new ProjectContext(TempFilePath);

        foreach (var language in context.Languages)
        {
            Languages.Add(new LanguageItemViewModel
            {
                Parent = this,
                LanguageId = language.Id,
                Name = language.Name
            });
        }
    }

    [RelayCommand]
    public async Task AddLanguage(CancellationToken cancellationToken)
    {
        using var context = new ProjectContext(TempFilePath);

        await context.Languages.AddAsync(new Language { Name = TextBoxContent }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        LoadLanguages();
    }

    public void OpenLanguage(int languageId)
    {
        Project.OpenLanguage(languageId);
    }
}
