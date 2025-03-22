using System.ComponentModel.DataAnnotations.Schema;

namespace Baum.DB;

public class Language
{
    public int Id { get; set; }
    public Language? Parent { get; set; }

    [InverseProperty(nameof(Word.Language))]
    public required List<Word> words;
}
