using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneNavigator : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
