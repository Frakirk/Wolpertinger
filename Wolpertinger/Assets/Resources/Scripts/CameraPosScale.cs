using UnityEngine;
using System.Collections;

public class CameraPosScale : MonoBehaviour {

    float xPos, yPos;
    Vector2 selfPos;
    Transform selfTransform;
    Transform[] playerTransforms;
    //float playerDist;
    GameObject gameManager;
    bool[] players;
    float[] xPositions;
    float[] yPositions;

    //public float screenSizeSmall, screenSizeLarge;

    void Start ()
    {
        gameManager = GameObject.Find("GameManager");
        xPositions = new float[4];
        yPositions = new float[4];
        playerTransforms = new Transform[4];
        CountPlayers();
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

        //playerDist = Vector2.Distance(p1Pos, p2Pos);

        xPos = (Mathf.Max(xPositions) + Mathf.Min(xPositions)) * 0.5f;
        xPos = (xPos - selfPos.x) * Time.deltaTime;

        yPos = Mathf.Max(yPositions)-4;
        yPos = (yPos - selfPos.y) * Time.deltaTime*2;

        transform.position += new Vector3(xPos, yPos);
        //if (playerDist < screenSizeSmall)
        //    gameCamera.orthographicSize = screenSizeSmall;
        //else if (playerDist > screenSizeLarge)
        //    gameCamera.orthographicSize = screenSizeLarge;
        //else
        //    gameCamera.orthographicSize = playerDist;
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
