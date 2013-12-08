using UnityEngine;
using System.Collections;

public class RunnerAbility
{
	// runner ability :
	
	public RunnerAbility ()
	{
        reset ();
	}
	
	// Copy constructor.
    public RunnerAbility(RunnerAbility src)
    {
        this.WalkSpeed = src.WalkSpeed;
		this.WalkSpeedAtJump = src.WalkSpeedAtJump;
		this.Gravity = src.Gravity;
		this.JumpSpeed = src.JumpSpeed;
    }
	

	public float WalkSpeed {get;set;}
	
	public float WalkSpeedAtJump {get;set;}
	
	public float Gravity {get;set;}
		
	public float JumpSpeed {get;set;}

    public void reset()
    {
        // set to default;
        this.WalkSpeed = 28.0f;
        this.WalkSpeedAtJump = 24.0f;
        this.Gravity = 16.0f;
        this.JumpSpeed = 8.0f;
    }
}
