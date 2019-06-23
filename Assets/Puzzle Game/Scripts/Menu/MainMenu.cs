using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject m_CancelPopUp;

    void Start()
    {
        m_CancelPopUp.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            m_CancelPopUp.SetActive(!m_CancelPopUp.activeInHierarchy);
    }

    public void CancelShop(GameObject shopPanel)
    {
        shopPanel.SetActive(false);
        m_CancelPopUp.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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