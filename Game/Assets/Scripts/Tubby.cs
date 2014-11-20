﻿using UnityEngine;
using System.Collections;

public class Tubby : MonoBehaviour {

    public PLAYER ePlayer;
    public GameObject oCamera;

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
        renderer.material.color = new Color(0, 1, 0);
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

	}

    void OnCollisionEnter(Collision a_object)
    {
        rigidbody.velocity = Vector3.zero;
        transform.position = oTempPos2;
    }

    void OnTriggerEnter(Collider a_object)
    {
        if (a_object.tag == "Food")
        {
            
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
            if (Input.GetKey(KeyCode.D))
            {
                fAccelX += fAccelMag;
            }

            if (Input.GetKey(KeyCode.A))
            {
                fAccelX -= fAccelMag;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                fAccelX += fAccelMag;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
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
            if (Input.GetKey(KeyCode.W))
            {
                fAccelY += fAccelMag;
            }

            if (Input.GetKey(KeyCode.S))
            {
                fAccelY -= fAccelMag;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                fAccelY += fAccelMag;
            }

            if (Input.GetKey(KeyCode.DownArrow))
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

    /*void SetVelocityX()
    {
        fVelocityX += fAccelX * Time.deltaTime;
    }

    void SetVelocityY()
    {
        fVelocityY += fAccelY * Time.deltaTime;
    }

    void ScaleVelocity()
    {
        
        
        if (Mathf.Abs(fVelocityX) + Mathf.Abs(fVelocityY) > fAccelMag)
        {
            fVelocityX = fVelocityX / 2;
            fVelocityY = fVelocityY / 2;
        }
    }*/

    void Move()
    {
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
