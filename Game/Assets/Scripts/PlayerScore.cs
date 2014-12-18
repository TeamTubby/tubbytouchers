using UnityEngine;
using System.Collections;

public class PlayerScore : MonoBehaviour {

    public PLAYER ePlayer;

	// Use this for initialization

    private GameManager oGameManager;

	void Start () {

        GameObject Temp = GameObject.Find("GameManager");
        oGameManager = Temp.GetComponent<GameManager>();

	}
	
	// Update is called once per frame
	void Update () {

        if (ePlayer == PLAYER.PLAYER_1)
        {
            guiText.text = "Player 1 Score: " + oGameManager.GetScore(PLAYER.PLAYER_1);
        }
        else
        {
            guiText.text = "Player 2 Score: " + oGameManager.GetScore(PLAYER.PLAYER_2);
        }
	}
}
