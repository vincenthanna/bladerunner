using UnityEngine;
using System.Collections;

public class RunnerController : MonoBehaviour
{
	bool keyboardInput = true;
	float walkSpeed = 28.0f;
	float walkSpeedAtJump = 16.0f;
	float gravity = 20.0f;
	float jumpSpeed = 12.0f;
	Vector3 velocity;
	bool stop = false;
	public AudioClip acGetCoin;
	float horizontalDirection = 0.0f;
	
	// item status
	int 
	
	// Use this for initialization
	void Start ()
	{
		if (animation) {
			animation ["Walk"].speed = 5.0f;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (BRGameStatus.Instance ().gameStatus () <= enumGameStatus.START) {
			return;
		}
		
		if (stop) {
			return;
		}
		CharacterController controller = (CharacterController)GetComponent ("CharacterController");
		
		if (keyboardInput) {
			
			/*
			if (controller.isGrounded) {
				velocity = new Vector3 (0, 0, 0f);
				//velocity *= walkSpeed;
				//Debug.Log(string.Format("({0} {1} {2})", velocity.x, velocity.y, velocity.z));
		
				if (Input.GetButtonDown ("Jump")) {
					velocity.y = jumpSpeed;
					if (animation)
						animation.CrossFade ("Jump", 0.1f);
				} else if (velocity.magnitude > 0.1) {
					if (animation)
						animation.CrossFade ("Walk", 0.1f);
					transform.LookAt (transform.position + velocity);
				} else {
					if (animation)
						animation.CrossFade ("Idle", 0.1f);
				}
			}
			*/
			
			/*
			Vector3 horizontalMoveVector = new Vector3 (Input.GetAxis ("Horizontal") * 0.5f, 0.0f, 0.0f);
			controller.Move (horizontalMoveVector);
			*/
			
			horizontalDirection += Input.GetAxis ("Horizontal");
			if (horizontalDirection > 4.0f) {
				horizontalDirection = 4.0f;
			}
			if (horizontalDirection < -4.0f) {
				horizontalDirection = -4.0f;
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
					velocity.y = jumpSpeed;
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
				nextPositionVector = directionVector * walkSpeed;			
			} else {
				Debug.Log ("at Jump!!!");
				nextPositionVector = directionVector * walkSpeedAtJump;			
			}
			velocity.y -= (gravity * Time.deltaTime);
			nextPositionVector.y = velocity.y;
			controller.Move (nextPositionVector * Time.deltaTime);
			
		} else {
		
			/*
			int sw = Screen.width, sh = Screen.height;
			bool jump = false;
			float horizontalAxis = 0.0f;
			if (Input.touches.Length == 1) {
				if (Input.touches [0].position.x < sw / 2) {
					// left
					horizontalAxis = -1.0f;
				} else {
					// right
					horizontalAxis = 1.0f;
				}
			} else if (Input.touches.Length >= 2) {
				// make jump
				jump = true;
			}
		
		
			if (controller.isGrounded) {
				velocity = new Vector3 (0, 0, 8);
				velocity *= walkSpeed;
				if (jump) {
					velocity.y = jumpSpeed;
					if (animation)
						animation.CrossFade ("Jump", 0.1f);
				} else if (velocity.magnitude > 0.5) {
					if (animation)
						animation.CrossFade ("Walk", 0.1f);
					transform.LookAt (transform.position + velocity);
				} else {
					if (animation)
						animation.CrossFade ("Idle", 0.1f);
				}
			}
		
			//float horizontalMove = Input.GetAxis ("Horizontal");
			Vector3 horizontalMoveVector = new Vector3 (horizontalAxis * 0.5f, 0.0f, 0.0f);
			controller.Move (horizontalMoveVector);
			
			velocity.y -= (gravity * Time.deltaTime);
			
			controller.Move (velocity * Time.deltaTime);
			*/
			
			int sw = Screen.width, sh = Screen.height;
			bool jump = false;
			float horizontalAxis = 0.0f;
			if (Input.touches.Length == 1) {
				if (Input.touches [0].position.x < sw / 2) {
					// left
					horizontalAxis = -0.25f;
				} else {
					// right
					horizontalAxis = 0.25f;
				}
			} else if (Input.touches.Length >= 2) {
				// make jump
				jump = true;
			}
			
			
			horizontalDirection += horizontalAxis;
			if (horizontalDirection > 4.0f) {
				horizontalDirection = 4.0f;
			}
			if (horizontalDirection < -4.0f) {
				horizontalDirection = -4.0f;
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
					velocity.y = jumpSpeed;
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
				nextPositionVector = directionVector * walkSpeed;			
			} else {
				Debug.Log ("at Jump!!!");
				nextPositionVector = directionVector * walkSpeedAtJump;			
			}
			velocity.y -= (gravity * Time.deltaTime);
			nextPositionVector.y = velocity.y;
			controller.Move (nextPositionVector * Time.deltaTime);
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
	
	void OnGetCoin ()
	{
		GameObject mainCamera = (GameObject)GameObject.FindWithTag ("MainCamera");
		AudioSource.PlayClipAtPoint (acGetCoin, mainCamera.transform.position);
	}
}
