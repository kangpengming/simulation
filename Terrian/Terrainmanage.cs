using UnityEngine;
using System.Collections;

public class Terrainmanage : MonoBehaviour {
	/**
	 * 首先计算出相机照射的中心点，然后根据中心点计算需要的地图。
	 * */
	//Camera的旋转角度：x 18；y 47;z 5.
	private Camera mainCamera = null;
	private int xRotaion = 18;
	private int yRotation = 47;
	private int zRotation = 5;
	//设定
	private float Range = 1000;
	
	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		//当目标在X轴上旋转的时候，找到此时的旋转中心，使用最长与最短的距离来计算其中心点
		float a = mainCamera.transform.position.y*Mathf.Tan(1);
		float b = mainCamera.transform.position.y*Mathf.Tan(1);
		float x = (a+b)/2 + mainCamera.transform.position.x;
		//当发生第二次旋转的时候，根据在平面划过的距离，然后在相应的坐标点上加上距离得出数据
		float c = Mathf.Sqrt(Mathf.Pow((mainCamera.transform.position.y),2)+Mathf.Pow((mainCamera.transform.position.x-x),2));
		float d = c*Mathf.Tan(1);     
		
		float z = mainCamera.transform.position.z + d;
		
	//	Range =  
	}
}
