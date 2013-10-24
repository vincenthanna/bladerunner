using UnityEngine;
using System.Collections;

public class Effect_ItemCoinGain : MonoBehaviour
{
	ParticleSystem ps;

	// Use this for initialization
	void Start ()
	{
		ps = (ParticleSystem)GetComponent ("ParticleSystem");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!ps.IsAlive ()) {
			Destroy (gameObject);
		}
	}
}
