using UnityEngine;
using System.Collections;

public class Camchoice : MonoBehaviour {
	private float Ypos1 = 0f;
	private float Ypos2 = 0f;
	private float Ypos3 = 0f;
	private float Ypos4 = 0f;
	private bool showDropdownButtons1;
	private bool showDropButtonsUP1;
	private float dropSpeed = 400;
	private string floorStr = "radar1";
	private GameObject[] cameras;
	private int numCame = 10;
	private GameObject ObjectAttchnei; 
	private float width = 50;
	private float length = 25;
	private float widthPos = 0;
	private float yOrigin = 75;
	private Load load = null;
	
	void Start(){
		//默认对准的相机为雷达一号
		floorStr = "radar1";
		cameras = new GameObject[numCame];
		showDropdownButtons1 = false;
		//在这里找到所有的相机进行封装
		getCameras();
		//解析数据
	//	GameObject mainCamera = GameObject.FindGameObjectWithTag("Sun");
		
	//	load =  mainCamera.GetComponent<Load>();
		//load = new Load();
		
		
	}
	
	void Update () {
		if (showDropdownButtons1 == true) {
			Ypos1 += Time.deltaTime*dropSpeed;
			Ypos2 += Time.deltaTime*dropSpeed;
			Ypos3 += Time.deltaTime*dropSpeed;
			Ypos4 += Time.deltaTime*dropSpeed;
			if(Ypos1 >= length){
				Ypos1 = length;
			}
			if(Ypos2 >= 2*length){
				Ypos2 = 2*length;
			}
			if(Ypos3 >= 3*length){
				Ypos3 = 3*length;
			}
			if(Ypos4 >= 4*length){
				Ypos4 = 4*length;
			}
			if(showDropButtonsUP1==true){
				Ypos1 -= Time.deltaTime*dropSpeed;
				Ypos2 -= Time.deltaTime*dropSpeed;
				Ypos3 -= Time.deltaTime*dropSpeed;
				Ypos4 -= Time.deltaTime*dropSpeed;
				
				if(Ypos1>=0||Ypos2>=0||Ypos3>=0||Ypos4>=0){
					Ypos1 = 0;
					Ypos2 = 0;
					Ypos3 = 0;
					Ypos4 = 0;
					showDropButtonsUP1 = false;
					showDropdownButtons1 = false;
				}
			} 
		}
	}
	
	void OnGUI(){
		if(showDropdownButtons1 == false){
			if(GUI.RepeatButton(new Rect(widthPos,yOrigin,width,length),floorStr)){
				showDropdownButtons1 = true;
			}
		}
		if(showDropdownButtons1 == true){
			if(GUI.Button(new Rect(widthPos,yOrigin,width,length),"radar1")){
				showDropdownButtons1 = false;
				showDropButtonsUP1 = true;
				floorStr = "radar1";
				ChangeCam(1);
			}
			if(GUI.Button(new Rect(widthPos,Ypos1+yOrigin,width,length),"radar2")){
				showDropdownButtons1 = false;
				showDropButtonsUP1 = true;
				floorStr = "radar2";
				ChangeCam(2);
			}
			if(GUI.Button(new Rect(widthPos,Ypos2+yOrigin,width,length),"radar3")){
				showDropdownButtons1 = false;
				showDropButtonsUP1 = true;
				floorStr = "radar3";
				ChangeCam(3);
			}
			if(GUI.Button(new Rect(widthPos,Ypos3+yOrigin,width,length),"radar4")){
				showDropdownButtons1 = false;
				showDropButtonsUP1 = true;
				floorStr = "radar4";
				ChangeCam(4);
			}
			if(GUI.Button(new Rect(widthPos,Ypos4+yOrigin,width,length),"radar5")){
				showDropdownButtons1 = false;
				showDropButtonsUP1 = true;
				floorStr = "radar5";
				ChangeCam(5);
			}
		}
	}
	
	void ChangeCam(int k ){
	/*	for (int i = 0; i < numCame; i++){
			if(i == k-1){
				Debug.Log(i);
				//cameras[i]
				cameras[i].SetActive(true);
				continue;
			}
			cameras[i].SetActive(false);
		}*/
	}
	//获得场景中的相机并将其放置在一个相机数组中
	void getCameras(){
		
		cameras[0] = GameObject.FindGameObjectWithTag("radar1Camera");
		cameras[1] = GameObject.FindGameObjectWithTag("radar2Camera");
		cameras[2] = GameObject.FindGameObjectWithTag("radar3Camera");
		cameras[3] = GameObject.FindGameObjectWithTag("radar4Camera");
		cameras[4] = GameObject.FindGameObjectWithTag("radar5Camera");
		cameras[5] = GameObject.FindGameObjectWithTag("radar6Camera");
		cameras[6] = GameObject.FindGameObjectWithTag("radar7Camera");
		cameras[7] = GameObject.FindGameObjectWithTag("radar8Camera");
		cameras[8] = GameObject.FindGameObjectWithTag("radar9Camera");
		cameras[9] = GameObject.FindGameObjectWithTag("MainCamera");
	}
}