using System.IO;
using System.Text.Json;

namespace ProjectBreakout;

internal static class ScoreManager
{
    private static int Score { get; set; }

    public static int IncrementScore(int pNumber)
    {
        Score += pNumber;
        return Score;
    }

    public static void SaveScore()
    {
        string fileName = "LastScore.json";
        string jsonString = JsonSerializer.Serialize(Score);
        File.WriteAllText(fileName, jsonString);
    }

    public static int LoadScore()
    {
        string fileName = "LastScore.json";
        string jsonString = File.ReadAllText(fileName);
        Score = JsonSerializer.Deserialize<int>(jsonString);
        return Score;
    }
}
