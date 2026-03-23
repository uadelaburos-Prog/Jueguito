using UnityEngine;
using UnityEngine.SceneManagement;

public class leMenuController : MonoBehaviour
{
    public void MenuPlay()
    {
        SceneManager.LoadScene("Nivel de Pruebas");
    }

    public void GameClose()
    {
        Application.Quit();
    }
}