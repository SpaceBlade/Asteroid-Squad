using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {
	public SquadMemberTurn playerTurn;
	public NavMeshAgent agent;
	public Transform target;
    public FindTarget targetFinder;

	// Use this for initialization
	void Start () {
		playerTurn = Component.FindObjectOfType<SquadMemberTurn> ();
        targetFinder = Component.FindObjectOfType<FindTarget>();
	}
	
	// Update is called once per frame
	void Update () {
        if (targetFinder != null)
        {
            if (playerTurn.canMove)
            {
                //agent.SetDestination(targetFinder.currentTarget.rigidbody.transform.position);
            }
        }
	}
}
