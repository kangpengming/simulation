using UnityEngine;
using System.Collections;

public class CamfollowMissile : MonoBehaviour {

	public Transform character;
	public float smoothTime = 0.01f;
	private Vector3 cameraVelocity = Vector3.zero;
	private Camera mainCamera;
	private Vector3 offset;
	private float Xrate;
	private float Yrate;
	private float Zrate;
	public Camera camera2;
	public Vector3 camera2offest;
	private bool ScrStatus;
	private int cameraCou;
	
	// Use this for initialization
	void Start () {
		cameraCou = 0;
		ScrStatus = true;
		//物体与相机之间的高度差
		offset = character.position - transform.position;
		float distacne = Vector3.Distance(character.position,transform.position);
		Xrate = offset.x/distacne;
		Yrate = offset.y/distacne;
		Zrate = offset.z/distacne;
		camera2offest = camera2.transform.position - character.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = character.position - offset;
		camera2.transform.position = camera2offest + character.position;
		
		if(Input.GetAxis("Mouse ScrollWheel")<0){
			if(offset.z < 80){
				offset.x += 2*Xrate;
				offset.y += 2*Yrate;
				offset.z += 2*Zrate;	
			}
		}
		
		if(Input.GetAxis("Mouse ScrollWheel")>0){ 
			if(offset.z > 10){
			offset.x -= 2*Xrate;
			offset.y -= 2*Yrate;
			offset.z -= 2*Zrate;
			}
		}
	}
}
