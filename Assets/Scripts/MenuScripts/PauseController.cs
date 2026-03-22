using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private void Start()
    {
        pauseMenu = GetComponent<GameObject>();
    }
    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        
    }
    public void Continue()
    {
        Time.timeScale = 1f;
    }
    public void Close()
    {
        Application.Quit();
    }
}
