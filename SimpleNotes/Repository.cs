using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using SQLite;

namespace SimpleNotes;

public class Repository
{
    private readonly string _dbPath;
    private readonly ILogger<Repository> _logger;
    private SQLiteConnection? _connection;

    public Repository(string dbPath, ILogger<Repository> logger)
    {
        _dbPath = dbPath;
        _logger = logger;
    }

    public List<Note> GetAll()
    {
        try
        {
            Init();
            return _connection.Table<Note>().ToList();
        }
        catch (Exception ex)
        {
            return [];
        }
    }

    public bool CreateOrReplace(Note item)
    {
        try
        {
            Init();
            return _connection.InsertOrReplace(item) != 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error inserting note");
            return false;
        }
    }

    public bool Delete(Note item)
    {
        try
        {
            Init();
            return _connection.Delete<Note>(item.Id) != 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting note");
            return false;
        }
    }

    [MemberNotNull(nameof(_connection))]
    private void Init()
    {
        if (_connection != null)
            return;

        _connection = new SQLiteConnection(_dbPath);
        _connection.CreateTable<Note>();
    }
}