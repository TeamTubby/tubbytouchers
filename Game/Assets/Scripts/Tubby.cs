using UnityEngine;
using System.Collections;

public class Tubby : MonoBehaviour {

    public PLAYER ePlayer;
    public GameObject oCamera;

    public float fFrictionCoefficient;
    public float fRunningForce;
    public float fMaxVelocity;
    public float fPlayerMass;
    public float fReboundVelocity;
    public float fReboundTime;
    public float fNonReboundTime;

    private GameManager oGameManager;

    private float fReboundTimer = 0;

    private float fFoodMass = 1;
    private int iStackSize = 0;

    private float fForceX = 0;
    private float fForceY = 0;

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

        rigidbody.mass = fPlayerMass;

	}

    void FixedUpdate()
    {
        oTempPos2 = oTempPos1;
        oTempPos1 = transform.position;
    }

	// Update is called once per frame
	void Update () {
        GetForceX();
        GetForceY();
        ScaleForce();
        Move();
		EatCheck();
        UpdateReboundTimer();
	}

    void OnCollisionEnter(Collision a_object)
    {

        if (a_object.gameObject.tag == "Tubby")
        {
            Vector3 oReboundDir = transform.position - a_object.transform.position;
            Vector3 oOtherReboundDir = a_object.transform.position - transform.position;

            oReboundDir = oReboundDir / oReboundDir.magnitude;
            oOtherReboundDir = oOtherReboundDir / oOtherReboundDir.magnitude;

            if (Vector3.Dot(rigidbody.velocity, oOtherReboundDir) < Vector3.Dot(a_object.rigidbody.velocity, oReboundDir))
            {
                iStackSize = 0;
                
                rigidbody.velocity = oReboundDir * fReboundVelocity;
                a_object.rigidbody.velocity = Vector3.zero;
                
                fReboundTimer = fReboundTime;
                a_object.gameObject.GetComponent<Tubby>().fReboundTimer = a_object.gameObject.GetComponent<Tubby>().fNonReboundTime;
            }
            else if (Vector3.Dot(rigidbody.velocity, oOtherReboundDir) > Vector3.Dot(a_object.rigidbody.velocity, oReboundDir))
            {
                a_object.gameObject.GetComponent<Tubby>().iStackSize = 0;

                rigidbody.velocity = Vector3.zero;
                a_object.rigidbody.velocity = oOtherReboundDir * fReboundVelocity;

                fReboundTimer = fNonReboundTime;
                a_object.gameObject.GetComponent<Tubby>().fReboundTimer = a_object.gameObject.GetComponent<Tubby>().fReboundTime;
            }
            else
            {
                rigidbody.velocity = oReboundDir * fReboundVelocity;
                a_object.rigidbody.velocity = oOtherReboundDir * fReboundVelocity;

                fReboundTimer = fReboundTime;
                a_object.gameObject.GetComponent<Tubby>().fReboundTimer = a_object.gameObject.GetComponent<Tubby>().fReboundTime;
            }
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
            transform.position = oTempPos2;
        }
    }

    void OnTriggerEnter(Collider a_object)
    {
        if (a_object.tag == "Food")
        {
            if (oGameManager.GetGameTime() > 0)
            {
                iStackSize += 1;
                rigidbody.mass += fFoodMass;
            }
            Destroy( a_object.gameObject );
        }
    }

    void GetForceX()
    {
        fForceX = 0;
        if (fReboundTimer == 0)
        {
            if (ePlayer == PLAYER.PLAYER_1)
            {
                if (Input.GetKey(KeyCode.D) || Input.GetAxis("P1_360_leftX") > 0)
                {
                    fForceX += fRunningForce;
                }

                if (Input.GetKey(KeyCode.A) || Input.GetAxis("P1_360_leftX") < 0)
                {
                    fForceX -= fRunningForce;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("P2_360_leftX") > 0)
                {
                    fForceX += fRunningForce;
                }

                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("P2_360_leftX") < 0)
                {
                    fForceX -= fRunningForce;
                }
            }
        }
    }

    void GetForceY()
    {
        fForceY = 0;
        if (fReboundTimer == 0)
        {
            if (ePlayer == PLAYER.PLAYER_1)
            {
                if (Input.GetKey(KeyCode.W) || Input.GetAxis("P1_360_leftY") < 0)
                {
                    fForceY += fRunningForce;
                }

                if (Input.GetKey(KeyCode.S) || Input.GetAxis("P1_360_leftY") > 0)
                {
                    fForceY -= fRunningForce;
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("P2_360_leftY") < 0)
                {
                    fForceY += fRunningForce;
                }

                if (Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("P2_360_leftY") > 0)
                {
                    fForceY -= fRunningForce;
                }
            }
        }
    }

    void ScaleForce()
    {
        if (Mathf.Abs(fForceX) + Mathf.Abs(fForceY) > fRunningForce)
        {
            fForceX = fForceX / 2;
            fForceY = fForceY / 2;
        }
    }


    void Move()
    {
        rigidbody.AddForce(Vector3.up * (fForceY - (9.8f * rigidbody.mass * fFrictionCoefficient * rigidbody.velocity.y)));
        rigidbody.AddForce(Vector3.right * (fForceX - (9.8f * rigidbody.mass * fFrictionCoefficient * rigidbody.velocity.x)));

        float fVelocityX = rigidbody.velocity.x;
        float fVelocityY = rigidbody.velocity.y;
        float fVelocityMag = Mathf.Sqrt(Mathf.Pow(fVelocityX, 2) + Mathf.Pow(fVelocityY, 2));

        if (fVelocityMag > fMaxVelocity)
        {
            float fRadians = Mathf.Atan(fVelocityY / fVelocityX);
            bool bPositiveX;
            bool bPositiveY;

            if (fVelocityX >= 0)
            {
                bPositiveX = true;
            }
            else
            {
                bPositiveX = false;
            }

            if (fVelocityY >= 0)
            {
                bPositiveY = true;
            }
            else
            {
                bPositiveY = false;
            }

            fVelocityMag = fMaxVelocity;
            fVelocityX = fVelocityMag * Mathf.Cos(fRadians);
            fVelocityY = fVelocityMag * Mathf.Sin(fRadians);

            if (bPositiveX == true)
            {
                if (fVelocityX < 0)
                {
                    fVelocityX = fVelocityX * -1;
                }
            }
            else
            {
                if (fVelocityX > 0)
                {
                    fVelocityX = fVelocityX * -1;
                }
            }

            if (bPositiveY == true)
            {
                if (fVelocityY < 0)
                {
                    fVelocityY = fVelocityY * -1;
                }
            }
            else
            {
                if (fVelocityY > 0)
                {
                    fVelocityY = fVelocityY * -1;
                }
            }

            rigidbody.velocity = new Vector3(fVelocityX, fVelocityY, 0);
        }

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

    void EatCheck()
	{			
		if (ePlayer == PLAYER.PLAYER_1)
		{
			if (Input.GetKey(KeyCode.Q))
			{
                if (oGameManager.GetGameTime() > 0)
                {
                    if( iStackSize > 0 ) {
                        oGameManager.AddPoints(ePlayer, iStackSize + (iStackSize - 1));
                    }
                }
				iStackSize = 0;
                rigidbody.mass = fPlayerMass;
			}
		}
		else
		{
			if (Input.GetKey(KeyCode.RightControl))
			{
                if (oGameManager.GetGameTime() > 0)
                {
                    if (iStackSize > 0)
                    {
                        oGameManager.AddPoints(ePlayer, iStackSize + (iStackSize - 1));
                    }
                }
			    iStackSize = 0;
                rigidbody.mass = fPlayerMass;
			}
		}
	}

    void UpdateReboundTimer()
    {
        if (fReboundTimer > 0)
        {
            fReboundTimer -= Time.deltaTime;
            if (fReboundTimer < 0)
            {
                fReboundTimer = 0;
            }
        }
    }

    void OnGUI()
    {
        //GUI.skin = skin;
        //skin.label.alignment = TextAnchor.MiddleCenter;
        Vector3 screenPosition = new Vector3();
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);// gets screen position.
        screenPosition.y = Screen.height - screenPosition.y;// inverts y

        GUI.Label(new Rect(screenPosition.x, screenPosition.y-20, 100, 24), "" + iStackSize);
    }

}
