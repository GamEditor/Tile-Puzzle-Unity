using UnityEngine;
using UnityEngine.UI;

public class LevelGenerator : MonoBehaviour
{
    [Header("Tiles materix")]
    // the public references are related to the own scene and should not to be set with any other part of scripts.
    public Transform m_InstantiateParent;
    public RectTransform m_Center;  // will removed
    public GameObject m_TilePrefab;
    
    // these are set by PuzzlePieces values
    private int m_Rows = 1;
    private int m_Columns = 1;
    private Material m_Material;    // this material will be set to all RawImages
    private GameObject[,] m_Tiles;
    
    void CheckLevelManagerValues()
    {
        if (Mathf.Abs(GameStatics.m_Rows) > 0)
            m_Rows = GameStatics.m_Rows;

        if (Mathf.Abs(GameStatics.m_Columns) > 0)
            m_Columns = GameStatics.m_Columns;

        m_Material = GameStatics.m_Material;
    }

	void Awake()
    {
        CheckLevelManagerValues();

        m_Tiles = new GameObject[m_Rows, m_Columns];

        int winID = 0; // for setting each tile's win id
        float uvWidth = 1.0f / m_Columns;
        float uvHeight = 1.0f / m_Rows;

        for (int i = 0; i < m_Rows; i++)
            for (int j = 0; j < m_Columns; j++)
            {
                m_Tiles[i, j] = Instantiate(m_TilePrefab, m_InstantiateParent);
                m_Tiles[i, j].name = "Tile " + i + ", " + j;

                DragIt temp = m_Tiles[i, j].GetComponent<DragIt>();
                temp.m_WinID = winID;
                temp.m_CurrentID = winID++;
                temp.GetComponent<RawImage>().material = m_Material;
                temp.GetComponent<RawImage>().uvRect = new Rect(j * uvWidth, i * uvHeight, uvWidth, uvHeight);

                RectTransform rect = m_Tiles[i, j].GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(Screen.width / m_Columns, Screen.height / m_Rows);
                rect.anchoredPosition = new Vector3(j * rect.rect.width, i * rect.rect.height, 0);
            }

        m_Center.sizeDelta = m_Tiles[0, 0].GetComponent<RectTransform>().sizeDelta;
        m_Center.anchoredPosition = new Vector2(((m_Center.rect.width * m_Columns) / 2) - (m_Center.rect.width / 2),
                                        ((m_Center.rect.height * m_Rows) / 2) - (m_Center.rect.height / 2));

        foreach (GameObject drg in m_Tiles)
            drg.transform.SetParent(m_Center);

        m_Center.anchoredPosition = new Vector2(0, 0);
	}
}