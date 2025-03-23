using Baum.DB;
using Baum.Views;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baum.ViewModels;

public partial class ProjectTreeViewModel : ViewModelBase
{
    IDbContextFactory<ProjectContext> _projectContextFactory;

    public int LanguageId { get; set; }
    public string Name { get; set; }

    public ObservableCollection<ProjectTreeViewModel> Children { get; set; }

    public ProjectTreeViewModel(IDbContextFactory<ProjectContext> contextFactory, int languageId, string name)
    {
        _projectContextFactory = contextFactory;
        LanguageId = languageId;
        Name = name;

        List<Language> children;
        using (var context = contextFactory.CreateDbContext())
        {
            children = context.Languages.Find(languageId)
                ?.Children
                ?.ToList() ?? throw new Exception("Language could not be found");
        }

        Children = [.. children.Select(_ => new ProjectTreeViewModel(contextFactory, _.Id, _.Name))];
    }

    [RelayCommand]
    async Task AddChild()
    {
        var child = new Language { Name = "Unnamed", ParentId = LanguageId };

        using (var context = _projectContextFactory.CreateDbContext())
        {
            await context.AddAsync(child);
            await context.SaveChangesAsync();
        }

        Children.Add(new ProjectTreeViewModel(_projectContextFactory, child.Id, child.Name));
    }

    public event EventHandler<int> OnOpen;

    [RelayCommand]
    void Open()
    {
        OnOpen?.Invoke(this, LanguageId);
    }
}
