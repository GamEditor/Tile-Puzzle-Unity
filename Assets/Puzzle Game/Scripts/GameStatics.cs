using UnityEngine;

public static class GameStatics
{
    #region Puzzle Pieces
    public static int m_Rows = default(int);
    public static int m_Columns = default(int);

    public static float m_WatchingDelayTime;

    public static Material m_Material;
    #endregion

    #region Data Manager on Run Time
    public static int m_LevelID;

    // these table's name will be concatinated with m_LevelID
    public static string m_LevelStatusTableName;
    public static string m_PlayStatusTableName;

    public static void ChangePlayStatus()
    {
        PlayerPrefs.SetString(m_PlayStatusTableName + m_LevelID, "played");
    }

    public static void UnlockNextLevel()
    {
        int next = m_LevelID + 1;
        PlayerPrefs.SetString(m_LevelStatusTableName + next, "unlocked");
    }

    public static void UnlockLevelsByID()
    {
        //PlayerPrefs.SetString(m_LevelStatusTableName + next, "unlocked");
    }
    #endregion
}