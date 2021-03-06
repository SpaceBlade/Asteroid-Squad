﻿using UnityEngine;
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
	public int squadId;

	public bool IsActiveSquaddie = false;
	public GameObject[] ActiveEffects;
	public GameObject ActiveGun;

	public BattleTurnSystem battleTurnManager;
	private short TargetsInRangeCount = 0;
	private Dictionary<string, SquadMemberTurn>TargetsInRange = new Dictionary<string, SquadMemberTurn> ();

	// Use this for initialization
	void Start () {
		canMove = true;
		canRotate = true;
		canShoot = true;
		squadId = 0;
	}

	void Awake(){
		playerStats = GetComponent<SquadStats> ();
		var turnmgr = GameObject.FindGameObjectWithTag ("TurnManager");
		battleTurnManager = turnmgr.GetComponent<BattleTurnSystem> ();
		SquadTextBox = GetComponentInChildren<Text> ();
		remainingTime = playerStats.ActionTime;
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

		SquadTextBox.text = string.Format ("{2}\n{0:n2} seconds\n {1:n} Health", remainingTime, playerStats.Health, name);
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
		if (battleTurnManager.GetActiveTurnMode () == BattleTurnSystem.TurnMode.TurnActions) {
			// Add objects to attack list
			if(!TargetsInRange.ContainsKey(other.name))
			{
				if(other.gameObject.CompareTag("SquadMate")){
					Debug.Log ("Player in attack range" + other.name);
					TargetsInRange.Add(other.name, other.GetComponent<SquadMemberTurn>());
				}
			}
		}
	}

	// Reset fire effects
	private void DisableShootingEffects()
	{
		ActiveGun.GetComponentInChildren<Light> ().enabled = false;
		ActiveGun.GetComponentInChildren<BoxCollider> ().enabled = false;
	}



	// Shoot primary weapon
	public void FireWeapon()
	{
		ActiveGun.GetComponentInChildren<Light> ().enabled = true;
		ActiveGun.GetComponentInChildren<BoxCollider> ().enabled = true;

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
		Debug.Log (string.Format( "Executing turn actions: {0}", name));
		// loop through objects in collider
		foreach (KeyValuePair<string, SquadMemberTurn> kvp in TargetsInRange) {
			Debug.Log (string.Format( "Applying damage to {0}\n", kvp.Value.ToString()));
			kvp.Value.playerStats.ApplyDamage(playerStats.Attack, playerStats.Precision);
            
            // Kill character
            if (!kvp.Value.playerStats.IsAlive)
            {
                StartCoroutine(kvp.Value.Death());
            }
            else
            {
                // Target is alive
            }
		}
	} // ExecuteturnActions

    // Death Animation
    public IEnumerator Death()
    {
        this.transform.Rotate(-75, 0, 0);
        yield return new WaitForSeconds(1.5f);
           
    }
}
