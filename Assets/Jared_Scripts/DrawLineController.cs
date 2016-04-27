using UnityEngine;
using System.Collections;

public class DrawLineController : MonoBehaviour {

	public Transform EndPoint = null;
	public float DrawSpeed = 1;
	public Material LineRendererMaterial = null;
	public DrawState InitialMode = DrawState.UnDraw;

	private Vector3 StartPoint, TravelPoint;
	private DrawState CurrentState;
	private LineRenderer Line; //The actual renderer for the line

	public enum DrawState {Draw_animated, Draw, UnDraw_animated, UnDraw, Idle}

	// Use this for initialization
	void Start () {
		CurrentState = InitialMode;
		Line = gameObject.AddComponent<LineRenderer> (); //Create LineRenderer component and get reference
		StartPoint = this.transform.position; //Get StartPoint
		TravelPoint = StartPoint;
		Line.SetPosition (0, StartPoint); //Set Line Start point
		Line.SetWidth (.25f, .25f);
		if (LineRendererMaterial != null) Line.material = LineRendererMaterial; //Set line material if one given
	}
	
	// Update is called once per frame
	void Update () {
		//Based on the current state, 
		switch (CurrentState) {
		case DrawState.UnDraw:
			Line.SetPosition (1, StartPoint);
			CurrentState = DrawState.Idle;
			break;
		case DrawState.UnDraw_animated:
			TravelPoint = Vector3.Lerp (TravelPoint, StartPoint, DrawSpeed * Time.deltaTime);
			Line.SetPosition (1, TravelPoint);
			if (Vector3.Distance (TravelPoint, StartPoint) < 0.05f) CurrentState = DrawState.UnDraw;
			break;
		case DrawState.Draw:
			Line.SetPosition (1, EndPoint.position);
			CurrentState = DrawState.Idle;
			break;
		case DrawState.Draw_animated:
			TravelPoint = Vector3.Lerp (TravelPoint, EndPoint.position, DrawSpeed * Time.deltaTime);
			Line.SetPosition (1, TravelPoint);
			if (Vector3.Distance(TravelPoint, EndPoint.position) < 0.05f) CurrentState = DrawState.Draw;
			break;
		case DrawState.Idle:
			//Line.SetPosition (1, EndPoint.position); //Enabling this line will make sure that the line ALWAYS updates
			break;
		}

	}

	public bool DrawLine()
	{
		///<summary>Draw a line between this object and the target object, true is successful</summary>
		CurrentState = DrawState.Draw;
		return true;
	}

	public bool DrawLine_Animated()
	{
		///<summary>Draw an animated line between this object and the target object, true is successful</summary>
		CurrentState = DrawState.Draw_animated;
		return true;
	}

	public bool UnDrawLine()
	{
		///<summary>Remove the line between this object and its target if it exists, true is successful</summary>
		CurrentState = DrawState.UnDraw;
		return true;
	}

	public bool UnDrawLine_Animated()
	{
		///<summary>ANIMATED: Remove the line between this object and its target if it exists, true is successful</summary>
		CurrentState = DrawState.UnDraw_animated;
		return true;
	}
}
