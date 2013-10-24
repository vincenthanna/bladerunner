using UnityEngine;
using System.Collections;

public class GoalLine : MonoBehaviour
{
	public GUISkin skin;
	bool finished = false;

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
		}

	}
	
	void OnGUI ()
	{
		if (!finished)
			return;
		int sw = Screen.width;
		int sh = Screen.height;
		GUI.skin = skin;
		GUI.Label (new Rect (sw / 2, sh / 2, sw, sh), "Game End!", "Message");
	}
}
