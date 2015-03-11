using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject TrackedPlayer;
	public float TrackingDistance;
	public float TrackingSpeed;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		offset = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		 * Vector3 dist = TrackedPlayer.transform.position - transform.position;
		if(dist.magnitude > TrackingDistance)
		{
			transform.position += dist.normalized * TrackingSpeed;
			transform.LookAt (TrackedPlayer.transform.position);
		}
		*/
	}

	void LateUpdate()
	{
		transform.position = TrackedPlayer.transform.position + offset;
	}
}
