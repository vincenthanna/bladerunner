using UnityEngine;
using System.Collections;

public class Buff_SpeedBoost : Buff {
	
	public Buff_SpeedBoost()
	{
		Duration = 1.0f;
		NeedCoinCount = 10;
		IconTexture = Resources.Load("ItemIcons/buffIcon_SpeedBoost") as Texture;		
		if (IconTexture == null) {
			Debug.Log("no IconTexture!");
		}
	}

	public override void apply (RunnerAbility ability)
	{
		ability.WalkSpeed += 80.0f;
		ability.WalkSpeedAtJump += 80.0f;
	}
}
