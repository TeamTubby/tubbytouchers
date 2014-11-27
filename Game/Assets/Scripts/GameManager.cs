﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    private int iPlayer1Score = 0;
    private int iPlayer2Score = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
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
}
