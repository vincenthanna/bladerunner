using UnityEngine;
using System.Collections;

public class Buff
{
	public Texture IconTexture;
	public float Duration {get;set;}
	public int NeedCoinCount {get;set;}
	public Rect iconRect;
	
	public Buff ()
	{
		Duration = 10.0f;
	}
	
	public virtual void checkDuration (float deltaTime)
	{
		Duration -= deltaTime;
	}
	
	public virtual void apply(RunnerAbility ability) {}
}
