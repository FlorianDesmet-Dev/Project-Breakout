using System.IO;
using System.Text.Json;

namespace ProjectBreakout;

internal class LevelManager
{
    public Level Level { get; private set; }
    public int NumberLevel { get; private set; }

    public LevelManager() 
    {
        
    }

    public void LoadLevel(int pNumberLevel)
    {
        NumberLevel = pNumberLevel;

        string fileName = "../../../Levels/Level_" + NumberLevel + ".json";
        string levelJsonString = File.ReadAllText(fileName);
        Level = JsonSerializer.Deserialize<Level>(levelJsonString);       
        Level.Load();
    }

    public void Unload()
    {
        Level.Unload();
    }

    public void NextLevel()
    {
        if (Level != null)
        {
            Level.Unload();
            Level = null;
        }

        LoadLevel(NumberLevel + 1);
    }

    public void Draw()
    {
        Level.Draw();
    }
}
