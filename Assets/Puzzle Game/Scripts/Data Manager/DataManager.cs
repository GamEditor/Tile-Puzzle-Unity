using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this class will be load at a single scene entire the game, and it will manage levels, their state (locked or unlocked), etc.
/// in other scenes, managers can access to some static methods of this class.
/// </summary>

public class DataManager : MonoBehaviour
{
    public string m_LevelIDTableName;       // int
    public string m_LevelStatusTableName;   // string
    public string m_PlayStatusTableName;    // string

    public LevelInformation[] m_InformationButtons;

    // retrieve informations from database and make UI.Buttons Interactable's true or false.
    void Awake()
    {
        m_InformationButtons = FindObjectsOfType<LevelInformation>();

        GameStatics.m_LevelStatusTableName = m_LevelStatusTableName;
        GameStatics.m_PlayStatusTableName = m_PlayStatusTableName;

        for (int i = 0; i < m_InformationButtons.Length; i++)
        {
            if (!PlayerPrefs.HasKey(m_LevelIDTableName + m_InformationButtons[i].m_LevelID))
            {
                PlayerPrefs.SetInt(m_LevelIDTableName + m_InformationButtons[i].m_LevelID, m_InformationButtons[i].m_LevelID);
                PlayerPrefs.SetString(m_LevelStatusTableName + m_InformationButtons[i].m_LevelID, m_InformationButtons[i].m_LevelID == 1 ? "unlocked" : "locked");
                PlayerPrefs.SetString(m_PlayStatusTableName + m_InformationButtons[i].m_LevelID, "notplayed");
            }
        }
    }
}