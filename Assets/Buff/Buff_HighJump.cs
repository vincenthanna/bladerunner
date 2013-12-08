using UnityEngine;
using System.Collections;

public class Buff_HighJump : Buff
{
	public Buff_HighJump()
	{
		Duration = 2.8f;
		NeedCoinCount = 6;
		IconTexture = Resources.Load("ItemIcons/buffIcon_HighJump") as Texture;
		if (IconTexture == null) {
			Debug.Log("no IconTexture!");
		}
	}
	
	public override void apply (RunnerAbility ability)
	{
		ability.JumpSpeed += 8.0f;
	}
}
