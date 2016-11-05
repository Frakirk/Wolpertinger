using UnityEngine;
using System.Collections;

public class CameraPosScale : MonoBehaviour {

    float xPos, yPos;
    Vector2 selfPos;
    Transform selfTransform;
    Transform[] playerTransforms;
    public GameObject gameManager;
    Camera gameCamera;
    bool[] players;
    float[] xPositions;
    float[] yPositions;

    public float screenSizeSmall, screenSizeLarge;

    void Start ()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        gameCamera = (GameObject.Find("Main Camera")).GetComponent<Camera>();
        xPositions = new float[4];
        yPositions = new float[4];
        playerTransforms = new Transform[4];
        if (gameManager != null)
        {
            CountPlayers();
        }
        xPos = 0;
        yPos = 0;
        selfTransform = GetComponent<Transform>();
    }

    void Update()
    {
        var j = 0;
        while (players[j] == false)
        {
            j++;
            if (j > 3)
                break;
        }
        for (int i = 0; i < 4; i++)
        {
            if (players[i])
            {
                xPositions[i] = playerTransforms[i].position.x;
                yPositions[i] = playerTransforms[i].position.y;
            }
            else if (j <= 3)
            {
                xPositions[i] = playerTransforms[j].position.x;
                yPositions[i] = playerTransforms[j].position.y;
            }
        }

        selfPos = selfTransform.position;

        var maxX = Mathf.Max(xPositions);
        var minX = Mathf.Min(xPositions);

        xPos = (maxX + minX) * 0.5f;
        xPos = (xPos - selfPos.x) * Time.deltaTime;

        yPos = Mathf.Max(yPositions) + 4 - (gameCamera.orthographicSize*gameCamera.orthographicSize*0.05f); // camera's target position relative to the top player scales inversely with orthographic size
        yPos = (yPos - selfPos.y) * Time.deltaTime*2;

        transform.position += new Vector3(xPos, yPos);

        var playerDist = Vector2.Distance(new Vector2(maxX, Mathf.Max(yPositions)+5), new Vector2(minX, Mathf.Min(yPositions)))*0.75f; // the sneaky +5 to Max(yPositions) boosts the vertical distance value, making the zoom more consistent between x and y

        gameCamera.orthographicSize += (playerDist - gameCamera.orthographicSize) * Time.deltaTime;
        if (gameCamera.orthographicSize < screenSizeSmall)
            gameCamera.orthographicSize += (screenSizeSmall - gameCamera.orthographicSize) * Time.deltaTime * 4;
        else if (gameCamera.orthographicSize > screenSizeLarge)
            gameCamera.orthographicSize += (screenSizeLarge - gameCamera.orthographicSize) * Time.deltaTime * 4;
    }

    public void CountPlayers()
    {
        players = gameManager.GetComponent<GameManager>().players;
        for (int i = 0; i < 4; i++)
        {
            if (players[i])
            {
                var player = GameObject.FindGameObjectWithTag("Player" + (i + 1).ToString());
                playerTransforms[i] = player.transform;
            }
        }
    }
}
