using UnityEngine;
using System.Collections;

public class CameraPosScale : MonoBehaviour {

    Camera gameCamera;
    float xPos, yPos;
    Vector2 p1Pos, p2Pos, selfPos;
    Transform p1Transform, p2Transform, selfTransform;
    float playerDist;
    public float screenSizeSmall, screenSizeLarge;

	void Start ()
    {
        gameCamera = (GameObject.Find("Main Camera")).GetComponent<Camera>();
        p1Transform = (GameObject.Find("Player1")).transform;
        p2Transform = (GameObject.Find("Player2")).transform;
        xPos = 0;
        yPos = 0;
        selfTransform = GetComponent<Transform>();

    }

    void Update()
    {
        p1Pos = p1Transform.position;
        p2Pos = p2Transform.position;
        selfPos = selfTransform.position;

        playerDist = Vector2.Distance(p1Pos, p2Pos);

        xPos = (Mathf.Max(p1Pos.x, p2Pos.x) + Mathf.Min(p1Pos.x, p2Pos.x)) * 0.5f;
        xPos = (xPos - selfPos.x) * Time.deltaTime;

        yPos = Mathf.Max(p1Pos.y, p2Pos.y)-4;
        yPos = (yPos - selfPos.y) * Time.deltaTime*2;

        transform.position += new Vector3(xPos, yPos);
        //if (playerDist < screenSizeSmall)
        //    gameCamera.orthographicSize = screenSizeSmall;
        //else if (playerDist > screenSizeLarge)
        //    gameCamera.orthographicSize = screenSizeLarge;
        //else
        //    gameCamera.orthographicSize = playerDist;
    }
}
