using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

    public int nextModuleHeight;
    public int uniqueModules;

    private bool nextModuleGenerated;
    private string nextModuleName;

    GameObject gameCamera;

	void Start () {
        nextModuleGenerated = false;

        nextModuleName = "ModuleMidgard" + Random.Range(0, uniqueModules);
        while (nextModuleName == name)
            nextModuleName = "ModuleMidgard" + Random.Range(0, uniqueModules);

        gameCamera = GameObject.Find("Main Camera");
    }
	
	void Update () {
        if (gameCamera.transform.position.y > transform.position.y + 60)
            Destroy(gameObject);
	}

    void OnTriggerEnter2D(Collider2D other) {
        if (nextModuleGenerated)
            return;
        nextModuleGenerated = true;
        GameObject nextModule = (GameObject)Instantiate(Resources.Load(nextModuleName), transform.position, transform.rotation);
        nextModule.transform.position = new Vector2(transform.position.x, transform.position.y + nextModuleHeight);
        nextModule.GetComponent<LevelGenerator>().nextModuleHeight = nextModuleHeight;
    }
}
