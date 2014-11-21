using UnityEngine;
using System.Collections;

public class Player1Score : MonoBehaviour {

	public int test = 10;
	//test = 10000;

	// Use this for initialization
	void Start () {
		guiText.text = "Player 1 Score "+test;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
