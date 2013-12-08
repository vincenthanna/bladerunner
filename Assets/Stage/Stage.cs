using UnityEngine;
using System.Collections;

public class Stage : MonoBehaviour
{
    public GameObject structBlockType1;
    public GameObject structBlockType2;
    public GameObject structBlockType3;
    public GameObject structBlockType4;
    public GameObject structBlockType5;
    public GameObject structBlockType6;
    public GameObject structBlockType7;
    public GameObject structBlockType8;
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
        
    }

    void generateInside ()
    {
        float position = 80.0f;
        GameObject prefab = structBlockType1;
        while (position < 400.0f) {
            int randomValue = (Random.Range (0, 9) % 8);
             
            Vector3 v = new Vector3 (0, 0.6f, position);
            switch (randomValue) {
            case 0:
                {
                    prefab = structBlockType1;
                }
                break;
            case 1:
                {
                    prefab = structBlockType2;
                }   
                break;
            case 2:
                {
                    prefab = structBlockType3;
                }
                break;
            case 3:
                {
                    prefab = structBlockType4;
                    v.x = 5.0f;
                    //v = new Vector3 (5, 0.6f, position);
                }
                break;
            case 4:
                {
                    prefab = structBlockType5;
                    v.x = -5.0f;
                    //v = new Vector3 (-5, 0.6f, position);
                    
                }
                break;
            case 5:
                {
                    prefab = structBlockType6;
                    v.x += Random.Range (-3.0f, 3.0f);
                    //v = new Vector3 (-5, 0.6f, position);
                    
                }
                break;
            case 6:
                {
                    if (BRGameStatus.Instance ().LevelNumber >= 5) {
                        prefab = structBlockType7;
                    }
                    //v = new Vector3 (-5, 0.6f, position);
                }
                break;
            case 7:
                {
                    if (BRGameStatus.Instance ().LevelNumber >= 3) {    
                        prefab = structBlockType8;
                        v.x += Random.Range (-4.0f, 4.0f);
                    }
                    //v = new Vector3 (-5, 0.6f, position);
                }
                break;
            }

            float obstacleMinRange, obstacleMaxRange;
            obstacleMinRange = Mathf.Max (40.0f / (BRGameStatus.Instance ().LevelNumber * 0.75f), 20.0f);
            obstacleMaxRange = Mathf.Max (80.0f / (BRGameStatus.Instance ().LevelNumber * 0.75f), 36.0f);


            position += Random.Range (obstacleMinRange, obstacleMaxRange);
            //Vector3 v = new Vector3 (0, 0.6f, position);
            v += gameObject.transform.position;
            GameObject obj = (GameObject)Instantiate (prefab, v, Quaternion.identity);
            obj.renderer.enabled = true;
            obj.BroadcastMessage("enableRenderer");

            obj.transform.parent = gameObject.transform; // add to stage
        }
    }
}
