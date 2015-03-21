using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SquadMemberTurn : MonoBehaviour {
	// Stats for turn management
	public Text SquadTextBox;
	public SquadStats playerStats;
	public float remainingTime;
	public bool canMove;
	public bool canShoot;
	public bool IsAlive;
	public bool IsActiveSquaddie;
	public GameObject[] ActiveEffects;


	// Use this for initialization
	void Start () {
		remainingTime = playerStats.ActionTime;
		canMove = true;
		canShoot = true;
	}
	
	// Update is called once per frame
	void Update () {
		// Check for available time
		if (IsActiveSquaddie) {
			remainingTime -= Time.deltaTime;
		}
		if (playerStats.ActionTime < 0) {
			playerStats.ActionTime = 0.0f;

			// Mark as unable to move
			canMove = false;
		}

		SquadTextBox.text = string.Format ("{0:n2} seconds", playerStats.ActionTime);
	}

	// Enable movement
	public void ToggleMovement(bool enable)
	{
		canMove = enable;
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

	// Reset player turn time
	public void ResetTurnTime()
	{
		// Check for any ill effects
		float timeMod = 0;

		// Update time
		remainingTime = playerStats.ActionTime - timeMod;
	}
}
