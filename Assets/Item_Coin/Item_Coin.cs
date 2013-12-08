using UnityEngine;
using System.Collections;

public class Item_Coin : MonoBehaviour
{
	public GameObject gainEffectPrefab;
	
	float checkBehindTimer = 5.0f;
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		checkDeleteSelf();
	}
	
	void checkDeleteSelf ()
	{
		checkBehindTimer -= Time.deltaTime;
		if (checkBehindTimer < 0) {
			checkBehindTimer = 5.0f;
			GameObject runner = GameObject.FindWithTag ("Player");
			if (runner.transform.position.z > transform.position.z + 100.0f) {
				Destroy (gameObject);
			}
		}
	}
	
	void OnTriggerEnter (Collider other)
	{
		//Debug.Log (string.Format("coin trigger {0}",other.tag ));
		if (other.tag == "PlayerController") {
			//other.gameObject.SendMessage("OnGetCoin");
			GameObject player = (GameObject)GameObject.FindWithTag ("Player");

            int coinCount = 0;
            if (gameObject.tag == "Item_Coin") {
                coinCount = 1;
            }
            else if (gameObject.tag == "Item_Bill_1000") {
                coinCount = 20;
            }
            else if (gameObject.tag == "Item_Bill_5000") {
                coinCount = 30;
            }
            else if (gameObject.tag == "Item_Bill_10000") {
                coinCount = 75;
            }
            else {
                Debug.Log (string.Format("Coin/Bill tag Error!!!"));
            }

            player.SendMessage ("OnGetCoin", coinCount);
			
			Instantiate (gainEffectPrefab, transform.position, transform.rotation);
			Destroy (gameObject);
		}
	}
	
	void OnCollisionEnter (Collision collision)
	{
		//Debug.Log (string.Format("coin collider tag={0}", collision.gameObject.tag));
	}

	void destroy()
	{
		Destroy(gameObject);
	}
}
