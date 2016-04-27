using UnityEngine;
using System.Collections;

public class NodeManager : MonoBehaviour {
	
	public float XPlacement = 1, YPlacement = 2;

	private int GameSeed;
	private GameObject FirstNode, NodePrefab;
	private NodeReference CurrentNode;

	// Use this for initialization
	void Start () {
		NodePrefab = Resources.Load<GameObject> ("Node");
		CurrentNode = new NodeReference (null, GameObject.Find ("StartNode"));
		FirstNode = CurrentNode.Reference;
		GenerateNodeSet ();
		GameSeed = Random.Range (0, 999999);
		Random.InitState (GameSeed);
		Reset ();
	}

	public void GenerateNodeSet()
	{
		if (Random.Range (0, 2) > 0) {
			//Left is correct
			CurrentNode.NextNodeLeft = (GameObject)GameObject.Instantiate(NodePrefab, CurrentNode.Reference.transform.position, Quaternion.identity);
			CurrentNode.NextNodeLeft.transform.position += new Vector3 (-XPlacement, YPlacement, 0);
			CurrentNode.NextNodeRight = (GameObject)GameObject.Instantiate(NodePrefab, CurrentNode.Reference.transform.position, Quaternion.identity);
			CurrentNode.NextNodeRight.transform.position += new Vector3 (XPlacement, YPlacement, 0);
			CurrentNode.NextNodeLeft.GetComponent<NodeData> ().isCorrectNode = true;
		} else {
			//right is correct
			CurrentNode.NextNodeLeft = (GameObject)GameObject.Instantiate(NodePrefab, CurrentNode.Reference.transform.position, Quaternion.identity);
			CurrentNode.NextNodeLeft.transform.position += new Vector3 (-XPlacement, YPlacement, 0);
			CurrentNode.NextNodeRight = (GameObject)GameObject.Instantiate(NodePrefab, CurrentNode.Reference.transform.position, Quaternion.identity);
			CurrentNode.NextNodeRight.transform.position += new Vector3 (XPlacement, YPlacement, 0);
			CurrentNode.NextNodeRight.GetComponent<NodeData> ().isCorrectNode = true;
		}
	}

	public bool PickLeftNode()
	{
		///<summary>Returns true if correct node is picked, otherwise false</summary>
		if (CurrentNode.NextNodeLeft.GetComponent<NodeData> ().isCorrectNode) {
			CurrentNode.Reference.GetComponent<DrawLineController> ().EndPoint = CurrentNode.NextNodeLeft.transform;
			CurrentNode.Reference.GetComponent<DrawLineController> ().DrawLine_Animated ();
			CurrentNode = new NodeReference (CurrentNode.Reference, CurrentNode.NextNodeLeft);
			GenerateNodeSet ();
			return true;
		}
		Reset ();
		return false;
	}

	public bool PickRightNode()
	{
		///<summary>Returns true if correct node is picked, otherwise false</summary>
		if (CurrentNode.NextNodeRight.GetComponent<NodeData> ().isCorrectNode) {
			CurrentNode.Reference.GetComponent<DrawLineController> ().EndPoint = CurrentNode.NextNodeRight.transform;
			CurrentNode.Reference.GetComponent<DrawLineController> ().DrawLine_Animated ();
			CurrentNode = new NodeReference (CurrentNode.Reference, CurrentNode.NextNodeRight);
			GenerateNodeSet ();
			return true;
		}
		Reset ();
		return false;
	}

	private void Reset()
	{
		CurrentNode = new NodeReference (null, GameObject.Find ("StartNode")); //Reset current node
		//Delete old nodes
		foreach (GameObject node in GameObject.FindGameObjectsWithTag("Node"))
		{
			Destroy (node,3);
			node.GetComponent<DrawLineController> ().UnDrawLine_Animated ();
		}
		//ReCreate first scene
		Random.InitState (GameSeed);
		GenerateNodeSet();
		CurrentNode.Reference.GetComponent<DrawLineController> ().UnDrawLine_Animated ();
	}

	public void ReSeed()
	{
		GameSeed = Random.Range (0, 999999);
		Random.InitState (GameSeed);
		Reset ();
		gameObject.GetComponent<GameController> ().Reset ();
	}

	protected class NodeReference
	{
		public GameObject PreviousNode, NextNodeLeft, NextNodeRight, Reference;

		public NodeReference(GameObject preNode, GameObject REF)
		{
			PreviousNode = preNode;
			Reference = REF;
			NextNodeLeft = null;
			NextNodeRight =  null;
		}
	}
}
