using UnityEngine;
using System.Collections;

public class Buff_RunningSpeedUp_001 : Buff
{
	public Buff_RunningSpeedUp_001()
	{
		Duration = 8.0f;
		NeedCoinCount = 20;
		IconTexture = Resources.Load("ItemIcons/buffIcon_RunningSpeedUp_001") as Texture;
		if (IconTexture == null) {
			Debug.Log("no IconTexture!");
		}
	}

	public override void apply (RunnerAbility ability)
	{
		ability.WalkSpeed += 24.0f;
		ability.WalkSpeedAtJump += 24.0f;
	}
}
