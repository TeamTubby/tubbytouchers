using UnityEngine;
using System.Collections;

public class ChairSpawner : MonoBehaviour {

    public GameObject oPrefab0;
    public GameObject oPrefab1;
    public GameObject oPrefab2;

    public int iMaxChairs;
    public int iChairSpread;
    public int iMinX;
    public int iMaxX;
    public int iMinY;
    public int iMaxY;
    public int iSpawnZ;

	// Use this for initialization
	void Start () {
        for (int i = 0; i < iMaxChairs; i++)
        {
            int iPosX = Random.Range(iMinX, iMaxX);
            int iPosY = Random.Range(iMinY, iMaxY);

            Vector3 oTemp;
            oTemp.x = iPosX;
            oTemp.y = iPosY;
            oTemp.z = iSpawnZ;

            GameObject[] oTubbies = GameObject.FindGameObjectsWithTag("Tubby");
            GameObject[] oTables = GameObject.FindGameObjectsWithTag("Table");
            GameObject[] oCollidables = GameObject.FindGameObjectsWithTag("Collidable");
            GameObject[] oChairs = GameObject.FindGameObjectsWithTag("Chair");

            bool bCollideFlag = false;

            foreach (GameObject o in oTubbies)
            {
                if (Vector3.Distance(oTemp, o.transform.position) < iChairSpread)
                {
                    bCollideFlag = true;
                    break;
                }
            }

            if (!bCollideFlag)
            {
                foreach (GameObject o in oTables)
                {
                    if (Vector3.Distance(oTemp, o.transform.position) < iChairSpread)
                    {
                        bCollideFlag = true;
                        break;
                    }
                }
            }

            if (!bCollideFlag)
            {
                foreach (GameObject o in oCollidables)
                {
                    if (Vector3.Distance(oTemp, o.transform.position) < iChairSpread)
                    {
                        bCollideFlag = true;
                        break;
                    }
                }
            }

            if (!bCollideFlag)
            {
                foreach (GameObject o in oChairs)
                {
                    if (Vector3.Distance(oTemp, o.transform.position) < iChairSpread)
                    {
                        bCollideFlag = true;
                        break;
                    }
                }
            }

            if (!bCollideFlag)
            {
                int iPrefab = Random.Range(0, 3);

                switch (iPrefab)
                {
                    case 0:
                        Instantiate(oPrefab0, oTemp, Quaternion.identity);
                        break;
                    case 1:
                        Instantiate(oPrefab1, oTemp, Quaternion.identity);
                        break;
                    default:
                        Instantiate(oPrefab2, oTemp, Quaternion.identity);
                        break;
                }
            }
        }
	}

    // Update is called once per frame
    void Update()
    {
	}

}
