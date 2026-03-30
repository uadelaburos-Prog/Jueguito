using UnityEngine;
using UnityEngine.SceneManagement;
public class lePauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenus;

    private void Start()
    {
        pauseMenus = GetComponent<GameObject>();
    }

    public void lePause()
    {  
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenus.SetActive(true);
            Time.timeScale = 0f;
        }
        
    }
    public void leContinue()
    {
        Time.timeScale = 1f;
    }
    public void leClose()
    {
        Application.Quit();
    }
}
