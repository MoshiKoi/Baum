using System.ComponentModel.DataAnnotations.Schema;

namespace Baum.DB;

public class Language
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public int? ParentId { get; set; }
    public Language? Parent { get; set; }

    [InverseProperty(nameof(Language.Parent))]
    public List<Language> Children { get; set; } = [];

    [InverseProperty(nameof(SoundChange.Language))]
    public List<SoundChange> SoundChanges { get; set; } = [];

    [InverseProperty(nameof(Word.Language))]
    public List<Word> Words { get; set; } = [];
}
