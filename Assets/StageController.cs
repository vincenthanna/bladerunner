using UnityEngine;
using System.Collections;

public class StageController : MonoBehaviour
{
    
    public GameObject stagePrefab;
    Vector3 nextStagePositionVector;
    int stageIndex = 1;
    ArrayList stages = new ArrayList ();
    float checkBehindTimer = 5.0f;
    RunnerController runnerController;

    // Use this for initialization
    void Start ()
    {
        nextStagePositionVector = new Vector3 (0, 0, 400.0f);
        GameObject runner = GameObject.FindWithTag ("Player");
        runnerController = (RunnerController)runner.GetComponent ("RunnerController");
    }
    
    // Update is called once per frame
    void Update ()
    {
        if (BRGameStatus.Instance ().gameStatus () > enumGameStatus.START) {
            updateStages (Time.deltaTime);
        }

        // make next stage when runner gets near end
        if (BRGameStatus.Instance ().gameStatus () == enumGameStatus.RUNNING) {
            if (lastStageEndPosition () - runnerController.transform.position.z < 200.0f) {
                createNextStage ();
            }
        }
    }

    void createNextStage ()
    {
        nextStagePositionVector = new Vector3 (0, 0, (float)stageIndex * 400.0f);
        GameObject stage = (GameObject)Instantiate (stagePrefab, nextStagePositionVector, Quaternion.identity);
        stages.Add (stage);
        stageIndex++;
    }

    public float lastStageEndPosition ()
    {
        return (stageIndex * 400.0f);
    }

    void updateStages (float deltaTime)
    {
        checkBehindTimer -= deltaTime;
        if (checkBehindTimer < 0) {
            checkBehindTimer = 5.0f;

            bool deleted = true;
            while (deleted) {
                deleted = false;
                foreach (GameObject stage in stages) {
                    GameObject runner = GameObject.FindWithTag ("Player");
                    if (runner.transform.position.z > stage.transform.position.z + 600.0f) {
                        stages.Remove (stage);
                        Destroy (stage);
                        deleted = true;
                        break;
                    }
                }
            }
        }
    }

    void clearCurrentStages ()
    {
        // reset stage status 
        if (stages.Count > 0) {
            for (int i = 0; i < stages.Count; i++) {
                GameObject s = (GameObject)stages [i];
                Destroy (s);
            }
        }
        stages = new ArrayList ();
        stageIndex = 0;
        nextStagePositionVector = new Vector3 (0, 0, (float)stageIndex * 400.0f);
        GameObject stage = (GameObject)Instantiate (stagePrefab, nextStagePositionVector, Quaternion.identity);
        stages.Add (stage);
        stageIndex++;

        // reset itemGenerator
        GameObject itemGenerator = GameObject.FindWithTag ("ItemGenerator");
        itemGenerator.transform.position = new Vector3 (0, 0, 0);
        itemGenerator.SendMessage ("Reset");
    }
}
