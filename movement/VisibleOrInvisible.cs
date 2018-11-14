using UnityEngine;
using System.Collections;

public class VisibleOrInvisible : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate(0,12,0);
	}
	
	private void OnBecameVisible(){
		print("----------------");
	}
	
	private void OnBecameInvisible(){
		print("-----111111-----------");
	}
}
