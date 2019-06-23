using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// this manager will attach to each button and finally each botton will execute the LoadGameScene().
/// </summary>
public class LevelManager : MonoBehaviour
{
    // PuzzlePieces Controllers:
    [Header("PuzzlePieces Controllers:")]
    public int m_Rows;
    public int m_Columns;
    public float m_WatchingDelayTime = 1.5f;
    public Material m_Material;

    [Header("Scene Manager:")]
    public int m_BuildIndex;

    [SerializeField] private Text m_SizeOfPuzzleText;

    private LevelInformation m_LevelInfo;

    private void Awake()
    {
        m_LevelInfo = GetComponent<LevelInformation>();

        m_SizeOfPuzzleText = GetComponentInChildren<Text>();
        m_SizeOfPuzzleText.text = m_Columns + "x" + m_Rows;
    }

    public void OnClick()
    {
        GameStatics.m_Rows = m_Rows;
        GameStatics.m_Columns = m_Columns;
        GameStatics.m_WatchingDelayTime = m_WatchingDelayTime;
        GameStatics.m_Material = m_Material;

        GameStatics.m_LevelID = m_LevelInfo.m_LevelID;

        SceneManager.LoadScene(m_BuildIndex);
    }
}