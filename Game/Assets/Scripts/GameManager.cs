using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public float fGameTime;
    public float fGameOverTextTimer;

    private int iPlayer1Score = 0;
    private int iPlayer2Score = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        UpdateGameTime();
        CheckGameOver();
        CheckEscape();
	}

    public void AddPoints(PLAYER a_ePlayer, int a_iPoints)
    {
        if (a_ePlayer == PLAYER.PLAYER_1)
        {
            iPlayer1Score += a_iPoints;
        }
        else
        {
            iPlayer2Score += a_iPoints;
        }
    }

    public int GetScore(PLAYER a_ePlayer)
    {
        if (a_ePlayer == PLAYER.PLAYER_1)
        {
            return iPlayer1Score;
        }
        else
        {
            return iPlayer2Score;
        }
    }

    void UpdateGameTime()
    {
        fGameTime -= Time.deltaTime;
        if (fGameTime < 0)
        {
            fGameTime = 0;
        }
    }

    public float GetGameTime()
    {
        return fGameTime;
    }

    void CheckGameOver()
    {
        if (fGameTime == 0)
        {
            fGameOverTextTimer -= Time.deltaTime;
            if (fGameOverTextTimer <= 0)
            {
                Application.LoadLevel(0);
            }
        }
    }

    void CheckEscape()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.LoadLevel(0);
        }
    }
}
