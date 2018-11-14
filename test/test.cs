using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	GameObject ob =null;
	GameObject center02 = null;
	// Use this for initialization
	void Start () {
	   ob = GameObject.Find("wave02");
		 center02 = GameObject.Find("center02");
	}
	
	// Update is called once per frame
	void Update () {
		if(ob==null){
			ob = GameObject.Find("wave02");
			center02 = GameObject.Find("center02");
		}else{
			ob.transform.RotateAround(center02.transform.position,center02.transform.up,40 * Time.deltaTime);
		}
	}
}
