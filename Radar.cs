using UnityEngine;
using System.Collections;

public class Radar : MonoBehaviour {
	
	GameObject Root;
	// Use this for initialization
	void Start () {
		Root = GameObject.FindGameObjectWithTag("Radar1root");
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(Root.transform.position,Root.transform.forward,40 * Time.deltaTime);
	}
}
