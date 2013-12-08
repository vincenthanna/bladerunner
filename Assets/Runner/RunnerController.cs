using UnityEngine;
using System.Collections;
using System;

public class RunnerController : MonoBehaviour
{
    bool keyboardInput = false;
    RunnerAbility runnerAbility = new RunnerAbility ();
    BuffContainer buffContainer = new BuffContainer ();
    Vector3 velocity;
    bool stop = false;
    public AudioClip acGetCoin;
    float horizontalDirection = 0.0f;
    public GUISkin skin;
    float horizontalDirectionLimitPositive = 5.0f;
    Buff tmpBuff;
    Rect rectMoveTouchIgnore;

    // item status
    public int CoinCount{ get; set; }

    public int getCoinCount ()
    {
        return CoinCount;
    }
    
    // Use this for initialization
    void Start ()
    {
        if (animation) {
            animation ["Walk"].speed = 5.0f;
        }
        
        runnerAbility = new RunnerAbility ();
        buffContainer = new BuffContainer ();

        tmpBuff = new Buff_SpeedBoost ();
        int sw = Screen.width, sh = Screen.height;
        //rectMoveTouchIgnore = new Rect (0, sh - tmpBuff.IconTexture.height, tmpBuff.IconTexture.width * 3, tmpBuff.IconTexture.height);
        rectMoveTouchIgnore = new Rect (0, 0, tmpBuff.IconTexture.width * 3, tmpBuff.IconTexture.height);
        Debug.Log(string.Format("rectMoveTouchIgnore={0}", rectMoveTouchIgnore));
    }
    
    // Update is called once per frame
    void Update ()
    {
        if (BRGameStatus.Instance ().gameStatus () != enumGameStatus.RUNNING) {
            return;
        }
        
        if (stop) {
            return;
        }
        CharacterController controller = (CharacterController)GetComponent ("CharacterController");
        
        RunnerAbility ra = new RunnerAbility (runnerAbility);
        
        checkSpecialTechniquesTrigger ();
        
        // apply all buffs
        buffContainer.applyAllBuffs (ra);
        buffContainer.updateBuffs (Time.deltaTime);
        
        if (keyboardInput) {
            
            horizontalDirection += Input.GetAxis ("Horizontal");
            if (horizontalDirection > horizontalDirectionLimitPositive) {
                horizontalDirection = horizontalDirectionLimitPositive;
            }
            if (horizontalDirection < -horizontalDirectionLimitPositive) {
                horizontalDirection = -horizontalDirectionLimitPositive;
            }
            
            // make direction and turn to it.           
            Vector3 directionVector = new Vector3 (horizontalDirection * 0.1f, 0.0f, 1.0f).normalized;
            /*
            Debug.Log (string.Format ("directionVector=({0},{1},{2})",
                directionVector.x, directionVector.y, directionVector.z));
                */
            controller.transform.LookAt (directionVector + controller.transform.position);
            
            if (controller.isGrounded) {
                velocity = new Vector3 (0, 0, 0);
                
                if (Input.GetButtonDown ("Jump")) {
                    Debug.Log ("Animation : Jump");
                    velocity.y = ra.JumpSpeed;
                    if (animation)
                        animation.CrossFade ("Jump", 0.1f);
                } else {                    
                    if (animation) {
                        if (animation.IsPlaying ("Walk") == false) {
                            animation.CrossFade ("Walk", 0.1f);
                            //Debug.Log ("Animation : Walk");
                        }
                    }
                }
            }
            
            // make move to direction + jump up
            Vector3 nextPositionVector;
            if (controller.isGrounded) {
                nextPositionVector = directionVector * ra.WalkSpeed;            
            } else {
                //Debug.Log ("at Jump!!!");
                nextPositionVector = directionVector * ra.WalkSpeedAtJump;          
            }
            velocity.y -= (ra.Gravity * Time.deltaTime);
            nextPositionVector.y = velocity.y;
            controller.Move (nextPositionVector * Time.deltaTime);
            
        } else {
            int sw = Screen.width, sh = Screen.height;
            bool jump = false;
            float horizontalAxis = 0.0f;

            if (Input.touches.Length == 1) {

                // skip if buff activation area is touched

                Vector2 touchPos = Input.touches[0].position;

                if (rectMoveTouchIgnore.Contains (touchPos)) {
                    // ignore this.
                    Debug.Log ("trace~~~");
                } else {
                    if (Input.touches [0].position.x < sw / 2) {
                        // left
                        horizontalAxis = -0.16f;
                    } else {
                        // right
                        horizontalAxis = 0.16f;
                    }
                }
            } else if (Input.touches.Length >= 2) {
                // make jump
                jump = true;
            }
            
            
            horizontalDirection += horizontalAxis;
            if (horizontalDirection > horizontalDirectionLimitPositive) {
                horizontalDirection = horizontalDirectionLimitPositive;
            }
            if (horizontalDirection < -horizontalDirectionLimitPositive) {
                horizontalDirection = -horizontalDirectionLimitPositive;
            }
            
            // make direction and turn to it.           
            Vector3 directionVector = new Vector3 (horizontalDirection * 0.1f, 0.0f, 1.0f).normalized;
            /*
            Debug.Log (string.Format ("directionVector=({0},{1},{2})",
                directionVector.x, directionVector.y, directionVector.z));
                */
            controller.transform.LookAt (directionVector + controller.transform.position);
            
            if (controller.isGrounded) {
                velocity = new Vector3 (0, 0, 0);
                
                if (jump) {
                    Debug.Log ("Animation : Jump");
                    velocity.y = ra.JumpSpeed;
                    if (animation)
                        animation.CrossFade ("Jump", 0.1f);
                } else {                    
                    if (animation) {
                        if (animation.IsPlaying ("Walk") == false) {
                            animation.CrossFade ("Walk", 0.1f);
                            //Debug.Log ("Animation : Walk");
                        }
                    }
                }
            }
            
            // make move to direction + jump up
            Vector3 nextPositionVector;
            if (controller.isGrounded) {
                nextPositionVector = directionVector * ra.WalkSpeed;            
            } else {
                Debug.Log ("at Jump!!!");
                nextPositionVector = directionVector * ra.WalkSpeedAtJump;          
            }
            velocity.y -= (ra.Gravity * Time.deltaTime);
            nextPositionVector.y = velocity.y;

            controller.Move (nextPositionVector * Time.deltaTime);
        }

        // check boundaries
        bool needRepositioning = false;
        Vector3 pos = controller.transform.position;
        if (pos.x > (10.0f - 1.2f)) {
            pos.x = (10.0f - 1.2f);
            needRepositioning = true;
        }   
        if (pos.x < (-10.0f + 1.2f)) {
            pos.x = (-10.0f + 1.2f);
            needRepositioning = true;
        }
        if (needRepositioning) {
            controller.transform.position = pos;
        }
    }
    
