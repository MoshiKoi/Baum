using Baum.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NotifyGenerator;
using CommunityToolkit.Mvvm.Input;
using System.Threading;

namespace Baum.ViewModels;

public class LanguageSelectorItem
{
    public string Name { get; set; }
    public int LanguageId { get; set; }
}

public partial class LanguageViewModel : ViewModelBase
{
    public string TempFilePath { get; set; }
    public int LanguageId { get; set; }

    public ObservableCollection<LanguageSelectorItem> Languages { get; set; } = [];

    public LanguageViewModel(string tempFilePath, int languageId)
    {
        TempFilePath = tempFilePath;
        LanguageId = languageId;
        LoadLanguages();
        LoadWords();
    }


    [Notify]
    public partial string TextBoxContent { get; set; } = string.Empty;

    public ObservableCollection<WordViewModel> Words { get; set; } = [];

    void LoadLanguages()
    {
        Languages.Clear();

        using var context = new ProjectContext(TempFilePath);

        foreach (var language in context.Languages)
        {
            Languages.Add(new LanguageSelectorItem
            {
                LanguageId = language.Id,
                Name = language.Name
            });
        }
    }

    void LoadWords()
    {
        Words.Clear();

        using var context = new ProjectContext(TempFilePath);

        foreach (var word in context.Words.Where(_ => _.LanguageId == LanguageId))
        {
            Words.Add(new WordViewModel { Form = word.Form });
        }
    }

    [RelayCommand]
    public async Task AddWord(CancellationToken cancellationToken)
    {
        using var context = new ProjectContext(TempFilePath);

        await context.Words.AddAsync(new Word
        {
            LanguageId = LanguageId,
            Form = TextBoxContent
        }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        LoadWords();
    }
}