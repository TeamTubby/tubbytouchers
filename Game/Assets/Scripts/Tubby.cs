﻿using UnityEngine;
using System.Collections;

public class Tubby : MonoBehaviour {

    public PLAYER ePlayer;
    public GameObject oCamera;

    private GameManager oGameManager;

    private const float fMaxAccel = 100;
    private const float fMinAccel = 100;
    private const float fDecel = 10;
    private const float fMaxVelocity = 30;
    private const float fAccelPenaltyPerItem = 1;

    private int iStackSize = 0;
    private float fAccelMag = 0;
    private float fAccelX = 0;
	private float fAccelY = 0;

    Vector3 oTempPos1;
    Vector3 oTempPos2;

	// Use this for initialization
	void Start () {
        
        GameObject Temp = GameObject.Find("GameManager");
        oGameManager = Temp.GetComponent<GameManager>();

		if (ePlayer == PLAYER.PLAYER_1)
		{
        	renderer.material.color = new Color(0, 1, 0);
		}
		if (ePlayer == PLAYER.PLAYER_2)
		{
			renderer.material.color = new Color(0, 0, 1);
		}

	}

    void FixedUpdate()
    {
        oTempPos2 = oTempPos1;
        oTempPos1 = transform.position;
    }

	// Update is called once per frame
	void Update () {
        SetAccelMag();
        SetAccelX();
        SetAccelY();
        ScaleAccel();
        Move();
		EatCheck();
	}


    void OnCollisionEnter(Collision a_object)
    {
        Debug.Log("Collision");
        rigidbody.velocity = Vector3.zero;
        transform.position = oTempPos2;
    }

    void OnTriggerEnter(Collider a_object)
    {
        Debug.Log("Collision");
        if (a_object.tag == "Food")
        {
            iStackSize += 1;
            Destroy( a_object.gameObject );
        }
    }

    void EatCheck()
	{			
		if (ePlayer == PLAYER.PLAYER_1)
		{
			if (Input.GetKey(KeyCode.Q))
			{
                oGameManager.AddPoints(ePlayer, iStackSize);
				iStackSize = 0;
			}
		}
		else
		{
			if (Input.GetKey(KeyCode.RightControl))
			{
                oGameManager.AddPoints(ePlayer, iStackSize);
			    iStackSize = 0;
			}
		}
	}

    void SetAccelMag()
    {
        fAccelMag = fMaxAccel - (fAccelPenaltyPerItem * iStackSize);
        if (fAccelMag < fMinAccel)
        {
            fAccelMag = fMinAccel;
        }
    }

    void SetAccelX()
    {
        fAccelX = 0;

        if (ePlayer == PLAYER.PLAYER_1)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetAxis("P1_360_leftX") >0)
            {
                fAccelX += fAccelMag;
            }

			if (Input.GetKey(KeyCode.A) || Input.GetAxis("P1_360_leftX") <0)
            {
                fAccelX -= fAccelMag;
            }
        }
        else
        {
			if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("P2_360_leftX") >0)
            {
                fAccelX += fAccelMag;
            }

			if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("P2_360_leftX") <0)
            {
                fAccelX -= fAccelMag;
            }
        }
    }

    void SetAccelY()
    {
        fAccelY = 0;

        if (ePlayer == PLAYER.PLAYER_1)
        {
			if (Input.GetKey(KeyCode.W) || Input.GetAxis("P1_360_leftY") <0)
            {
                fAccelY += fAccelMag;
            }

			if (Input.GetKey(KeyCode.S) || Input.GetAxis("P1_360_leftY") >0)
            {
                fAccelY -= fAccelMag;
            }
        }
        else
        {
			if (Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("P2_360_leftY") <0)
            {
                fAccelY += fAccelMag;
            }

			if (Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("P2_360_leftY") >0)
            {
                fAccelY -= fAccelMag;
            }
        }
    }

    void ScaleAccel()
    {
        if (Mathf.Abs(fAccelX) + Mathf.Abs(fAccelY) > fAccelMag)
        {
            fAccelX = fAccelX / 2;
            fAccelY = fAccelY / 2;
        }
    }

    void Move()
    {
        if (ePlayer == PLAYER.PLAYER_1)
        {
            Debug.Log(fAccelY);
        }
        rigidbody.AddForce(Vector3.up * fAccelY);
        rigidbody.AddForce(Vector3.right * fAccelX);

        Vector3 ScreenPos = oCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);
        if (ScreenPos.x < 0)
        {
            ScreenPos.x = 0;
            transform.position = oCamera.GetComponent<Camera>().ViewportToWorldPoint(ScreenPos);
            rigidbody.velocity = Vector3.zero;
        }
        else if (ScreenPos.x > 1)
        {
            ScreenPos.x = 1;
            transform.position = oCamera.GetComponent<Camera>().ViewportToWorldPoint(ScreenPos);
            rigidbody.velocity = Vector3.zero;
        }
        if (ScreenPos.y < 0)
        {
            ScreenPos.y = 0;
            transform.position = oCamera.GetComponent<Camera>().ViewportToWorldPoint(ScreenPos);
            rigidbody.velocity = Vector3.zero;
        }
        else if (ScreenPos.y > 1)
        {
            ScreenPos.y = 1;
            transform.position = oCamera.GetComponent<Camera>().ViewportToWorldPoint(ScreenPos);
            rigidbody.velocity = Vector3.zero;
        }
    }
}
