using UnityEngine;
using System.Collections;

public class PlayerInputScript : MonoBehaviour {
	public float PlayerSpeed;
	public float JumpForce;
	public GameObject targetPlayer;

	public BattleTurnSystem BattleTurn;

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {

	}

	// Physics
	void FixedUpdate()
	{
		if (targetPlayer == null) {
			return;
		}
		float moveX = Input.GetAxis ("Horizontal");
		float moveY = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3 (moveX, 0.0f, moveY) * PlayerSpeed * Time.deltaTime;

		// Jump
		if(Input.GetKey(KeyCode.Return))
	    {
			targetPlayer.rigidbody.AddForce (Vector3.up * JumpForce * Time.deltaTime);
		}

		// Switch player turn
		if (Input.GetKeyDown (KeyCode.Tab)) {
			BattleTurn.SwitchActivePlayer();
		}

		// If player can shoot
		if (targetPlayer.GetComponent<SquadStats> ().CanShoot ()) {
			// Fire
			if (Input.GetKey (KeyCode.Space)) {
				Debug.Log ("Player: " + targetPlayer.name + " Fire!");
				GameObject projectile = GameObject.Find ("CubeProjectile");
				projectile.GetComponent<MeshRenderer> ().enabled = true;

				projectile.GetComponent<Rigidbody> ().AddForce (targetPlayer.GetComponent<Rigidbody> ().velocity * 5.0f);
			}
		}

		// If player can move
		if (targetPlayer.GetComponent<SquadStats> ().CanMove ()) {
			// rigidbody.AddForce (movement * PlayerSpeed * Time.deltaTime);
			targetPlayer.transform.position += movement;
		}
	}
}
