using UnityEngine;
using System.Collections;

public class FoodDrops : MonoBehaviour {

	//public Vector3[] oTablePos;
	public float timer = 3;
	public float timerReset = 3;
	public float fFoodOffset;

	private Vector3[] oTablePos = new Vector3[9];

	private int iRand;
	public int iMaxFood;
	
	// Update is called once per frame
	void Update () {
		if (timer > 0){
			timer -= Time.deltaTime;
		}
		if(timer <= 0){
			foodDrop();
			timer = timerReset;
		}
	}
	
	void Start() {
		GameObject Temp = GameObject.Find("Table1");
		oTablePos [0] = Temp.transform.position;

		Temp = GameObject.Find("Table2");
		oTablePos [1] = Temp.transform.position;

		Temp = GameObject.Find("Table3");
		oTablePos [2] = Temp.transform.position;

		Temp = GameObject.Find("Table4");
		oTablePos [3] = Temp.transform.position;

		Temp = GameObject.Find("Table5");
		oTablePos [4] = Temp.transform.position;

		Temp = GameObject.Find("Table6");
		oTablePos [5] = Temp.transform.position;

		Temp = GameObject.Find("Table7");
		oTablePos [6] = Temp.transform.position;

		Temp = GameObject.Find("Table8");
		oTablePos [7] = Temp.transform.position;

		Temp = GameObject.Find("Table9");
		oTablePos [8] = Temp.transform.position;

		//Debug.Log (oTablePos[1]);

		//oTablePos = 
		}

	public GameObject prefab;
	public int numberOfObjects = 20;
	public float radius = 5f;

	void foodDrop() {
		for (int i = 0; i < numberOfObjects; i++)
		{
			GameObject[] oFood; 
			oFood = GameObject.FindGameObjectsWithTag("Food");
			int iFoodCount = 0;

			foreach( GameObject o in oFood ){
				iFoodCount ++;
			}

			if( iFoodCount < iMaxFood )
			{
				int iIndex;
				iIndex = Random.Range(0, 9);
				Vector3 pos = oTablePos[iIndex];
				pos.x -= fFoodOffset;

			FoodPositionCheck:
				foreach( GameObject o in oFood )
				{
					Vector3 oTemp = o.transform.position;
					if( o.transform.position == pos ) {
						pos.x += fFoodOffset;
						goto FoodPositionCheck;
					}
				}
				Instantiate(prefab, pos, Quaternion.identity);
			}
		}
	}
}