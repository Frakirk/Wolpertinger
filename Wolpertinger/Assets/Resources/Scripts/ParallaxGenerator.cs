using UnityEngine;
using System.Collections;

public class ParallaxGenerator : MonoBehaviour {

    public GameObject parallaxPrefab;

    private float height;
    private float baseX, baseY;
    private Transform gameCamera;
    int numGenerated;
    bool nextGenerated;
    float ps;

    // Use this for initialization
    void Start () {
        height = GetComponent<Renderer>().bounds.size.y;
        baseX = transform.position.x;
        baseY = transform.position.y;
        gameCamera = GameObject.Find("Game Camera").transform;
        numGenerated = 0;
        nextGenerated = false;
    }
	
	void Update () {
        if (gameCamera.position.y > transform.position.y + height*numGenerated && !nextGenerated)
        {
            numGenerated++;
            nextGenerated = true;
            var nextParallax = (GameObject)Instantiate(parallaxPrefab, new Vector2(0, transform.position.y + height*numGenerated), transform.rotation);
            nextParallax.transform.SetParent(transform);
            nextParallax.transform.position = new Vector2(transform.position.x, transform.position.y + height * numGenerated);
            nextParallax.GetComponent<Parallax>().enabled = false;
        }
    }
}
