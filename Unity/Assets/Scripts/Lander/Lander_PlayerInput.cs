﻿using UnityEngine;
using System.Collections;

public class Lander_PlayerInput : MonoBehaviour {
        // Public objects
    public float PlayerSpeed;
    public float JumpForce;

    public SquadMemberTurn SquaddieTurn;
    
    // Private objects
    public GameObject LanderShip;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * moveY * PlayerSpeed * Time.deltaTime;
        Vector3 rotation = new Vector3(0.0f, moveX, 0.0f) * Mathf.PI * 20 * Time.deltaTime;

        // Jump
        if (Input.GetKey(KeyCode.Return))
        {
            // SquaddieTurn.rigidbody.. .AddForce(Vector3.up * JumpForce * Time.deltaTime);
            // targetPlayer.rigidbody.AddForce (Vector3.up * JumpForce * Time.deltaTime);
        }



        // If player can shoot
        if (SquaddieTurn.CanShoot())
        {
            // Fire
            if (Input.GetKey(KeyCode.Space))
            {
                Debug.Log("Player: " + name + " Fire!");
                //  GameObject projectile = GameObject.Find ("CubeProjectile");
                //  projectile.GetComponent<MeshRenderer> ().enabled = true;

                // projectile.GetComponent<Rigidbody> ().AddForce (targetPlayer.GetComponent<Rigidbody> ().velocity * 5.0f);

                // Enable FireRange objects

                SquaddieTurn.FireWeapon();

                // FireLight.enabled = true;
                // FireCollider.GetComponent<BoxCollider>().enabled=true;
            }
        }

        // If player can Move/Rotate
        if (SquaddieTurn.CanRotate())
        {
            transform.Rotate(rotation);
        }
        if (SquaddieTurn.CanMove())
        {
            transform.position += movement;
        }
    }

    // Physics
    void FixedUpdate()
    {
    }



}
