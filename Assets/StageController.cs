using UnityEngine;
using System.Collections;

public class StageController : MonoBehaviour {
	
	public GameObject stagePrefab;
	Vector3 nextStagePositionVector;
	int stageIndex = 1;

	// Use this for initialization
	void Start () {
		nextStagePositionVector = new Vector3(0,0,400.0f);
	}
	
	// Update is called once per frame
	void Update () {
			
	}
	void createNextStage()
	{
		nextStagePositionVector = new Vector3(0,0,(float)stageIndex * 400.0f);
		Instantiate(stagePrefab, nextStagePositionVector, Quaternion.identity);
		stageIndex++;
	}
}
