using UnityEngine;
using LitJson;
using System.Collections;

public class Camfollow : MonoBehaviour {
	
	public Transform character;
	public float smoothTime = 0.01f;
	private Vector3 cameraVelocity = Vector3.zero;
	private Camera mainCamera;
	private Vector3 offset;
	private float Xrate;
	private float Yrate;
	private float Zrate;
	//public Camera camera2;
	public Vector3 camera1offest;
	public Vector3 camera2offest;
	private GameObject target;
	private GameObject target1;
	private GameObject Camera1;
	private GameObject Camera2;
	private GameObject Camera3;
	private GameObject Camera4;
	private Load load = null;
	private bool flag = false;
	//因为动态的相机需要找到跟随的物体，因此需要在这两个相机中加入标签，
	private bool flag1 = false;
	private bool flag2 = false;
	private bool flag3 = false;
	private bool flag4 = false;
	private string text;
	private int numModel = 0;
	private int numOrigin = 0;
	// Use this for initialization
	void Start () {
		//寻找到所有的相机,命名原因：为了在文件中的统一性，主要是数字大一些不易于与其他的重复,radar2Camera与radar3Camera为动态，在这里前两个是动态相机
		 Camera1 = GameObject.FindGameObjectWithTag("41");
		 Camera2 = GameObject.FindGameObjectWithTag("42");
		 Camera3 = GameObject.FindGameObjectWithTag("43");
		 Camera4 = GameObject.FindGameObjectWithTag("44");
		
		GameObject mainCamera = GameObject.FindGameObjectWithTag("Sun");
		load =  mainCamera.GetComponent<Load>();
		text = load.getFiles(new OtherConst().getCameraConfPath());
		string textModel = load.getFiles(new OtherConst().getConfPath());
		load.parseFiles(textModel);	
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if( numModel != numOrigin  || (numModel == numOrigin && numModel==0)){
			parseCamFile(text);
		}else{
			if(target != null)
				Camera1.transform.position = camera1offest + target.transform.position;
			if(target1 != null)
				Camera2.transform.position = camera2offest + target1.transform.position;
		}
	}
	/**
	 * 总共有四个相机，前两个相机是跟随物体运动的。后两个相机是静止的。
	 * */
	public void parseCamFile(string filestr){
		CamModel camModel = JsonMapper.ToObject<CamModel>(filestr);
		numOrigin = camModel.Camlist.Count;
		foreach(Camerapropety cameInof in camModel.Camlist){
			if(cameInof.CameraName.Equals("1")){
				Camera1.transform.position = new Vector3(float.Parse(cameInof.CameraX),float.Parse(cameInof.CameraY),float.Parse(cameInof.CameraZ));
				target = GameObject.Find(cameInof.name);
				if(target != null && !flag1){
					Camera1.transform.LookAt(target.transform.position);
					numModel++;
					flag1 = true;
					camera1offest = Camera1.transform.position - target.transform.position;
					target.AddComponent<Movement>();
					target.AddComponent<Plane>();
				}
			}else if(cameInof.CameraName.Equals("2")){
			
				Camera2.transform.position = new Vector3(float.Parse(cameInof.CameraX),float.Parse(cameInof.CameraY),float.Parse(cameInof.CameraZ));
				target1 = GameObject.Find(cameInof.name);
				if(target1 != null && !flag2){
					Camera2.transform.LookAt(target1.transform.position);
					numModel++;
					flag2 = true;
					camera2offest = Camera2.transform.position - target1.transform.position;
					target1.AddComponent<Movement>();
					target1.AddComponent<Plane>();
				}
			}else if(cameInof.CameraName.Equals("3") && !flag3){
				
				Camera3.transform.position = new Vector3(float.Parse(cameInof.CameraX),float.Parse(cameInof.CameraY),float.Parse(cameInof.CameraZ));
				Camera3.transform.Rotate(new Vector3(float.Parse(cameInof.RotationX),float.Parse(cameInof.RotationY),float.Parse(cameInof.RotationZ))); 
				numModel++;
				flag3 = true;
			}else if(cameInof.CameraName.Equals("4") && !flag4){
				Camera4.transform.position = new Vector3(float.Parse(cameInof.CameraX),float.Parse(cameInof.CameraY),float.Parse(cameInof.CameraZ));
				Camera4.transform.Rotate(new Vector3(float.Parse(cameInof.RotationX),float.Parse(cameInof.RotationY),float.Parse(cameInof.RotationZ))); 	
				numModel++;
				flag4 = true;
			}
		}
	}
}
