using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    GameObject p1, p2, p3, p4, gameManager;
    public CameraPosScale CameraScript;
    //public GameObject gameManagerPrefab;

	// Use this for initialization
	void Start ()
    {
        p1 = p2 = p3 = p4 = null;
        gameManager = GameObject.Find("GameManager");
        if (gameManager == null)
            gameManager = (GameObject)Instantiate(Resources.Load("GameManager"), transform.position, transform.rotation);
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetButtonDown("P1Start") && p1 == null)
            p1 = CreatePlayer(1, new Vector2(transform.position.x, transform.position.y));
        if (Input.GetButtonDown("P1BtnB") && p1 != null)
            p1 = DestroyPlayer(1, p1);

        if (Input.GetButtonDown("P2Start") && p2 == null)
            p2 = CreatePlayer(2, new Vector2(transform.position.x+2, transform.position.y));
        if (Input.GetButtonDown("P2BtnB") && p2 != null)
            p2 = DestroyPlayer(2, p2);

        if (Input.GetButtonDown("P3Start") && p3 == null)
            p3 = CreatePlayer(3, new Vector2(transform.position.x+4, transform.position.y));
        if (Input.GetButtonDown("P3BtnB") && p3 != null)
            p3 = DestroyPlayer(3, p3);

        if (Input.GetButtonDown("P4Start") && p4 == null)
            p4 = CreatePlayer(4, new Vector2(transform.position.x+6, transform.position.y));
        if (Input.GetButtonDown("P4BtnB") && p4 != null)
            p4 = DestroyPlayer(4, p4);

    }

    GameObject CreatePlayer(int playerID, Vector2 playerPos)
    {
        var player = (GameObject)Instantiate(Resources.Load("Player"), playerPos, transform.rotation);
        player.GetComponent<GamepadInput>().player = 'P' + playerID.ToString();
        player.tag = "Player" + playerID.ToString();
        gameManager.GetComponent<GameManager>().players[playerID-1] = true;
        CameraScript.CountPlayers();

        return player;
        
    }

    GameObject DestroyPlayer(int playerID, GameObject player)
    {
        Destroy(player);
        gameManager.GetComponent<GameManager>().players[playerID-1] = false;
        CameraScript.CountPlayers();
        return null;
    }

}
