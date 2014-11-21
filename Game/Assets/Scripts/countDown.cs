using UnityEngine;
using System.Collections;

public class countDown : MonoBehaviour {

	public float timeLeft = 60;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		guiText.text = Mathf.Round(timeLeft).ToString();
	}
}
