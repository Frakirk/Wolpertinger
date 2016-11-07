using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

    public float parallaxScale;
    private Transform gameCamera;
    private float baseX, baseY;

	// Use this for initialization
	void Start () {
        baseX = transform.position.x;
        baseY = transform.position.y;
        gameCamera = GameObject.Find("Game Camera").transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector2(baseX + (gameCamera.position.x - baseX)*parallaxScale, baseY + (gameCamera.position.y - baseY) * parallaxScale);
	}
}
