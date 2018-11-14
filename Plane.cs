using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
/*
 * 
 * 改变地形的位置，根据飞机的位置进行修改，方法是使用一个临时变量记录飞机上次的位置，然后在update中对位置
 * 进行比较，如果有所变化即对地形的位置进行修改并更新临时变量。这里的位置需要与高度图进行对应、
 * 
 * 注：不需要对地形的位置进行修改，只需要修改生成的plan就ok。
 * 
 * 1.飞行器开始的位置与地图的起始位置相对应。
 * 2.在改变的过程中，横纵向与经纬度一致性。
 * */

/*
 * 摧毁的原理，在此帧内做好标记，但是不会进行摧毁，在下一帧进行的时候进行销毁，所以也需要在下一帧进行检测是否销毁。添加一个flag进行标记是否摧毁
 * */
public class Plane : MonoBehaviour {
	
	
	private float initWidth = 600;
	private bool start = false;
	public float speed  = 1f;
	private Vector2 XZ ;
	
	//临时变量去记录X与Z的位置,只是用来表示飞机对于地形的位置进行了改变，然后对下面的terrainXZ矩阵进行变化，
	int tempX = 0 , tempZ = 0, temp = 0;
	//创建一个矩阵，横数列表示场景中的X方向，纵数列表示Y方向，用来去指代场景中的高度图。
	int[,] terrainXZ = null;
	Vector2[] generatArray = null;
	private TerrainControl terrainControl = null;
	private GameObject mainCamera = null;
	
	private bool flagDesX = false;
	private bool flagDesZ = false;
	private int[] keysX = null;
	private int[] keysZ = null;
	//记录九块地形中生成地形的名称
	private int[] strNameAl = null;
	private int[] strNameLa = null;
	
	private string direFlag;
	
	// Use this for initialization
	void Start () {
		
		Vector3 plan = transform.position;
		tempX  = (int)(plan.x-(int)initWidth/2)/(int)initWidth;
		tempZ =  (int)(plan.z-(int)initWidth/2)/(int)initWidth;
		
		keysX = new int[3];
		keysZ = new int[3];
		terrainXZ = new int[3,3];
		strNameAl = new int[3];
		strNameLa = new int[3];
		
		generatArray = new Vector2[3];
		
		//transform.position = new Vector3(1708.237f,1199.575f,-1248.022f);
		//transform.position = new Vector3(2299f,1120f,2370f);
		for(int k = 0; k < 9; k++){
			terrainXZ[k/3,k%3] = k+1;
		}
		
		Vector3 posPlane = Load.centerPos;
		float x = posPlane.x;
		float z = posPlane.z;
		float xPosFloor = Mathf.Floor(x/initWidth);
		float yPosFloor = Mathf.Floor(z/initWidth);
		
		for(int k = 0; k < 3; k++){
			strNameLa[k] = (int)xPosFloor+k;
			strNameAl[k] = (int)yPosFloor+k;
		}
		
		temp = terrainXZ[1,1];
	}
	
