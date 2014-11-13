using UnityEngine;
using System.Collections;

public class Tubby : MonoBehaviour {

    public PLAYER ePlayer;

    private const float fMaxAccel = 10;
    private const float fMinAccel = 1;
    private const float fMaxVelocity = 30;
    private const float fAccelPenaltyPerItem = 1;

    private int iStackSize = 0;
    private float fAccelMag = 0;
    private float fAccelH = 0;
    private float fAccelV = 0;
    private float fVelocityH = 0;
    private float fVelocityV = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        SetAccelMag();
        SetAccelH();
        SetAccelV();
        ScaleAccel();

	}

    void SetAccelMag()
    {
        fAccelMag = fMaxAccel - (fAccelPenaltyPerItem * iStackSize);
        if (fAccelMag < fMinAccel)
        {
            fAccelMag = fMinAccel;
        }
    }

    void SetAccelH()
    {
        fAccelH = 0;

        if (Input.GetButton("d"))
        {
            fAccelH += fAccelMag;
        }

        if (Input.GetButton("a"))
        {
            fAccelH -= fAccelMag;
        }
    }

    void SetAccelV()
    {
        fAccelV = 0;

        if (Input.GetButton("w"))
        {
            fAccelV += fAccelMag;
        }

        if (Input.GetButton("s"))
        {
            fAccelV -= fAccelMag;
        }
    }

    void ScaleAccel()
    {
        if (Mathf.Abs(fAccelH) + Mathf.Abs(fAccelV) > fAccelMag)
        {
            fAccelH = fAccelH / 2;
            fAccelV = fAccelV / 2;
        }
    }

    void SetVelocityH()
    {
        if (fVelocityH == 0)
        {
            fVelocityH += fAccelH;
        }
        else if (fVelocityH < 0)
        {
            if (fAccelH < 0)
            {
                fVelocityH += fAccelH;
            }
            else
            {
                fVelocityH = 0;
            }
        }
        else if (fVelocityH > 0)
        {
            if (fVelocityH > 0)
            {
                fVelocityH += fAccelH;
            }
            else
            {
                fVelocityH = 0;
            }
        }
    }
}
