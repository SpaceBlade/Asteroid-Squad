using UnityEngine;
using System.Collections;

public class PlayerInputScript : MonoBehaviour {
	// Public objects
	public float PlayerSpeed;
	public float JumpForce;
	// public GameObject targetPlayer;

	// Timer for player actions


	public BattleTurnSystem BattleTurn;
	public SquadMemberTurn SquaddieTurn;



	// Private objects

	// Use this for initialization
	void Start () {
			
	}

	void Awake()
	{
		SquaddieTurn = GetComponent<SquadMemberTurn> ();
	}
	
	// Update is called once per frame
	void Update () {
		//if (targetPlayer == null) {
		//	return;
		//}
		float moveX = Input.GetAxis ("Horizontal");
		float moveY = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3 (moveX, 0.0f, moveY) * PlayerSpeed * Time.deltaTime;
		
		// Jump
		if(Input.GetKey(KeyCode.Return))
		{
			// targetPlayer.rigidbody.AddForce (Vector3.up * JumpForce * Time.deltaTime);
		}
		

		
		// If player can shoot
		if ( SquaddieTurn.CanShoot ()) {
			// Fire
			if (Input.GetKey (KeyCode.Space)) {
				Debug.Log ("Player: " + name + " Fire!");
				//  GameObject projectile = GameObject.Find ("CubeProjectile");
				//  projectile.GetComponent<MeshRenderer> ().enabled = true;
				
				// projectile.GetComponent<Rigidbody> ().AddForce (targetPlayer.GetComponent<Rigidbody> ().velocity * 5.0f);
				
				// Enable FireRange objects

				SquaddieTurn.FireWeapon();
				
				// FireLight.enabled = true;
				// FireCollider.GetComponent<BoxCollider>().enabled=true;
			}
		}
		
		// If player can move
		if (SquaddieTurn.CanMove ()) {
			// rigidbody.AddForce (movement * PlayerSpeed * Time.deltaTime);
			transform.position += movement;
		}
	}

	// Physics
	void FixedUpdate()
	{
	}



}
