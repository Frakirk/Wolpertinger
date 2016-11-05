using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

    public string nextScene;

    void OnTriggerEnter2D(Collider2D other)
    {
        GoToNextScene();
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
}
