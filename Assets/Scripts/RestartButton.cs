using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public void RestartTheGame()
    {
        SceneManager.LoadScene(0);
    }
}
