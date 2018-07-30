using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject TrackedPlayer;
	public float TrackingDistance;
	public float TrackingSpeed;
	private Vector3 offset;
    public float RotationSpeed = 1.0f;

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

        ControlCamera();
	}

	void LateUpdate()
	{
		if (TrackedPlayer != null) {
			transform.position = TrackedPlayer.transform.position + offset;
		} else {
			// Do Something
		}
	}

    void ControlCamera()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            this.gameObject.transform.Rotate(this.gameObject.transform.worldToLocalMatrix.MultiplyVector(Vector3.up), -Mathf.PI * RotationSpeed);
        }
        if (Input.GetKey(KeyCode.E))
        {
            this.gameObject.transform.Rotate(this.gameObject.transform.worldToLocalMatrix.MultiplyVector(Vector3.up), Mathf.PI * RotationSpeed);
        }
    }
}
