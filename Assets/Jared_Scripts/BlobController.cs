using UnityEngine;
using System.Collections;

public class BlobController : MonoBehaviour {

	public DrawState InitialState = DrawState.Grow;
	public float MaxRadius = 0.35f;
	public float SizeChangeRate = 0.1f;

	private DrawState CurrentState;
	private float radius = 0f;

	public enum DrawState {Grow, Shrink, Idle}

	// Use this for initialization
	void Start () {
		CurrentState = InitialState;
	}
	
	// Update is called once per frame
	void Update () {
		switch (CurrentState) {
		case DrawState.Idle:
			break;
		case DrawState.Grow:
			radius += Time.deltaTime * SizeChangeRate;
			if (radius > MaxRadius) {
				radius = MaxRadius;
				//Gizmos.DrawSphere (transform.position, radius);
				CurrentState = DrawState.Idle;
				return;
			}
			//Gizmos.DrawSphere (transform.position, radius);
			break;
		case DrawState.Shrink:
			radius -= Time.deltaTime * SizeChangeRate;
			if (radius < 0.0001f) {
				radius = 0;
				//Gizmos.DrawSphere (transform.position, radius);
				CurrentState = DrawState.Idle;
				return;
			}
			//Gizmos.DrawSphere (transform.position, radius);
			break;
		}
	}

	void OnDrawGizmosSelected() {
		Gizmos.color = Color.black;
		Gizmos.DrawSphere (transform.position, radius);
	}

	public void Grow()
	{
		CurrentState = DrawState.Grow;
	}

	public void Shrink()
	{
		CurrentState = DrawState.Shrink;
	}
	public void OnDestroy()
	{
		
	}
}
