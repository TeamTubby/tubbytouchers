using UnityEngine;
using System.Collections;

public class countDown : MonoBehaviour {

	public float timeLeft = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		guiText.text = Mathf.Round(timeLeft).ToString();
		gameOver();
	}

	void gameOver(){
			if (timeLeft <= 0)
			{
				// Call End Game.
				//print("DIEDIDIDID");
			}
		}
}