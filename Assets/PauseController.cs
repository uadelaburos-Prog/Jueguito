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
        if (Input.GetKey(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            //pauseMenu.SetActive(true);            
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
