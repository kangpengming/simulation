using UnityEngine;
using System.Collections;

public class Smallmap : MonoBehaviour {
	
	public GUITexture smallTarget;
	private GameObject planeGame;
	Plane plane = new Plane();
	Vector2 smallPosition;
	private float smallx = 	0;
	private float smally = 0;
	// Use this for initialization
	void Start () {
		planeGame = GameObject.FindGameObjectWithTag("plane");
	 	plane = planeGame.GetComponent<Plane>();
	}
	
	// Update is called once per frame
	void Update () {
		smallPosition = plane.getXZ();
		if(!smallPosition.Equals(null)){
			smallx = smallPosition.x*128/3000+300;
			smally = smallPosition.y*58/1360+120;
			if(smally < 120 ){
				smally = 120;
			}
			if(smallx < 300){
				smallx = 300;
			}
		smallTarget.pixelInset = new Rect{
			x = smallPosition.x*128/3000+300,
			y = smally,
			width = 3,
			height = 3,
		};
		}
	}
}