	// Update is called once per frame
	void Update () {

		
		if(Input.GetKey(KeyCode.A)){
			transform.RotateAround(transform.position,transform.forward,30*Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.S)){
			transform.RotateAround(transform.position,transform.forward,-30*Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.W)){
			transform.RotateAround(transform.position,transform.right,30*Time.deltaTime);
		}
		if(Input.GetKey(KeyCode.Z)){
			transform.RotateAround(transform.position,transform.right,-30*Time.deltaTime);
		}
		
		if(start){
			//在这里面删减飞机的运行
			transform.Translate(0,0,speed);
			XZ.x = transform.position.x;
			XZ.y = transform.position.z;
		}
		
		//记录飞机的位置并使用地形的名称进行标记,位置保留的是三位小数，进行地理的经纬度与数值之间的转换，地形的宽长是（3000,3000）；
		Vector3 planePos = transform.position;
		//计算两个方向上的位置变化，因为地形是3000的宽度
		int Xnum = (int)(planePos.x-(int)initWidth/2)/(int)initWidth;
		//int Xrest = (int)(planePos.x-6000)/12000;
		int Znum = (int)(planePos.z-initWidth/2)/(int)initWidth;
		//int Zrest = (int)(planePos.z-6000)/12000;
		if(flagDesX) {
			//在这里填入飞行的方向,传递五个参数，后三个为什么一起填写，效率会高一些
			/*
			 * 参数1：plane的位置
			 * 参数2：需要生成地形的位置
			 * 参数3，4：两个方向的名称
			 * 参数5：飞行方向的标记
			 * */
			terrainControl.generateTerrain(keysX,generatArray,strNameLa,strNameAl,direFlag);
			flagDesX = false;
		}
		if(tempX != Xnum ){
			//这时候向经度方向移动
			//摧毁地形
			mainCamera = GameObject.FindGameObjectWithTag("Sun");
			terrainControl =  mainCamera.GetComponent<TerrainControl>();
			//int[] keysX = new int[3];
			//将矩阵的第一列移至最后一列，否则将最后一列矩阵移至第一列
			if(Xnum > tempX){
				for(int i = 0; i< 3; i++)
					keysX[i] = terrainXZ[0,i];
				
				terrainControl.destroyTerrain(keysX);
				//利用飞机所在的位置求出将要生成的地形的位置
				GameObject posGame = GameObject.Find("plan"+terrainXZ[1,1]);
				Vector3 gamePos = posGame.transform.position;
				float gamePosX = gamePos.x;
				float gamePosZ = gamePos.z;
				Vector2 a1 = new Vector2(gamePosX+initWidth*2,gamePosZ+initWidth);
				generatArray[0] = a1;
				a1.x = gamePosX+initWidth*2;
				a1.y = gamePosZ;
				generatArray[1] = a1;
				a1.x = gamePosX+initWidth*2;
				a1.y = gamePosZ-initWidth;
				generatArray[2] = a1;
				//terrainControl.generateTerrain(keysX,generatArray);
				flagDesX = true;
				
				for(int k = 0;k < 3; k++){
					print("-------------------"+generatArray[k]);
				}
				
				
				for(int k = 0;k < 3; k++)
					strNameLa[k] += 1;
				direFlag = "XG";
				terrainXZ=getMatrix(terrainXZ,"XG");
				//生成地形，但是需要加上地理位置 
			}else{
				for(int i = 0; i< 3; i++)
					keysX[i] = terrainXZ[2,i];
				terrainControl.destroyTerrain(keysX);
				
				//利用飞机所在的位置求出将要生成的地形的位置
				GameObject posGame = GameObject.Find("plan"+terrainXZ[1,1]);
				Vector3 gamePos = posGame.transform.position;
				float gamePosX = gamePos.x;
				float gamePosZ = gamePos.z;
				Vector2 a1 = new Vector2(gamePosX-initWidth*2,gamePosZ+initWidth);
				generatArray[0] = a1;
				a1.x = gamePosX-initWidth*2;
				a1.y = gamePosZ;
				generatArray[1] = a1;
				a1.x = gamePosX-initWidth*2;
				a1.y = gamePosZ-initWidth;
				generatArray[2] = a1;
				//terrainControl.generateTerrain(keysX,generatArray);
				flagDesX = true;
				for(int k = 0;k < 3; k++)
					strNameLa[k] -= 1;
				
				direFlag = "XL";
				terrainXZ=getMatrix(terrainXZ,"XL");
				
				for(int k = 0;k < 3; k++){
					print("22-------------------"+generatArray[k]);
				}
			}				
				//进行位置的移动（销毁并创建地形）			
			tempX = Xnum;
		}
	
	/*---------------------------------------------------------------------------------------------------------*/
		
		if(flagDesZ){
			terrainControl.generateTerrain(keysZ,generatArray,strNameLa,strNameAl,direFlag);	
			flagDesZ = false;
		}
		
		if(tempZ != Znum){
			//这时候向纬度方向移动
			mainCamera = GameObject.FindGameObjectWithTag("Sun");
			terrainControl =  mainCamera.GetComponent<TerrainControl>();
			//int[] keysZ = new int[3];
			if(Znum > tempZ){
				//将矩阵的第一行移至最后一行，否则将最后一行移至第一行
				for(int i = 0; i< 3; i++)
					keysZ[i] = terrainXZ[i,2];
				terrainControl.destroyTerrain(keysZ);
				
				for(int i = 0;i < 3;i++)
					print(keysZ[i]);
				
				
				//利用飞机所在的位置求出将要生成的地形的位置
				GameObject posGame = GameObject.Find("plan"+terrainXZ[1,1]);
				Vector3 gamePos = posGame.transform.position;
				float gamePosX = gamePos.x;
				float gamePosZ = gamePos.z;
				Vector2 a1 = new Vector2(gamePosX-initWidth,gamePosZ+initWidth*2);
				generatArray[0] = a1;
				a1.x = gamePosX;
				a1.y = gamePosZ+initWidth*2;
				generatArray[1] = a1;
				a1.x = gamePosX+initWidth;
				a1.y = gamePosZ+initWidth*2;
				generatArray[2] = a1;
		//		terrainControl.generateTerrain(keysZ,generatArray);
				flagDesZ = true;
				for(int k = 0;k < 3; k++)
					strNameAl[k] -= 1;
				
				direFlag = "ZG";
				terrainXZ=getMatrix(terrainXZ,"ZG");
			}else{
				for(int i = 0; i< 3; i++)
					keysZ[i] = terrainXZ[i,0];
				terrainControl.destroyTerrain(keysZ);
				
				//利用飞机所在的位置求出将要生成的地形的位置
				GameObject posGame = GameObject.Find("plan"+terrainXZ[1,1]);
				Vector3 gamePos = posGame.transform.position;
				float gamePosX = gamePos.x;
				float gamePosZ = gamePos.z;
				Vector2 a1 = new Vector2(gamePosX-initWidth,gamePosZ-initWidth*2);
				generatArray[0] = a1;
				a1.x = gamePosX;
				a1.y = gamePosZ-initWidth*2;
				generatArray[1] = a1;
				a1.x = gamePosX+initWidth;
				a1.y = gamePosZ-initWidth*2;
				generatArray[2] = a1;
				//传递的是下次生成地形的坐标点
		//		terrainControl.generateTerrain(keysZ,generatArray);	
				for(int k = 0;k < 3; k++)
					strNameAl[k] += 1;
				
				direFlag = "ZL";
				flagDesZ = true;	
				terrainXZ=getMatrix(terrainXZ,"ZL");
				
			}		
			/*
			 * 1.计算出在Z方向上对于数值的变化，是增还是减。
			 * 2.找出前一块的地形，在本程序中以“plane”+number的形式进行标记，根据此名称摧毁，
			 * */
			tempZ = Znum;
			
		}
	}
	
