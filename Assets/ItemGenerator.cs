using UnityEngine;
using System.Collections;

public class ItemGenerator : MonoBehaviour
{
    public GameObject itemCoinPrefab;
    Vector3 lastCreatedCoinPos;
    Vector3 lastCreatedBillPos;
    bool tendencyLeft = false;
    float tendencyLeftTimer = 2.0f;
    float tendencyLeftTimerDurationMax = 2.0f;
    public GameObject itemBill_1000_Prefab;
    public GameObject itemBill_5000_Prefab;
    public GameObject itemBill_10000_Prefab;
    
    // Use this for initialization
    void Start ()
    {
        lastCreatedCoinPos = new Vector3 (0, 0, 0);
        lastCreatedBillPos = new Vector3 (0, 0, 0);
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
            tendencyLeft = (Random.Range (-5.0f, 5.0f) > 0.0f) ? true : false;
            tendencyLeftTimer = Random.Range (1.2f, tendencyLeftTimerDurationMax);
            //Debug.Log("Direction Changed!");
        }
        
        //if (pos.z + 30.0f < 400.0f) { //check position is in-stage
        Vector3 itemPos = new Vector3 (lastCreatedCoinPos.x, 1.2f, pos.z + 30.0f);
            
        if (tendencyLeft) {
            itemPos.x += Random.Range (-1.5f, -0.5f);
        } else {
            itemPos.x += Random.Range (0.5f, 1.5f);
        }
            
        if (itemPos.x < -8.0f) {
            tendencyLeft = !tendencyLeft;
            itemPos.x = -8.0f;
        }
        if (itemPos.x > 8.0f) {
            tendencyLeft = !tendencyLeft;
            itemPos.x = 8.0f;
        }

        itemPos.y = 1.4f;
        //Debug.Log(string.Format("itemPos.x={0}", itemPos.x));
            
        GameObject prefab = itemCoinPrefab;
        //prefab = itemBill_10000_Prefab;
            
        //Debug.Log (string.Format ("itemPos = ({0}, {1}, {2})", itemPos.x, itemPos.y, itemPos.z));
        if (itemPos.z - lastCreatedCoinPos.z > 5.0f) {
            GameObject item = (GameObject)Instantiate (prefab, itemPos, Quaternion.identity);               
            item.transform.parent = gameObject.transform;
            lastCreatedCoinPos = itemPos;
        }

        if (BRGameStatus.Instance ().LevelNumber >= 4) {
            float dropPosition;
            dropPosition = 60.0f - (BRGameStatus.Instance ().LevelNumber * 1.2f);
            dropPosition += (BRGameStatus.Instance ().LevelNumber % 5) * 3.0f;
            dropPosition *= (pos.z < 400.0f ? 0.82f : 1.0f);
            itemPos.x = Random.Range(-8.5f , 8.5f);

            if (itemPos.z - lastCreatedBillPos.z > dropPosition) {
                if (Random.Range (0, 2) > 0) {

                    int type = Random.Range (0, 11);
                    if (type < 7) {
                        prefab = itemBill_1000_Prefab;
                    } else if (type < 10) {
                        prefab = itemBill_5000_Prefab;
                    } else {
                        prefab = itemBill_10000_Prefab;
                    }

                    GameObject item = (GameObject)Instantiate (prefab, itemPos, Quaternion.identity);
                    item.transform.parent = gameObject.transform;
                    lastCreatedBillPos = itemPos;
                }
            }
        }
    }

    void Reset ()
    {
        lastCreatedCoinPos = new Vector3 (0, 0, 0);
        lastCreatedBillPos = new Vector3 (0, 0, 0);
        tendencyLeftTimer = 2.0f;
        tendencyLeftTimerDurationMax = 2.0f;
        BroadcastMessage ("destroy");
    }
}
