using UnityEngine;
using System.Collections;

// Class for managing squad member stats
public class SquadStats : MonoBehaviour {
	// Stats for each squad member
	public float ActionTime;
	public ushort Health;
	public ushort Mana;
	public float Precision;
	public float Reaction;
	public ushort Attack;
	public ushort Defense;
	public ushort Luck;
	public ushort Level;

	// Stats for turn management
	private float remainingTime;
	private bool canMove;
	private bool canShoot;

	// Use this for initialization
	void Start () {
		RandomStats ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// Initialize values to RandomStats
	public void RandomStats()
	{
		ActionTime = Random.Range (10, 20);	// action time in seconds

		Health = 100;		// default health
		Mana = 50;			// default mana
		Precision = Random.Range (0.5f, 0.7f);	// default presicion 50-70% accuracy
		Reaction = Random.Range (0.5f, 0.7f);	// default reaction 50-70% chance
		Attack = 10;		// Attack HP damage
		Defense = 5;		// HP damage protection
		Luck = 2;			// Luck modifier
		Level = 1;			// Player level

		// Turn stats
		remainingTime = ActionTime;	// remaining time in turn
		canMove = true;
		canShoot = true;
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
}