	void OnGUI(){
		if(GUI.Button(new Rect(0,0,50,25),"start")){
			start = true;
		}
	}
	
	public Vector2 getXZ(){
		return XZ;
	}
	
	public int[,] getMatrix(int[,] matrix, string str){
		
		int[,]  reMatrix = new int[3,3];
		
		if(str.Equals("XL")){
			for(int i = 0; i< 3;i++){
				for(int k = 0; k < 3; k++){
					if(i == 2){
						reMatrix[0,k] = matrix[2,k];
					}else{
						reMatrix[i+1,k] = matrix[i,k];
					}
				}
			}
		}
		
		if(str.Equals("XG")){
			for(int i = 0 ; i < 3; i++){
				for(int k = 0; k < 3;k++){
					if(i == 0){
						reMatrix[2,k] = matrix[0,k];
					}else{
						reMatrix[i-1,k] = matrix[i,k];
					}
				}
			}
		}
		
		if(str.Equals("ZL")){
			for(int i = 0; i< 3; i++){
				for(int k = 0;k < 3; k++){
					if(i == 0)
						reMatrix[k,2] = matrix[k,0];
					else
						reMatrix[k,i-1] = matrix[k,i];
				}
			}
		}
		
		if(str.Equals("ZG")){
			for(int i = 0; i < 3; i++){
				for(int k = 0; k < 3; k++){
					if( i == 2)
						reMatrix[k,0] = matrix[k,2];
					else
						reMatrix[k,i+1] = matrix[k,i];
				}
			}
		}
		
		return reMatrix;
	}
}
