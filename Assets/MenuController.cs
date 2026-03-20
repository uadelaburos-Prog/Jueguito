using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Nivel de Pruebas");
    }

    public void Close()
    {
        Application.Quit();
    }
}