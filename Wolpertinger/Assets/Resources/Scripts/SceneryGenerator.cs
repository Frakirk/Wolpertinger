using UnityEngine;
using System.Collections;

public class SceneryGenerator : MonoBehaviour {

    public GameObject scenery;

    private float height;
    private float baseX, baseY;
    private Transform gameCamera;
    bool nextGenerated;


    // Use this for initialization
    void Start () {
        height = GetComponentInChildren<Renderer>().bounds.size.y;
        baseX = transform.position.x;
        baseY = transform.position.y;
        gameCamera = GameObject.Find("Game Camera").transform;
        nextGenerated = false;

	}
	
	// Update is called once per frame
	void Update () {
	    if (gameCamera.position.y > transform.position.y && !nextGenerated)
        {
            Instantiate(scenery, new Vector2 (baseX, baseY + height), transform.rotation);
            nextGenerated = true;
        }
	}
}
