using UnityEngine;
using System.Collections;

public class FoodDrops : MonoBehaviour {

	public float timer = 3;
	public float timerReset = 3;
	
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
		}

	public GameObject prefab;
	public int numberOfObjects = 20;
	public float radius = 5f;

	void foodDrop() {
		for (int i = 0; i < numberOfObjects; i++) {
			//float angle = i * Mathf.PI * 2 / numberOfObjects;
			Vector3 pos = new Vector3( Mathf.Cos(Random.Range (10,30)), Mathf.Sin(Random.Range (10,30)), 0 ) * Random.Range (10,75);
			Instantiate(prefab, pos, Quaternion.identity);
		}
	}
}