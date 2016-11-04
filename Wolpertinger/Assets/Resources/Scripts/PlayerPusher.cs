using UnityEngine;
using System.Collections;

public class PlayerPusher : MonoBehaviour {

    public float pushForceX;
    public float pushForceY;
    private Rigidbody2D rbody;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.Substring(0,6) == "Player")
            rbody = other.GetComponent<Rigidbody2D>();
            if (rbody.velocity.y < pushForceY)
                rbody.velocity = new Vector2(rbody.velocity.x + pushForceX, pushForceY);
    }
}
