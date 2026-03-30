using UnityEngine;
using UnityEngine.SceneManagement;

public class leMenuController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameClose();
        }
    }
    public void MenuPlay()
    {
        SceneManager.LoadScene("Nivel de Pruebas");
    }

    public void GameClose()
    {
        Application.Quit();
    }
}