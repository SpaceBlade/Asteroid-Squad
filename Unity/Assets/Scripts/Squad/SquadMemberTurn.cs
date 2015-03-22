using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SquadMemberTurn : MonoBehaviour {
	// Stats for turn management
	public Text SquadTextBox;
	public SquadStats playerStats;
	public float remainingTime;
	public bool canMove;
	public bool canRotate;
	public bool canShoot;
	public bool IsAlive;
	public bool IsActiveSquaddie;
	public GameObject[] ActiveEffects;
	public GameObject FireCollider;

	public BattleTurnSystem battleTurnManager;
	private short TargetsInRangeCount = 0;
	private Dictionary<string, GameObject>TargetsInRange = new Dictionary<string, GameObject> ();


	// Use this for initialization
	void Start () {
		remainingTime = playerStats.ActionTime;
		canMove = true;
		canRotate = true;
		canShoot = true;
	}
	
	// Update is called once per frame
	void Update () {
		// Check for available time
		if (IsActiveSquaddie) {
			remainingTime -= Time.deltaTime;
		}
		if (remainingTime < 0) {
			remainingTime = 0.0f;

			// Mark as unable to move
			canMove = false;
		}

		SquadTextBox.text = string.Format ("{0:n2} seconds", remainingTime);
	}

	// Enable movement
	public void ToggleMovement(bool enable)
	{
		canMove = enable;
		canRotate = enable;
	}
	
	// Enable shooting
	public void ToggleShooting(bool enable)
	{
		canShoot = enable;
	}
	
	// Return if player can shoot
	public bool CanShoot()
	{
		return canShoot;
	}
	
	// Return if player can move
	public bool CanMove()
	{
		return canMove;
	}

	// Return if player can rotate
	public bool CanRotate()
	{
		return canRotate;
	}
	
	// Reset player turn time
	public void ResetTurn()
	{
		// Check for any ill effects
		float timeMod = 0;
		ToggleMovement (true);
		ToggleShooting (true);

		// Clear targets
		TargetsInRangeCount = 0;
		TargetsInRange.Clear ();

		// Update time
		remainingTime = playerStats.ActionTime - timeMod;

		DisableShootingEffects ();
	}

	// Track number of players in range
	public void OnTriggerEnter(Collider other)
	{
		TargetsInRangeCount++;
	}
	public void OnTriggerExit(Collider other)
	{
		TargetsInRangeCount--;
		if (TargetsInRangeCount < 0) {
			TargetsInRangeCount = 0;
		}
	}

	// Check if attack hit anything
	public void OnTriggerStay(Collider other)
	{
		// Only track damage at turn end
		if (battleTurnManager.GetActiveTurnMode () == BattleTurnSystem.TurnMode.EndTurn) {
			// Add objects to attack list
			if(!TargetsInRange.ContainsKey(other.name))
			{
				Debug.Log ("Player in attack range" + other.name);
				TargetsInRange.Add(other.name, other.gameObject);
			}
		}
	}

	// Reset fire effects
	private void DisableShootingEffects()
	{
		FireCollider.GetComponent<Light> ().enabled = false;
		FireCollider.GetComponent<BoxCollider> ().enabled = false;
	}



	// Shoot primary weapon
	public void FireWeapon()
	{
		FireCollider.GetComponent<Light> ().enabled = true;
		FireCollider.GetComponent<BoxCollider> ().enabled = true;

		canMove = false;
	}

	// Cancel shoot of primary weapont
	public void CancelFireWeapon()
	{
		ToggleMovement (true);
	}

	// Fire Overload ability
	public void FireOverload()
	{
	}

	// Cancel use of Overload ability
	public void CancelFireOverload()
	{
		ToggleMovement (true);
	}

	// Execute turn actions
	public void ExecuteTurnActions()
	{
	}
}
