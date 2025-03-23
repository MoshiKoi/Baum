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

    [Notify]
    public partial string SoundChangeInput { get; set; } = string.Empty;

    public ObservableCollection<SoundChangeViewModel> SoundChanges { get; set; } = [];

    [Notify]
    public partial string WordInput { get; set; } = string.Empty;

    public ObservableCollection<WordViewModel> Words { get; set; } = [];

    public LanguageViewModel(string tempFilePath, int languageId)
    {
        TempFilePath = tempFilePath;
        LanguageId = languageId;
        LoadSoundChanges();
        LoadWords();
    }

    void LoadSoundChanges()
    {
        SoundChanges.Clear();

        using var context = new ProjectContext(TempFilePath);

        foreach (var soundChange in context.SoundChanges.Where(_ => _.LanguageId == LanguageId))
        {
            SoundChanges.Add(new SoundChangeViewModel
            {
                Id = soundChange.Id,
                Notation = soundChange.Notation
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
    public async Task AddSoundChange(CancellationToken cancellationToken)
    {
        using var context = new ProjectContext(TempFilePath);

        await context.SoundChanges.AddAsync(new SoundChange
        {
            LanguageId = LanguageId,
            Notation = SoundChangeInput
        }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        LoadSoundChanges();
    }

    [RelayCommand]
    async Task RemoveSoundChange(SoundChangeViewModel soundChange)
    {
        using var context = new ProjectContext(TempFilePath);

        var entity = context.SoundChanges.Find(soundChange.Id) ?? throw new Exception("Sound change doesn't exist");
        context.Remove(entity);
        await context.SaveChangesAsync();
        SoundChanges.Remove(soundChange);
    }

    [RelayCommand]
    public async Task AddWord(CancellationToken cancellationToken)
    {
        using var context = new ProjectContext(TempFilePath);

        await context.Words.AddAsync(new Word
        {
            LanguageId = LanguageId,
            Form = WordInput
        }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        LoadWords();
    }
}