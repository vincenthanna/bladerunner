using UnityEngine;
using System.Collections;

public class HalfLine : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Player") {
			/*
			//Debug.Log ("Trigger!!!!!!!");
			finished = true;
		
			GameObject player = (GameObject)GameObject.FindWithTag ("Player");
			player.SendMessage ("endGame");
		
			yield return new WaitForSeconds(3.0f);
		
			Application.LoadLevel ("Stage1");
			*/
		
			Debug.Log("Prepare Next Stage....");
			GameObject stageController = (GameObject)GameObject.FindWithTag ("StageController");
			stageController.SendMessage ("createNextStage");
		}

	}
}
