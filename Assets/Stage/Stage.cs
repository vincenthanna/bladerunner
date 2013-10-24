using UnityEngine;
using System.Collections;

public class Stage : MonoBehaviour
{
	public GameObject structBlockType1;
	public GameObject structBlockType2;
	public GameObject structBlockType3;
	public GameObject structBlockType4;
	public GameObject structBlockType5;
	float checkBehindTimer = 5.0f;

	// Use this for initialization
	void Start ()
	{
		Debug.Log ("Stage Created!");
		generateInside ();
			
	}
	
	// Update is called once per frame
	void Update ()
	{
		checkDeleteSelf ();	
	}
	
	void checkDeleteSelf ()
	{
		checkBehindTimer -= Time.deltaTime;
		if (checkBehindTimer < 0) {
			checkBehindTimer = 5.0f;
			GameObject runner = GameObject.FindWithTag ("Player");
			if (runner.transform.position.z > transform.position.z + 600.0f) {
				Destroy (gameObject);
			}
		}
	}
	
	void generateInside ()
	{
		float position = 0.0f;
		GameObject prefab = structBlockType1;
		while (position < 400.0f) {
			int randomValue = (Random.Range (0, 9) % 5);
			 
			Vector3 v = new Vector3 (0, 0.6f, position);
			switch (randomValue) {
			case 0: {
					prefab = structBlockType1;
				}
				break;
			case 1: {
					prefab = structBlockType2;
				}	
				break;
			case 2: {
					prefab = structBlockType3;
				}
				break;
			case 3: {
					prefab = structBlockType4;
					v = new Vector3 (5, 0.6f, position);
				}
				break;
			case 4: {
					prefab = structBlockType5;
					v = new Vector3 (-5, 0.6f, position);
				}
				break;
			}
			
			position += Random.Range (30.0f, 60.0f);
			//Vector3 v = new Vector3 (0, 0.6f, position);
			v += gameObject.transform.position;
			GameObject obj = (GameObject)Instantiate (prefab, v, Quaternion.identity);
			
			obj.transform.parent = gameObject.transform; // add to stage
		}
	}
}
