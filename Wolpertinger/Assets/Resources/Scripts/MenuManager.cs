using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    GameObject gameManager;
    public GameManager gmScript;
    public GameObject[] players;
    public CameraPosScale CameraScript;
    public RuntimeAnimatorController sable;

	// Use this for initialization
	void Start ()
    {
        players = new GameObject[4];
        for (int i = 0; i < 4; i++)
            players[i] = null;
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        if (gameManager == null)
            gameManager = (GameObject)Instantiate(Resources.Load("GameManager"), transform.position, transform.rotation);
        gmScript = gameManager.GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("Escape"))
            Application.Quit();
        if (gmScript.gamePaused)
            return;
        for (int i = 0; i < 4; i++)
        {
            if (Input.GetButtonDown("P"+(i+1).ToString()+"Start") && players[i] == null)
                players[i] = CreatePlayer(i + 1, new Vector2(transform.position.x + i * 2, transform.position.y));

            if (Input.GetButtonDown("P" + (i + 1).ToString() + "BtnB") && players[i] != null)
                players[i] = DestroyPlayer(i, players[i]);
        }


    }

    GameObject CreatePlayer(int playerID, Vector2 playerPos)
    {
        var player = (GameObject)Instantiate(Resources.Load("Player"), playerPos, transform.rotation);
        player.GetComponent<GamepadInput>().player = 'P' + playerID.ToString();
        if (playerID == 2)
            player.GetComponent<Animator>().runtimeAnimatorController = sable;
        player.tag = "Player" + playerID.ToString();
        gmScript.players[playerID-1] = true;
        CameraScript.CountPlayers();

        return player;
    }

    GameObject DestroyPlayer(int playerID, GameObject player)
    {
        Destroy(player);
        gmScript.players[playerID] = false;
        CameraScript.CountPlayers();
        return null;
    }

}
