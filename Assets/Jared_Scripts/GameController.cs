using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject CamTarget;
	public float XMov = 3, YMov = 6;
	public float CameraMoveSpeed = 1;
	public UnityEngine.UI.Text ScoreText;

	private int Score = 0, BestScore = 0;
	private Vector3 CameraStartLocation;
	private Vector3 NextCameraLocation;
	private NodeManager manager = null;

	// Use this for initialization
	void Start () {
		CamTarget = GameObject.FindGameObjectWithTag ("MainCamera");
		if (manager == null)
			manager = gameObject.GetComponent<NodeManager> ();
		CameraStartLocation = CamTarget.transform.position;
		NextCameraLocation = CameraStartLocation;

		//Load Best score
		try{
			System.IO.StreamReader sr = new System.IO.StreamReader (Application.dataPath + "/score.dat");
			int.TryParse(sr.ReadLine(), out BestScore);
			sr.Close ();
			UpdateScore();
		}
		catch (System.Exception e) {

		}
	}
	
	// Update is called once per frame
	void Update () {
		//Ceck for inputs
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			AdvanceGame(manager.PickLeftNode(), new Vector3(-XMov,YMov,0));
		else if (Input.GetKeyDown(KeyCode.RightArrow))
			AdvanceGame(manager.PickRightNode(), new Vector3(XMov,YMov,0));
		//Move camera to next target
		CamTarget.transform.position = Vector3.Lerp (CamTarget.transform.position, NextCameraLocation, CameraMoveSpeed * Time.deltaTime); 
	}

	private void AdvanceGame(bool LastMoveWasSuccessfull, Vector3 NextLoc)
	{
		//If successfull
		if (LastMoveWasSuccessfull) {
			NextCameraLocation += NextLoc;
			Score++;
			UpdateScore ();
			return;
		}
		//Reset game
		Reset();
		return;
	}

	public void RightMove()
	{
		AdvanceGame(manager.PickRightNode(), new Vector3(XMov,YMov,0));
		CamTarget.transform.position = Vector3.Lerp (CamTarget.transform.position, NextCameraLocation, CameraMoveSpeed * Time.deltaTime); 
	}

	public void LeftMove()
	{
		AdvanceGame(manager.PickLeftNode(), new Vector3(-XMov,YMov,0));
		CamTarget.transform.position = Vector3.Lerp (CamTarget.transform.position, NextCameraLocation, CameraMoveSpeed * Time.deltaTime); 
	}

	public void Reset()
	{
		Score = 0;
		UpdateScore ();
		NextCameraLocation = CameraStartLocation;
	}

	private void UpdateScore ()
	{
		if (Score > BestScore) {
			BestScore = Score;
			ScoreText.text = "BestScore: " + Score + "\nScore: " + Score;
			try{
				System.IO.StreamWriter sw = new System.IO.StreamWriter (Application.dataPath + "/score.dat");
				sw.WriteLine (BestScore);
				sw.Close ();
			}
			catch (System.Exception e) {

			}
			return;
		}
		ScoreText.text ="BestScore: " + BestScore + "\nScore: " + Score;
	}
}
