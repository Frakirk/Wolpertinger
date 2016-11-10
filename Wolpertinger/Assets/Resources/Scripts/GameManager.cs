using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool[] players;
    public bool gamePaused;
    public GameObject pauseScreenPrefab;
    public GameObject pauseScreenInstance;
    public RuntimeAnimatorController sable;
    public RuntimeAnimatorController sealpoint;
    //public float hitstopTime;

    void Awake () {
        DontDestroyOnLoad(this.gameObject);
        players = new bool[4];
        gamePaused = false;
        GameObject.Find("Game Camera").GetComponent<CameraPosScale>().gameManager = gameObject;
        GameObject.Find("Game Camera").GetComponent<CameraPosScale>().CountPlayers();
        //hitstopTime = 0;
    }

    //void Update()
    //{
    //    if (hitstopTime > 0 && !gamePaused)
    //    {
    //        hitstopTime -= 0.05f;
    //        if (hitstopTime < 0)
    //        {
    //            Time.timeScale = 1;
    //        }  
    //    }
    //}

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
        for (int i = 1; i < 5; i++)
        {
            if (players[i - 1])
            {
                var spawnPos = (GameObject.Find("p" + i.ToString() + "Spawn")).transform.position;
                var player = (GameObject)Instantiate(Resources.Load("Player"), spawnPos, transform.rotation);
                player.GetComponent<GamepadInput>().player = 'P' + i.ToString();
                if (i == 2)
                    player.GetComponent<Animator>().runtimeAnimatorController = sable;
                if (i == 3)
                    player.GetComponent<Animator>().runtimeAnimatorController = sealpoint;
                player.tag = "Player" + i.ToString();
            }
        }
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
