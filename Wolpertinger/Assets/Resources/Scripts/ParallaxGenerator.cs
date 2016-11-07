using UnityEngine;
using System.Collections;

public class ParallaxGenerator : MonoBehaviour {

    public GameObject parallaxPrefab;

    private float height;
    private float baseX, baseY;
    private Transform gameCamera;
    bool nextGenerated;
    bool first;
    float ps;

    // Use this for initialization
    void Start () {
        height = GetComponent<Renderer>().bounds.size.y;
        baseX = transform.position.x;
        baseY = transform.position.y;
        gameCamera = GameObject.Find("Game Camera").transform;
        nextGenerated = false;
        if (GameObject.FindGameObjectWithTag(gameObject.name) == null)
        {
            first = true;
            gameObject.tag = gameObject.name;
        }
        Debug.Log(first);
    }
	
	// Update is called once per frame
	void Update () {
        if (gameCamera.position.y > transform.position.y && !nextGenerated)
        {
            var nextParallax = (GameObject)Instantiate(parallaxPrefab, new Vector2(0, transform.position.y + height), transform.rotation);
            nextGenerated = true;
        }
    }
}
