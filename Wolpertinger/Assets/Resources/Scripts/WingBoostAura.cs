﻿using UnityEngine;
using System.Collections;

public class WingBoostAura : MonoBehaviour {

    public GameObject parent;
    public float pushForce;
    public float duration;
    private float startTime, currentTime;
    private Rigidbody2D rbody;

    void Start()
    {
        startTime = Time.time;
    }

	void Update()
    {
        if (parent != null)
            transform.position = parent.transform.position;
        currentTime = Time.time - startTime;
        if (currentTime > duration)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Substring(0, 6) == "Player" && other.tag != parent.tag)
        {
            rbody = other.GetComponent<Rigidbody2D>();
            var xForce =  rbody.transform.position.x - parent.transform.position.x;
            var yForce =  rbody.transform.position.y - parent.transform.position.y;
            var totalForce = Mathf.Abs(xForce) + Mathf.Abs(yForce);
            xForce /= totalForce;
            yForce /= totalForce;
            xForce *= pushForce;
            yForce *= pushForce;
            if (yForce > 0)
                yForce *= 1.25f;
            rbody.velocity += new Vector2(xForce, yForce);
            //GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().hitstopTime = 0.2f;
            //Time.timeScale = 0;
        }
    }
}
