using UnityEngine;
using System.Collections;

// Class for managing squad member stats
public class SquadStats : MonoBehaviour {
	// Stats for each squad member
	public float ActionTime;
	public int Health;
	public int Mana;
	public float Precision;
	public float Reaction;
	public int Attack;
	public int Defense;
	public int Luck;
	public int Level;
	public bool IsAlive;



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
		ActionTime = Random.Range (5 , 10);	// action time in seconds

		Health = 100;		// default health
		Mana = 50;			// default mana
		Precision = Random.Range (0.5f, 0.7f);	// default presicion 50-70% accuracy
		Reaction = Random.Range (0.5f, 0.7f);	// default reaction 50-70% chance
		Attack = 10;		// Attack HP damage
		Defense = 5;		// HP damage protection
		Luck = 2;			// Luck modifier
		Level = 1;			// Player level

		IsAlive = true;		// Player is alive

	}

	// Increase stats due to XP
	public void LevelUp()
	{
	}

	// Player has been hit
	public void ApplyDamage(int attack, float precision)
	{
		Debug.Log (string.Format( "Applying damage to {0}\nHealth: {1:n}", name, Health));
		// Decrease health
		Health -= attack;
		if (Health < 0) {
			Health = 0;
		}
	}
}
