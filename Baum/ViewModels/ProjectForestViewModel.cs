using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Baum.DB;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using NotifyGenerator;

namespace Baum.ViewModels;

public partial class ProjectForestViewModel : ViewModelBase
{
    IDbContextFactory<ProjectContext> _dbContextFactory;

    ProjectViewModel Project { get; }

    [Notify]
    public partial string TextBoxContent { get; set; } = string.Empty;

    public ObservableCollection<ProjectTreeViewModel> Languages { get; set; } = [];

    public ProjectForestViewModel(ProjectViewModel project, string tempFilePath)
    {
        _dbContextFactory = new ProjectContextFactory(tempFilePath);
        Project = project;
        LoadLanguages();
    }

    void LoadLanguages()
    {
        Languages.Clear();

        using var context = _dbContextFactory.CreateDbContext();

        foreach (var language in context.Languages)
        {
            var model = new ProjectTreeViewModel(_dbContextFactory, language.Id, language.Name);
            model.OnOpen += (_, id) => OpenLanguage(id);
            Languages.Add(model);
        }
    }

    [RelayCommand]
    public async Task AddLanguage(CancellationToken cancellationToken)
    {
        using var context = _dbContextFactory.CreateDbContext();

        await context.Languages.AddAsync(new Language { Name = TextBoxContent }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        LoadLanguages();
    }

    public void OpenLanguage(int languageId)
    {
        Project.OpenLanguage(languageId);
    }
}
