using UnityEngine;
using System.Collections;

public class AoELightning : MonoBehaviour {

	// Use this for initialization
	public GameObject Lightning, Cloud;
	public float betweenStrikes;
	public int Strikes;
	private float btwnStrikes;

	void Start () {
		btwnStrikes = betweenStrikes;
	}
	
	// Update is called once per frame
	void Update () {
		betweenStrikes -= Time.deltaTime;

		if (Strikes > 0 && betweenStrikes <= 0) {
			Instantiate(Lightning, Cloud.transform.position , Cloud.transform.rotation);
			Strikes -= 1;
			betweenStrikes = btwnStrikes;
		}
		if (Strikes <= 0) {
			Destroy (this.gameObject);
		}
	}
}
