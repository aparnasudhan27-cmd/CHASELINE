using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelNavigation : MonoBehaviour
{
    public void LoadLevel2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
    }
    public void LoadLevel3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level3");
    }
    public void RetryLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