    void OnTriggerEnter (Collider other)
    {
        //Debug.Log (string.Format ("### player OnTriggerEnter tag={0}", other.gameObject.tag));
    }
    
    void endGame ()
    {
        Debug.Log ("END GAME!!!");
        stop = true;
    }
    
    void OnGetCoin (int coinCount)
    {
        GameObject mainCamera = (GameObject)GameObject.FindWithTag ("MainCamera");
        AudioSource.PlayClipAtPoint (acGetCoin, mainCamera.transform.position);
        
        Debug.Log (string.Format ("Get Coin = {0}", coinCount));
        CoinCount += coinCount;
    }

    void OnGetBill ()
    {
        Debug.Log ("Get Bill!!!");
        CoinCount += 50;
    }
    
    void OnGUI ()
    {
        /*
        int sw = Screen.width;
        int sh = Screen.height;
        GUI.skin = skin;
        string str = string.Format ("{0}", CoinCount);
        GUI.Label (new Rect (0, 0, sw, sh), str, "CoinCount");
        */      
        
    }
    
    // special technique list
    
    void checkSpecialTechniquesTrigger ()
    {
        if (Input.GetButtonDown ("z")) {
            specialTechniquesOn (new Buff_HighJump ());
        }
        
        if (Input.GetButtonDown ("x")) {
            specialTechniquesOn (new Buff_RunningSpeedUp_001 ());
        }
        
        if (Input.GetButtonDown ("c")) {
            specialTechniquesOn (new Buff_SpeedBoost ());
        }
    }
    
    public void specialTechniquesOn (Buff buff)
    {
        Debug.Log ("Special On");
        if (CoinCount < buff.NeedCoinCount) {
            return;
        }
        CoinCount -= buff.NeedCoinCount;
        buffContainer.add ((Buff)System.Activator.CreateInstance (buff.GetType ()));
    }


    // reset to start status
    public void prepareNewRun ()
    {
        CharacterController controller = (CharacterController)GetComponent ("CharacterController");
        if (controller == null) {
            Debug.Log ("Fatal Error!");
        }
        controller.transform.position = new Vector3 (0, 0.01f, 5);
        velocity = new Vector3 (0, 0, 0);
        horizontalDirection = 0;
        runnerAbility.reset ();
        buffContainer.clear ();

        animation.CrossFade ("Idle", 0.1f);
    }

    public bool buffIsOn (Buff buff)
    {
        return buffContainer.isOn (buff);
    }
}
