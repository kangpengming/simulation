using UnityEngine;
using System.Collections;

public class Tree : MonoBehaviour {
	
	private GameObject tree = null;
	private GameObject trees = null;
	// Use this for initialization
	void Start () {
		for(int i = 0;i < 100 ; i++){
			tree = (GameObject)Instantiate(Resources.Load("Prefab/tree"));
			trees = GameObject.Find("Trees");
			tree.transform.parent = trees.transform;
			tree.transform.position = new Vector3(i*2+3,100,2*i+10); 
		}
		}
	
	// Update is called once per frame
	void Update () {
	
	}
}
