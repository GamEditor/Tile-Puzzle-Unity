using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject m_CancelPopUp;

    void Start ()
    {
        m_CancelPopUp.SetActive(false);
    }

    private void Update()
    {
        if(GameManager.Instance.m_CanPlay == true && Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.m_CanPlay = false;
            m_CancelPopUp.SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.Instance.m_CanPlay = true;
            m_CancelPopUp.SetActive(false);
        }
    }

    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void ReplayCurrentGame()
    {
        GameManager.Instance.StartGame();
    }

    public void Resume(GameObject parent)
    {
        parent.SetActive(false);
        GameManager.Instance.m_CanPlay = true;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}