using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using SQLite;

namespace SimpleNotes;

public sealed class Repository : IDisposable
{
    private readonly string _dbPath;
    private readonly ILogger<Repository> _logger;
    private SQLiteConnection? _connection;
    private readonly Lock _lock = new();

    public Repository(string dbPath, ILogger<Repository> logger)
    {
        _dbPath = dbPath;
        _logger = logger;
    }

    public List<Note> GetAll()
    {
        lock (_lock)
        {
            try
            {
                Init();
                return _connection.Table<Note>().OrderByDescending(n => n.Id).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting notes");
                return [];
            }
        }
    }

    public Note? Get(int id)
    {
        lock (_lock)
        {
            try
            {
                Init();
                return _connection.Table<Note>().FirstOrDefault(n => n.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting note {id}", id);
                return null;
            }
        }
    }

    public bool Create(Note item)
    {
        lock (_lock)
        {
            try
            {
                Init();
                return _connection.Insert(item) != 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error inserting note");
                return false;
            }
        }
    }

    public bool Update(Note note)
    {
        lock (_lock)
        {
            try
            {
                Init();
                return _connection.Update(note) != 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error updating note");
                return false;
            }
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            try
            {
                Init();
                return _connection.Delete<Note>(id) != 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error deleting note {id}", id);
                return false;
            }
        }
    }

    public void Dispose() => _connection?.Dispose();

    [MemberNotNull(nameof(_connection))]
    private void Init()
    {
        if (_connection != null)
            return;

        _connection = new SQLiteConnection(_dbPath);
        _connection.CreateTable<Note>();
    }
}