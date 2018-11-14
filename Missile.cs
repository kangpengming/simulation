using UnityEngine;
using System.Collections;

public class Missile : MonoBehaviour {
	
	public ParticleSystem par;
	public GameObject target;
	public GameObject missile;
	private const float Max_Rotation = 180;
	private static float Max_Rotation_frame = Max_Rotation/((float)(Application.targetFrameRate == -1 ? 60 : Application.targetFrameRate));
	private Quaternion roation;
	private float speed;
	private bool sysflag;
	private float rotationSpeed;
	
	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag("F-15E");
		missile = GameObject.FindGameObjectWithTag("missile2");
		float flyTime = 20;
		sysflag = true;
		roation = getRotation();
		rotationSpeed = Quaternion.Angle(missile.transform.rotation,roation)/flyTime;
		speed = Vector3.Distance(missile.transform.position, target.transform.position)/flyTime;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(transform.position,target.transform.position) < 40 && sysflag){
//			par.Play();
			sysflag = false;
		}
		this.transform.rotation = Quaternion.Slerp(missile.transform.rotation,roation,Time.deltaTime*rotationSpeed);
		this.transform.Translate(new Vector3(0,0,Time.deltaTime*speed));
	}
	
	public Quaternion getRotation(){
		
		//计算差值
		float dx = target.transform.position.x - missile.transform.position.x;
		float dy = target.transform.position.y - missile.transform.position.y;
		float dz = target.transform.position.z - missile.transform.position.z;
		
		float rotationTheta = Mathf.Atan(dx/dz)*180/Mathf.PI;
		float dxz = Mathf.Sqrt(dz*dz+dx*dx);
		float rotattionPhi = Mathf.Atan(dy/dxz) * 180/Mathf.PI;
		rotattionPhi = -rotattionPhi;
		return Quaternion.Euler(rotattionPhi,rotationTheta,0);
	
	}
}
