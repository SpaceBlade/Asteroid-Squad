using UnityEngine;
using System.Collections;

public class TurnStats : MonoBehaviour {
	// Stats for turn management
	public float remainingTime;
	public bool canMove;
	public bool canShoot;
	public bool IsAlive;
	public SquadStats PlayerSquaddieStats;


	// Use this for initialization
	void Start () {

		// Turn stats
		remainingTime = PlayerSquaddieStats.ActionTime;	// remaining time in turn
		canMove = true;
		canShoot = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
