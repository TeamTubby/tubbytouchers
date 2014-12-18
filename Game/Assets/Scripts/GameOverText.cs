using UnityEngine;
using System.Collections;

public class GameOverText : MonoBehaviour {

    private GameManager oGameManager;

	// Use this for initialization
	void Start () {
        GameObject Temp = GameObject.Find("GameManager");
        oGameManager = Temp.GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (oGameManager.GetGameTime() == 0)
        {
            if (oGameManager.GetScore(PLAYER.PLAYER_1) > oGameManager.GetScore(PLAYER.PLAYER_2))
            {
                guiText.text = "Player 1 Wins!";
            }
            else if (oGameManager.GetScore(PLAYER.PLAYER_1) < oGameManager.GetScore(PLAYER.PLAYER_2))
            {
                guiText.text = "Player 2 Wins!";
            }
            else
            {
                guiText.text = "You're Both Losers!";
            }
        }
        else
        {
            guiText.text = "";
        }
	}
}
