using UnityEngine;
using System.Collections;

public class ItemGenerator : MonoBehaviour
{
	public GameObject itemCoinPrefab;
	Vector3 lastCreatedItemPos;
	
	bool tendencyLeft = false;
	float tendencyLeftTimer = 2.0f;
	float tendencyLeftTimerDurationMax = 2.0f;
	
	// Use this for initialization
	void Start ()
	{
		lastCreatedItemPos = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update ()
	{
		tendencyLeftTimer -= Time.deltaTime;
			
		GameObject player = (GameObject)GameObject.FindWithTag ("Player");
		if (player == null) {
			Debug.Log ("Error 1");
		}
		
		CharacterController controller = (CharacterController)player.GetComponent ("CharacterController");
		if (controller == null) {
			Debug.Log ("Error 2");
		}
		
		Vector3 pos = player.transform.position;
		//Debug.Log (string.Format ("({0}, {1}, {2})", pos.x, pos.y, pos.z));
		
		// make item ahead of this
		if (tendencyLeftTimer < 0.0f) {
			tendencyLeft = (Random.Range(-5.0f, 5.0f) > 0.0f) ? true : false;
			tendencyLeftTimer = Random.Range(1.2f, tendencyLeftTimerDurationMax);
			Debug.Log("Direction Changed!");
		}
		
		//if (pos.z + 30.0f < 400.0f) { //check position is in-stage
			Vector3 itemPos = new Vector3 (lastCreatedItemPos.x, 1.2f, pos.z + 30.0f);
			
			if (tendencyLeft) {
				itemPos.x += Random.Range(-1.5f,-0.5f);
			}
			else {
				itemPos.x += Random.Range(0.5f,1.5f);
			}
			
			if (itemPos.x < -8.0f) {
				tendencyLeft = !tendencyLeft;
				itemPos.x = -8.0f;
			}
			if (itemPos.x > 8.0f) {
				tendencyLeft = !tendencyLeft;
				itemPos.x = 8.0f;
			}
			//Debug.Log(string.Format("itemPos.x={0}", itemPos.x));
			
			GameObject prefab = itemCoinPrefab;
			//Debug.Log (string.Format ("itemPos = ({0}, {1}, {2})", itemPos.x, itemPos.y, itemPos.z));
			if (itemPos.z - lastCreatedItemPos.z > 5.0f) {
				Instantiate (prefab, itemPos, Quaternion.identity);				
				lastCreatedItemPos = itemPos;
			}
		//}
	}
}
