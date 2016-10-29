using UnityEngine;
using System.Collections;

public class Killzone : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            other.transform.position += new Vector3(0, 20);
    }
}
