using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SquadMemberTurn : MonoBehaviour {
	public Text SquadTextBox;
	public SquadStats playerStats;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		// Check for available time
		playerStats.ActionTime -= Time.deltaTime;
		if (playerStats.ActionTime < 0) {
			playerStats.ActionTime = 0.0f;

			// Mark as unable to move
			playerStats.ToggleMovement(false);
		}

		SquadTextBox.text = string.Format ("{0:n2} seconds", playerStats.ActionTime);
	}
}
