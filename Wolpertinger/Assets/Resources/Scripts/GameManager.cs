using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool[] players;
    public bool gamePaused;
    public GameObject pauseScreenPrefab;
    public GameObject pauseScreenInstance;

	void Awake () {
        DontDestroyOnLoad(this.gameObject);
        players = new bool[4];
        gamePaused = false;
        GameObject.Find("Game Camera").GetComponent<CameraPosScale>().gameManager = gameObject;
        GameObject.Find("Game Camera").GetComponent<CameraPosScale>().CountPlayers();
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
        //if ((SceneManager.GetActiveScene()).name != "Menu")
        //{
            for (int i = 1; i < 5; i++)
            {
                if (players[i - 1])
                {
                    var spawnPos = (GameObject.Find("p" + i.ToString() + "Spawn")).transform.position;
                    var player = (GameObject)Instantiate(Resources.Load("Player"), spawnPos, transform.rotation);
                    player.GetComponent<GamepadInput>().player = 'P' + i.ToString();
                    player.tag = "Player" + i.ToString();
                    //if ((SceneManager.GetActiveScene()).name == "Menu")
                        //var menuPlayers = GameObject.Find("MenuManager").GetComponent<MenuManager>().players;
                }
            }
        //}
    }
	
    public void PauseGame()
    {
        gamePaused = !gamePaused;
        if (pauseScreenInstance != null)
            Destroy(pauseScreenInstance);
        if (gamePaused)
        {
            pauseScreenInstance = (GameObject)Instantiate(pauseScreenPrefab, transform.position, transform.rotation);
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;
    }
}
