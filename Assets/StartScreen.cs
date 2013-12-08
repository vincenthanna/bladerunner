using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour
{
    public GUISkin skin; // Unity UI에서 drag&drop으로 지정해준다.
    public GUIText guiTextTimeRunningOut;

    // strings :
    public string strTitle = "CoinRunner";
    Vector2 strTitleSize;
    public string strTouchStart = "Touch To Start!";
    Vector2 strTouchStartSize;
    
    // properties :
    public float waitTime = 3.0f; // wait time until start
    
    ArrayList allBuffs = new ArrayList ();
    RunnerController runnerController;
    RecordController recordController;

    // game duration
    float currentStageTimeoutTime = 25.0f;
    float showResultTimer = 3.0f;
    float runningLengthForClear = 200.0f;

    // Use this for initialization
    void Start ()
    {
        strTitle = "CoinRunner";
        string strTouchStart = "Touch To Start!";
        waitTime = 5.0f;
        currentStageTimeoutTime = 25.0f;
        showResultTimer = 5.0f;
        runningLengthForClear = 200.0f;

        GUIContent content;
        GUIStyle styleTextCharacterName;

        int sw = Screen.width;
        int sh = Screen.height;
        
        styleTextCharacterName = skin.GetStyle ("Start_Title"); 
        content = new GUIContent (strTitle);
        strTitleSize = styleTextCharacterName.CalcSize (content);   // *여기서 해당 문자열이 지정된 폰트로
        
        styleTextCharacterName = skin.GetStyle ("Start_TouchStart");    
        content = new GUIContent (strTouchStart);
        strTouchStartSize = styleTextCharacterName.CalcSize (content);   // *여기서 해당 문자열이 지정된 폰트로

        // set buff icon setting
        Buff buff;
        float baseX = 0.0f;
        
        buff = new Buff_SpeedBoost ();
        buff.iconRect = new Rect (baseX, sh - buff.IconTexture.height, buff.IconTexture.width, buff.IconTexture.height);
        allBuffs.Add (buff);
        baseX += buff.IconTexture.width + 10;
        
        buff = new Buff_HighJump ();        
        buff.iconRect = new Rect (baseX, sh - buff.IconTexture.height, buff.IconTexture.width, buff.IconTexture.height);
        allBuffs.Add (buff);
        baseX += buff.IconTexture.width + 10;
        
        buff = new Buff_RunningSpeedUp_001 ();
        buff.iconRect = new Rect (baseX, sh - buff.IconTexture.height, buff.IconTexture.width, buff.IconTexture.height);
        allBuffs.Add (buff);
        baseX += buff.IconTexture.width + 10;
        
        GameObject runner = GameObject.FindWithTag ("Player");
        runnerController = (RunnerController)runner.GetComponent ("RunnerController");

        recordController = (RecordController)(GameObject.FindWithTag ("RecordController").GetComponent ("RecordController"));

        updateLevelCondition ();
    }
    
    // Update is called once per frame
    void Update ()
    {
        switch (BRGameStatus.Instance ().gameStatus ()) {
        case enumGameStatus.RUNNING:
            {
                currentStageTimeoutTime -= Time.deltaTime;
                if (currentStageTimeoutTime < 0) {
                    // Timeout - GAME OVER
                    showResultTimer = 4.0f;
                    BRGameStatus.Instance ().setGameStatus (enumGameStatus.TIMEOUT);
                    
                } else if (runnerController.transform.position.z > runningLengthForClear) {

                    // CLEAR! - Move to Next Level
                    showResultTimer = 3.0f;
                    BRGameStatus.Instance ().setGameStatus (enumGameStatus.CLEAR);
                }
            }
            break;
        case enumGameStatus.CLEAR:
            {
                showResultTimer -= Time.deltaTime;
                if (showResultTimer < 0) {
                    currentStageTimeoutTime = 25.0f;
                    GameObject stageController = (GameObject)GameObject.FindWithTag ("StageController");
                    stageController.SendMessage ("clearCurrentStages");
                    runnerController.SendMessage ("prepareNewRun");
                    BRGameStatus.Instance ().setGameStatus (enumGameStatus.START);
                    BRGameStatus.Instance ().LevelNumber++;

                    // update level timeout & clear condition
                    updateLevelCondition ();
                }
            }
            break;
        case enumGameStatus.TIMEOUT:
            {
                showResultTimer -= Time.deltaTime;
                if (showResultTimer < 0) {
                    GameObject rc = (GameObject)GameObject.FindWithTag ("RecordController");
                    rc.SendMessage ("updateRecord");

                    currentStageTimeoutTime = 25.0f;
                    GameObject stageController = (GameObject)GameObject.FindWithTag ("StageController");
                    stageController.SendMessage ("clearCurrentStages");
                    runnerController.SendMessage ("prepareNewRun");
                    BRGameStatus.Instance ().setGameStatus (enumGameStatus.START);
                    BRGameStatus.Instance ().LevelNumber = 1;
                    runnerController.CoinCount = 0;
                    
                    // update level timeout & clear condition
                    updateLevelCondition ();
                }
            }
            break;

        case enumGameStatus.START:
            {
                /*
                waitTime -= Time.deltaTime;
                if (waitTime < 0) {
                    BRGameStatus.Instance ().setGameStatus (enumGameStatus.RUNNING);
                }
                */
            
                if (Input.GetButtonDown ("Jump")) {
                    BRGameStatus.Instance ().setGameStatus (enumGameStatus.RUNNING);
                }
            
                if (Input.touches.Length > 0) {
                    BRGameStatus.Instance ().setGameStatus (enumGameStatus.RUNNING);
                }
            }
            break;
        }
    }
    
    void OnGUI ()
    {
        int sw = Screen.width;
        int sh = Screen.height;
        GUI.skin = skin;

        switch (BRGameStatus.Instance ().gameStatus ()) {
        case enumGameStatus.START:
            {
                if (BRGameStatus.Instance ().LevelNumber == 1) {
                    GUI.Label (new Rect ((sw / 2) - (strTitleSize.x / 2), (sh / 2) - (strTitleSize.y / 2), 0, 0),
                       strTitle,
                       "Start_Title");
                } else {
                    string str = string.Format ("Level {0}", BRGameStatus.Instance ().LevelNumber);
                    GUIStyle styleTextCharacterName = skin.GetStyle ("Start_Title"); 
                    GUIContent content = new GUIContent (str);
                    Vector2 strSize = styleTextCharacterName.CalcSize (content);   // 크기를 얻는함수 인자는 GUIContent
                    GUI.Label (new Rect (sw - strSize.x, 0, strSize.x, strSize.y), str, "Start_Title");

                    GUI.Label (new Rect ((sw / 2) - (strSize.x / 2), (sh / 2) - (strSize.y / 2), 0, 0),
                           str, "Start_Title");
                }

                GUI.Label (new Rect ((sw / 2) - (strTouchStartSize.x / 2), (sh * 2 / 3), 0, 0),
                       strTouchStart,
                       "Start_TouchStart");
            }
            break;
        case enumGameStatus.RUNNING:
            {
                // Remaining Time
                string str = string.Format ("{0:F1}", currentStageTimeoutTime);
                GUIStyle styleTextCharacterName = skin.GetStyle ("TimeAttack"); 
                GUIContent content = new GUIContent (str);
                Vector2 strSize = styleTextCharacterName.CalcSize (content);   // 크기를 얻는함수 인자는 GUIContent
                GUI.Label (new Rect (sw - strSize.x, 0, strSize.x, strSize.y), str, "TimeAttack");
                
                // Running Length
                float runLength = runnerController.transform.position.z;
                str = string.Format ("Running {0:F1}", runLength);
                styleTextCharacterName = skin.GetStyle ("RunLength"); 
                content = new GUIContent (str);
                strSize = styleTextCharacterName.CalcSize (content);   // 크기를 얻는함수 인자는 GUIContent
                GUI.Label (new Rect (sw - strSize.x, sh / 5, strSize.x, strSize.y), str, "RunLength");

                // Buffs In Effect

                // show available item icon
                for (int i = 0; i < allBuffs.Count; i++) {
                    Buff buff = ((Buff)allBuffs [i]);
                
                    if (buff.NeedCoinCount < runnerController.getCoinCount ()) {
                        if (GUI.Button (buff.iconRect, buff.IconTexture)) {
                            runnerController.SendMessage ("specialTechniquesOn", buff);
                        }
                    }
                }

                // Stage Number
                str = string.Format ("Stage {0}", BRGameStatus.Instance ().LevelNumber);
                styleTextCharacterName = skin.GetStyle ("RunLength");
                content = new GUIContent (str);
                strSize = styleTextCharacterName.CalcSize (content);   // 크기를 얻는함수 인자는 GUIContent
                GUI.Label (new Rect ((sw - strSize.x) / 2, 0, strSize.x, strSize.y), str, "RunLength");


                // Inform if Time is Running Out!
                if (currentStageTimeoutTime <= 11.0f) {
                    int remainedTimeSec = (int)currentStageTimeoutTime;
                    str = string.Format ("{0}", remainedTimeSec);
                    styleTextCharacterName = skin.GetStyle ("Timeout");
                    content = new GUIContent (str);
                    strSize = styleTextCharacterName.CalcSize (content);   // 크기를 얻는함수 인자는 GUIContent
                    GUI.Label (new Rect ((sw - strSize.x) / 2, sh / 3, strSize.x, strSize.y), str, "Timeout");
                }

                // alive Buff list
                float iconPosX = sw;
                for (int i = 0; i < allBuffs.Count; i++) {
                    Buff buff = ((Buff)allBuffs [i]);
                    if (runnerController.buffIsOn (buff)) {
                        //Debug.Log (string.Format ("buff {0} is On!", buff.GetType ().ToString ()));
                        GUI.DrawTexture (new Rect (iconPosX - buff.iconRect.width, (sh / 5) * 2, buff.iconRect.width, buff.iconRect.height),
                                    buff.IconTexture);
                        iconPosX -= buff.iconRect.width;
                    }
                }
                
                
            }
            break;
        case enumGameStatus.CLEAR:
            {
                string str = string.Format ("Stage {0} Clear!", BRGameStatus.Instance ().LevelNumber);
                GUIStyle styleTextCharacterName = skin.GetStyle ("RunLength");
                GUIContent content = new GUIContent (str);
                Vector2 strSize = styleTextCharacterName.CalcSize (content);   // 크기를 얻는함수 인자는 GUIContent
                GUI.Label (new Rect ((sw - strSize.x) / 2, (sh - strSize.y) / 2, strSize.x, strSize.y), str, "RunLength");
            }
            break;
        case enumGameStatus.TIMEOUT:
            {
                string str = string.Format ("Game Over");
                GUIStyle styleTextCharacterName = skin.GetStyle ("RunLength");
                GUIContent content = new GUIContent (str);
                Vector2 strSize = styleTextCharacterName.CalcSize (content);   // 크기를 얻는함수 인자는 GUIContent
                GUI.Label (new Rect ((sw - strSize.x) / 2, (sh - strSize.y) / 2, strSize.x, strSize.y), str, "RunLength");
            }
            break;
        }

        // show coin count
        {
            string s = string.Format ("Coin:{0}", runnerController.CoinCount);
            GUI.Label (new Rect (0, 0, sw, sh), s, "Coin");
        }

        // show max level
        if (recordController.getLastRecord () > 0) {
            string str = string.Format ("Level Max:{0}", recordController.getLastRecord ());
            GUIStyle styleTextCharacterName = skin.GetStyle ("RunLength");
            GUIContent content = new GUIContent (str);
            Vector2 strSize = styleTextCharacterName.CalcSize (content);   // 크기를 얻는함수 인자는 GUIContent
            GUI.Label (new Rect (0, sh / 4, strSize.x, strSize.y), str, "RunLength");
        }
    }

    void updateLevelCondition ()
    {
        currentStageTimeoutTime = 55.0f - (BRGameStatus.Instance ().LevelNumber * 3);
        if (currentStageTimeoutTime < 30.0f) {
            currentStageTimeoutTime = 30.0f;
        }

        runningLengthForClear = ((BRGameStatus.Instance ().LevelNumber) * 120.0f) + 200;
    }
}
