using UnityEngine;
using System.Collections;
using Steerings;

public class KBDRotate : MonoBehaviour {

	public float angularSpeed = 90;

	private KinematicState ks;

	// Use this for initialization
	void Start () {
		// if there's a kinematic state attached, get it
		ks = GetComponent<KinematicState>();
	}

	// Update is called once per frame
	void Update () {


		// RIGHT ARROW (KEY) => rotate clockwise (add)
		// LEFT ARROW (KEY) => rotate anticlockwise (substract)


		float move = Input.GetAxis("Horizontal");

		float orientation = transform.eulerAngles.z;
		orientation = orientation - move * angularSpeed * Time.deltaTime;
		transform.rotation = Quaternion.Euler(0, 0, orientation);

		if (ks != null) {
			ks.orientation = orientation;
		}
	}
}
