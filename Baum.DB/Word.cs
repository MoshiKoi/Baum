namespace Baum.DB;

public class Word
{
    public int Id { get; set; }

    public int LanguageId { get; set; }
    public Language Language { get; set; }
    public string Form { get; set; }
}
