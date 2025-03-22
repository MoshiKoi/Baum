namespace Baum.DB;

public class Word
{
    public int Id { get; set; }
    public required Language Language { get; set; }
    public required string Form { get; set; }
}
