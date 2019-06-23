using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton:
    private static GameManager _Instance;
    public static GameManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<GameManager>();

                if (_Instance == null)
                    throw new System.NullReferenceException("GameManager lost");

                return _Instance;
            }
            else
                return _Instance;
        }
    }

    [Header("Time and status")]
    private float m_WatchCompletedDelayTime = 1.5f; // it will be set by PuzzlePieces related value
    [Range(0.0f, 10.0f)] public float m_ShowWinPanelTime = 3.0f;
    [Range(0.0f, 10.0f)] public float m_AnimationMoveTime = 0.5f;
    public bool m_CanPlay = false;
    private DragIt[] m_Tiles;

    // Swap temporary stuffs:
    private Vector3 m_TempPosition;
    private int m_TempID;
    private int m_MovementCount;

    // references:
    [Header("UI stuffs")]
    public Text m_MovementsShowText;
    public GameObject m_WinPanel;

    [Header("Help Options")]
    public RawImage m_HelpImage;

    [Header("By Dragging")]
    public DragIt m_DraggingObject;
    public DragIt m_DropingObject;

    [Header("By Selection")]
    public DragIt m_SelectedFirst;
    public DragIt m_SelectedSecond;

    // MonoBehaviour methods:
    private void Start()
    {
        m_Tiles = FindObjectsOfType<DragIt>();

        GameStatics.ChangePlayStatus();

        CheckLevelManagerValues();
        StartGame();
    }

    // Methods:
    private void CheckLevelManagerValues()
    {
        m_WatchCompletedDelayTime = GameStatics.m_WatchingDelayTime;
    }

    public void StartGame()
    {
        CheckErrors();

        m_CanPlay = false;
        m_WinPanel.SetActive(false);

        m_HelpImage.material = GameStatics.m_Material;
        m_HelpImage.gameObject.SetActive(false);

        m_MovementCount = 0;
        m_MovementsShowText.text = "Moves: " + m_MovementCount;

        StartCoroutine(Disassemble(m_WatchCompletedDelayTime));
    }

    private void CheckErrors()
    {
        if (m_SelectedFirst != null)
        {
            m_SelectedFirst.GetComponent<Outline>().enabled = false;
            AnimateSelectedTile(new Vector3(1, 1, 1));
            m_SelectedFirst = null;
        }
    }

    IEnumerator Disassemble(float startDelayTime)
    {
        yield return new WaitForSeconds(startDelayTime);

        for (int i = 0; i < m_Tiles.Length; i++)
        {
            SwapPlace(ref m_Tiles[i], ref m_Tiles[Random.Range(0, m_Tiles.Length)]);
            yield return new WaitForFixedUpdate();
        }

        CheckWin(); // if rows and columns are set to 1, then user can not play;
        m_CanPlay = true;
    }

    public void CheckWin()
    {
        foreach (DragIt tile in m_Tiles)
            if (tile.m_WinID != tile.m_CurrentID)
                return;

        m_CanPlay = false;

        if (m_SelectedFirst != null)
            AnimateSelectedTile(new Vector3(1, 1, 1));

        int levelID = GameStatics.m_LevelID + 1;
        GameStatics.UnlockNextLevel();

        System.Action loadWinPanel = () => { m_WinPanel.SetActive(true); };
        Invoke(loadWinPanel.Method.Name, m_ShowWinPanelTime);
    }

    public void AnimateSelectedTile(Vector3 scaleAmount)
    {
        if (m_SelectedFirst != null)
            iTween.ScaleTo(m_SelectedFirst.gameObject, scaleAmount, 0.1f);
    }

    public void SwapPlace(ref DragIt first, ref DragIt second)
    {
        // Swap places
        if (first != null)
        {
            m_TempPosition = first.transform.position;
            m_TempID = first.m_CurrentID;

            // first
            first.m_CurrentID = second.m_CurrentID;
            first.transform.position = second.transform.position;

            // second
            second.m_CurrentID = m_TempID;
            second.transform.position = m_TempPosition;
        }
    }

    public void SwapPlaceSecondAnimated(ref DragIt first, ref DragIt second)
    {
        // Swap places
        if (first != null)
        {
            m_CanPlay = false;

            m_TempPosition = first.transform.position;
            m_TempID = first.m_CurrentID;

            // first
            first.m_CurrentID = second.m_CurrentID;
            first.transform.position = second.transform.position;

            // second
            second.transform.SetAsLastSibling();
            second.m_CurrentID = m_TempID;
            iTween.MoveTo(second.gameObject, m_TempPosition, m_AnimationMoveTime);

            if (first != second)
                m_MovementsShowText.text = "Moves: " + ++m_MovementCount;

            StartCoroutine(ControlPlaying(second.transform, m_TempPosition));
        }
    }

    public void SwapPlaceAnimated(ref DragIt first, ref DragIt second)
    {
        // Swap places
        if (first != null)
        {
            m_CanPlay = false;

            m_TempPosition = first.transform.position;
            m_TempID = first.m_CurrentID;

            // first
            first.m_CurrentID = second.m_CurrentID;
            iTween.MoveTo(first.gameObject, second.transform.position, m_AnimationMoveTime);

            // second
            second.transform.SetAsLastSibling();
            second.m_CurrentID = m_TempID;
            iTween.MoveTo(second.gameObject, m_TempPosition, m_AnimationMoveTime);

            if (first != second)
                m_MovementsShowText.text = "Moves: " + ++m_MovementCount;

            StartCoroutine(ControlPlaying(second.transform, m_TempPosition));
        }
    }

    IEnumerator ControlPlaying(Transform t, Vector3 desiredPosition)
    {
        if (t.position != desiredPosition)
            yield return new WaitUntil(() => Vector3.Distance(t.position, desiredPosition) <= 0.5f);

        t.position = desiredPosition;
        m_CanPlay = true;
    }
}