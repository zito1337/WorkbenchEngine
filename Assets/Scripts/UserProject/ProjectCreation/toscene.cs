using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class toscene : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
