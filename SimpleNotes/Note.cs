using SQLite;

namespace SimpleNotes;

[Table("todo_items")]
public class Note
{
    public const int MaxTitleLength = 50;
    public const int MaxContentLength = 100_000;

    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    [MaxLength(MaxTitleLength)] public string Title { get; set; } = DateTime.Now.Date.ToShortDateString();
    [MaxLength(MaxContentLength)] public string Content { get; set; } = string.Empty;

    protected bool Equals(Note other)
    {
        return Id == other.Id && Title == other.Title && Content == other.Content;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Note)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Title, Content);
    }
}