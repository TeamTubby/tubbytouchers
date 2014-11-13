using UnityEngine;
using System.Collections;

public class Tubby : MonoBehaviour {

    public PLAYER ePlayer;
    public GameObject oCamera;

    private const float fMaxAccel = 50;
    private const float fMinAccel = 1;
    private const float fMaxVelocity = 30;
    private const float fAccelPenaltyPerItem = 1;

    private int iStackSize = 0;
    private float fAccelMag = 0;
    private float fAccelX = 0;
    private float fAccelY = 0;
    private float fVelocityX = 0;
    private float fVelocityY = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        SetAccelMag();
        SetAccelX();
        SetAccelY();
        ScaleAccel();
        SetVelocityX();
        SetVelocityY();
        Move();

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

    void SetVelocityX()
    {
        fVelocityX += fAccelX * Time.deltaTime;
        /*if (fVelocityX == 0)
        {
            fVelocityX += fAccelX * Time.deltaTime;
        }
        else if (fVelocityX < 0)
        {
            if (fAccelX < 0)
            {
                fVelocityX += fAccelX * Time.deltaTime;
            }
            else
            {
                fVelocityX = 0;
            }
        }
        else if (fVelocityX > 0)
        {
            if (fAccelX > 0)
            {
                fVelocityX += fAccelX * Time.deltaTime;
            }
            else
            {
                fVelocityX = 0;
            }
        }*/
    }

    void SetVelocityY()
    {
        fVelocityY += fAccelY * Time.deltaTime;
        /*if (fVelocityY == 0)
        {
            fVelocityY += fAccelY * Time.deltaTime;
        }
        else if (fVelocityY < 0)
        {
            if (fAccelY < 0)
            {
                fVelocityY += fAccelY * Time.deltaTime;
            }
            else
            {
                fVelocityY = 0;
            }
        }
        else if (fVelocityY > 0)
        {
            if (fAccelY > 0)
            {
                fVelocityY += fAccelY * Time.deltaTime;
            }
            else
            {
                fVelocityY = 0;
            }
        }*/
    }

    void ScaleVelocity()
    {
        if (Mathf.Abs(fVelocityX) + Mathf.Abs(fVelocityY) > fAccelMag)
        {
            fVelocityX = fVelocityX / 2;
            fVelocityY = fVelocityY / 2;
        }
    }

    void Move()
    {
        float MovementX = fVelocityX * Time.deltaTime;
        float MovementY = fVelocityY * Time.deltaTime;
        transform.Translate(MovementX, MovementY, 0);
        Vector3 ScreenPos = oCamera.GetComponent<Camera>().WorldToViewportPoint(transform.position);
        if (ScreenPos.x < 0)
        {
            ScreenPos.x = 0;
            transform.position = oCamera.GetComponent<Camera>().ViewportToWorldPoint(ScreenPos);
            fVelocityX = 0;
        }
        else if (ScreenPos.x > 1)
        {
            ScreenPos.x = 1;
            transform.position = oCamera.GetComponent<Camera>().ViewportToWorldPoint(ScreenPos);
            fVelocityX = 0;
        }
        if (ScreenPos.y < 0)
        {
            ScreenPos.y = 0;
            transform.position = oCamera.GetComponent<Camera>().ViewportToWorldPoint(ScreenPos);
            fVelocityY = 0;
        }
        else if (ScreenPos.y > 1)
        {
            ScreenPos.y = 1;
            transform.position = oCamera.GetComponent<Camera>().ViewportToWorldPoint(ScreenPos);
            fVelocityY = 0;
        }
    }
}
