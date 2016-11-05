using UnityEngine;
using System.Collections;

public class Killzone : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Substring(0, 6) == "Player")
            other.transform.position += new Vector3(0, 20);
    }
}
