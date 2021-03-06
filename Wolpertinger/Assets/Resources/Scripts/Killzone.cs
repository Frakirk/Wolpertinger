﻿using UnityEngine;
using System.Collections;

public class Killzone : MonoBehaviour {

    //Transform[] deadPlayers;
    int numPlayersDead;
    int numPlayersTotal;
    Transform cameraTransform;
    float startTime, currentTime;
    public float suddenDeathTime, RisingSpeed, victoryTimescale;
    public AudioClip deathSqueal;
    AudioSource audioSource;

    void Start()
    {
        cameraTransform = GameObject.Find("Game Camera").GetComponent<Transform>();
        //deadPlayers = new Transform[4];
        numPlayersDead = 0;
        var playerBoolArray = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().players;
        numPlayersTotal = 0;
        for (int i = 0; i < 4; i++)
        {
            if (playerBoolArray[i])
                numPlayersTotal++;
        }
        startTime = Time.time;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (numPlayersDead == numPlayersTotal - 1)
            Time.timeScale = victoryTimescale;
        currentTime = Time.time - startTime;
        if (currentTime > suddenDeathTime && Time.timeScale == 1)
            transform.position += new Vector3(0,RisingSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Substring(0, 6) == "Player")
        {
            other.gameObject.GetComponent<Renderer>().enabled = false; // other.transform.position += new Vector3(0, 20);
            numPlayersDead++;
            other.gameObject.transform.SetParent(cameraTransform);
            other.gameObject.transform.position = cameraTransform.position;
            Destroy(other.gameObject.GetComponent<Rigidbody2D>());
            Destroy(other.gameObject.GetComponent<CircleCollider2D>());
            other.gameObject.GetComponent<GamepadInput>().enabled = false;
            audioSource.PlayOneShot(deathSqueal, 0.75f);
            //deadPlayers[numPlayersDead] = other.gameObject.GetComponent<Transform>();
            //Debug.Log("NEW DEAD GUY: " + deadPlayers[numPlayersDead]);
        }
    }
}
