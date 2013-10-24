using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour
{
	public GUISkin skin; // Unity UI에서 drag&drop으로 지정해준다.
	
	// strings :
	public string strTitle = "Blade Runner";
	Vector2 strTitleSize;
	public string strTouchStart = "Touch To Start!";
	Vector2 strTouchStartSize;
	
	// properties :
	public float waitTime = 3.0f; // wait time until start
	
	// Use this for initialization
	void Start ()
	{
		GUIContent content;
		GUIStyle styleTextCharacterName;
		
		styleTextCharacterName = skin.GetStyle ("Start_Title");	
		content = new GUIContent (strTitle);
		strTitleSize = styleTextCharacterName.CalcSize (content);   // *여기서 해당 문자열이 지정된 폰트로
		
		styleTextCharacterName = skin.GetStyle ("Start_TouchStart");	
		content = new GUIContent (strTouchStart);
		strTouchStartSize = styleTextCharacterName.CalcSize (content);   // *여기서 해당 문자열이 지정된 폰트로
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (BRGameStatus.Instance ().gameStatus () > enumGameStatus.START) {
			return;
		} else {
			waitTime -= Time.deltaTime;
			if (waitTime < 0) {
				BRGameStatus.Instance ().setGameStatus (enumGameStatus.RUNNING);
			}
			
			if (Input.GetButtonDown ("Jump")) {
				BRGameStatus.Instance ().setGameStatus (enumGameStatus.RUNNING);
			}
			
			if (Input.touches.Length > 0) {
				BRGameStatus.Instance ().setGameStatus (enumGameStatus.RUNNING);
			}
		}
	}
	
	// Update is called once per frame
	void OnGUI ()
	{
		if (BRGameStatus.Instance ().gameStatus () <= enumGameStatus.START) {
			int sw = Screen.width;
			int sh = Screen.height;
			GUI.skin = skin;
		
			GUI.Label (new Rect ((sw / 2) - (strTitleSize.x / 2), (sh / 2) - (strTitleSize.y / 2), 0, 0),
			strTitle,
			"Start_Title");
			
			GUI.Label (new Rect ((sw / 2) - (strTouchStartSize.x / 2), (sh * 2 / 3), 0, 0),
				strTouchStart,
				"Start_TouchStart");
				
		}
	}
}
