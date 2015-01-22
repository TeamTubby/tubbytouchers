using UnityEngine;
using System.Collections;

public class Tubby : MonoBehaviour
{

    public PLAYER ePlayer;              //Represents whether player 1 or player 2
    public GameObject oCamera;          //Camera object

    public AudioClip EatSound;          //Sound played when eating
    public AudioClip FoodPickup;        //Sound played when picking up food

    public float fFrictionCoefficient;  //Coefficient of friction
    public float fRunningForce;         //Running force applied when player tries to move
    public float fMaxVelocity;          //Maximum velocity allowed for Tubby
    public float fPlayerMass;           //Mass of tubby
    public float fReboundVelocity;      //Initial rebound velocity when colliding with other player
    public float fReboundTime;          //Time that player is immobilised after losing a tubby-tubby collision
    public float fNonReboundTime;       //Time that player is immobilised after winning a tubby-tubby collision

    public Texture StandingTexture;     //Texture of tubby when not knocked over
    public Texture KnockedDownTexture;  //Texture of tubby when knocked over

    private GameManager oGameManager;   //Local reference to game manager

    private float fReboundTimer = 0;    //Time that player has been immobilised for since last tubby-tubby collision

    private float fFoodMass = 1;    //Mass gained from each item of food held
    private int iStackSize = 0;     //Current amount of food held

    private float fForceX = 0;      //Input force in x direction
    private float fForceY = 0;      //Input force in y direction

    private float fPolarAngle = Mathf.Asin(-1);     //Current polar angle of sprite
    private bool bBlocked = false;                  //Bool to signify player has collided with something other than a tubby or food
    private Vector3 fPreviousPosition;              //Position of tubby in previous frame

    Vector3 oTempPos1;  //Position of tubby during most recent fixed update
    Vector3 oTempPos2;  //Position of tubby two fixed updates ago

    // Use this for initialization
    void Start()
    {

        GameObject Temp = GameObject.Find("GameManager");
        oGameManager = Temp.GetComponent<GameManager>();

        if (ePlayer == PLAYER.PLAYER_1)
        {
            //renderer.material.color = new Color(0, 1, 0);
        }
        if (ePlayer == PLAYER.PLAYER_2)
        {
            //renderer.material.color = new Color(0, 0, 1);
        }

        rigidbody.mass = fPlayerMass;
        fPreviousPosition = transform.position;
    }

    void FixedUpdate()
    {
        oTempPos2 = oTempPos1;
        oTempPos1 = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(fPolarAngle);
        GetForceX();
        GetForceY();
        ScaleForce();
        Move();
        Rotate();
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
                rigidbody.mass = fPlayerMass;
                renderer.material.mainTexture = KnockedDownTexture;

                rigidbody.velocity = oReboundDir * fReboundVelocity;
                a_object.rigidbody.velocity = Vector3.zero;

                fReboundTimer = fReboundTime;
                a_object.gameObject.GetComponent<Tubby>().fReboundTimer = a_object.gameObject.GetComponent<Tubby>().fNonReboundTime;
            }
            else if (Vector3.Dot(rigidbody.velocity, oOtherReboundDir) > Vector3.Dot(a_object.rigidbody.velocity, oReboundDir))
            {
                a_object.gameObject.GetComponent<Tubby>().iStackSize = 0;
                a_object.gameObject.GetComponent<Tubby>().rigidbody.mass = a_object.gameObject.GetComponent<Tubby>().fPlayerMass;
                a_object.gameObject.GetComponent<Tubby>().renderer.material.mainTexture = a_object.gameObject.GetComponent<Tubby>().KnockedDownTexture;

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
            bBlocked = true;
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
                audio.PlayOneShot(FoodPickup);
            }
            Destroy(a_object.gameObject);
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
                    if (iStackSize > 0)
                    {
                        oGameManager.AddPoints(ePlayer, iStackSize + (iStackSize - 1));
                        audio.PlayOneShot(EatSound);
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
                        audio.PlayOneShot(EatSound);
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
        else if (renderer.material.mainTexture != StandingTexture)
        {
            renderer.material.mainTexture = StandingTexture;
        }
    }

    void Rotate()
    {
        if (!bBlocked)
        {
            if (transform.position != fPreviousPosition)
            {
                float fNewPolarAngle = Mathf.Atan((transform.position.y - fPreviousPosition.y) / (transform.position.x - fPreviousPosition.x));

                if (transform.position.x < fPreviousPosition.x)
                {
                    fNewPolarAngle = fNewPolarAngle + Mathf.PI;
                }
                transform.Rotate(0, 0, (fNewPolarAngle - fPolarAngle) * (180 / Mathf.PI));
                fPolarAngle = fNewPolarAngle;
            }
        }
        else
        {
            bBlocked = false;
        }
        fPreviousPosition = transform.position;
    }

    void OnGUI()
    {
        Vector3 screenPosition = new Vector3();
        screenPosition = Camera.main.WorldToScreenPoint(transform.position);// gets screen position.
        screenPosition.y = Screen.height - screenPosition.y;// inverts y

        GUI.Label(new Rect(screenPosition.x, screenPosition.y - 20, 100, 24), "" + iStackSize);
    }

}
