using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

    public int nextModuleHeight;
    public int uniqueModules;
    public GameObject[] Modules;

    private bool nextModuleGenerated;
    private string nextModuleName;
    private Vector2 nextModPos;

    GameObject gameCamera;

	void Start () {
        nextModuleGenerated = false;

        nextModPos = new Vector2(transform.position.x, transform.position.y + nextModuleHeight);
        gameCamera = GameObject.Find("Main Camera");
    }
	
	void Update () {
        if (gameCamera.transform.position.y > transform.position.y + 80)
            Destroy(gameObject);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (nextModuleGenerated)
            return;
        nextModuleGenerated = true;
        var module = (GameObject)Instantiate(Modules[Random.Range(0, Modules.Length)], nextModPos, transform.rotation);
        if (Random.value < 0.5f)
        {
            module.transform.localScale = new Vector3(module.transform.localScale.x*-1, module.transform.localScale.y, module.transform.localScale.z);
            Debug.Log("REVERSO");
        }
    }
}
