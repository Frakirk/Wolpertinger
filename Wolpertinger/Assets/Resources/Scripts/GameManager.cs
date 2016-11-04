using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool[] players;

	void Start () {
        DontDestroyOnLoad(this.gameObject);
        players = new bool[4];
	}

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelStart;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelStart;
    }

    void OnLevelStart(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Entering " + (SceneManager.GetActiveScene()).name);
        if ((SceneManager.GetActiveScene()).name != "Menu")
        {
            for (int i = 1; i < 5; i++)
            {
                if (players[i - 1])
                {
                    var spawnPos = (GameObject.Find("p" + i.ToString() + "Spawn")).transform.position;
                    var player = (GameObject)Instantiate(Resources.Load("Player"), spawnPos, transform.rotation);
                    player.GetComponent<GamepadInput>().player = 'P' + i.ToString();
                    player.tag = "Player" + i.ToString();
                }
            }
        }
    }
	
}
