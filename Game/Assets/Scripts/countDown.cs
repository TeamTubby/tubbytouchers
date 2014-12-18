using UnityEngine;
using System.Collections;

public class countDown : MonoBehaviour {

    private GameManager oGameManager;

	// Use this for initialization
	void Start () {
        GameObject Temp = GameObject.Find("GameManager");
        oGameManager = Temp.GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        guiText.text = Mathf.Round(oGameManager.GetGameTime()).ToString();
	}

}