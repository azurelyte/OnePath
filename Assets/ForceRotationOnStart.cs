using UnityEngine;
using System.Collections;

public class ForceRotationOnStart : MonoBehaviour {
	public float XAmount = 90f;

	// Use this for initialization
	void Start () {
		transform.Rotate (transform.eulerAngles + new Vector3 (XAmount, 0, 0));
	}
}
