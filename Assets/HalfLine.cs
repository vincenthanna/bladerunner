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

        /*
		if (other.tag == "Player") {
			Debug.Log("Prepare Next Stage....");
			GameObject stageController = (GameObject)GameObject.FindWithTag ("StageController");
			stageController.SendMessage ("createNextStage");
		}
        */      

	}
}
