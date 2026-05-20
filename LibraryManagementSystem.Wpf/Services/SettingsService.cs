using System.Text.Json;
using System.IO;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Services;

public class SettingsService
{
    private const string FileName = "settings.json";
    private Settings _settings = new();

    public SettingsService()
    {
        Load();
    }

    public Settings Get() => _settings;

    public void Update(Settings settings)
    {
        _settings = settings;
        Save();
    }

    private void Load()
    {
        try
        {
            if (!File.Exists(FileName))
            {
                _settings = new Settings();
                Save();
                return;
            }

            var json = File.ReadAllText(FileName);
            _settings = JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
        }
        catch
        {
            _settings = new Settings();
        }
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FileName, json);
    }
}
