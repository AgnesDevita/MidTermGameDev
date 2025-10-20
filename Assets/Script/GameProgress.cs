using UnityEngine;

public static class GameProgress
{
    private static int persistentScore = 0;
    private static int persistentDiamonds = 0;
    
    public static void SaveProgress(int score, int diamonds)
    {
        persistentScore = score;
        persistentDiamonds = diamonds;
        Debug.Log($"[GameProgress] Saved: Score={score}, Diamonds={diamonds}");
    }
    
    public static void LoadProgress(out int score, out int diamonds)
    {
        score = persistentScore;
        diamonds = persistentDiamonds;
        Debug.Log($"[GameProgress] Loaded: Score={score}, Diamonds={diamonds}");
    }
    
    public static void ResetProgress()
    {
        persistentScore = 0;
        persistentDiamonds = 0;
        Debug.Log("[GameProgress] Reset to 0");
    }
    
    public static int GetTotalDiamonds() => persistentDiamonds;
    public static int GetTotalScore() => persistentScore;
}
