using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour {
	
	public GameObject target;
	public GameObject missile;
	private const float Max_Rotation = 180;
	//private static float Max_Rotation_frame = Max_Rotation/((float)(Application.targetFrameRate == -1 ? 60 : Application.targetFrameRate));
	private Quaternion roation;
	private float speed = 0;
	private float rotationSpeed = 0;
	private float flyTime ;
	private List<Vector3> listPos;
	private List<float> listTime;
	private float tempTime= 0;
	private Vector3 targetPos;
	private bool flag;
	private bool flagRadar;
	private float RadarRoatSpeed = 0;
	private float roateRadarTime = 0;
	
	public static List<string> listName;
	public static string nameModel;
	public static List<string> listType;
	public static List<string> listWave;
	public static List<float> listARadius;
	public static List<float> listBRadius;
	public static List<string> listNameSatus;
	
	public static List<Vector3> listPosRadar;
	public static List<float> listTimeRadar;
	public static List<string> listNameRadar;
	public static List<string> listTypeRadar;
	public static List<string> listWaveRadar;
	public static List<float> listARadiusRadar;
	public static List<float> listBRadiusRadar;
	public static List<string> listNameRadarSatus;
	
	public static Hashtable tableTime;
	public static Hashtable tablePos;
	public static Hashtable tableFlage;
	public static Hashtable tableSpeed;
	public static Hashtable tableRotationSpeed;
	
	public static Hashtable tableAngle;
	public static Hashtable tableRadarTime;
	public static Hashtable tableRadarRotateSpeed;
	
	public static Hashtable modelHashProperty;
	
	void Start () {
		nameModel = "2";
		missile = GameObject.Find("2");
		//missile = transform.gameObject;
		target = GameObject.FindGameObjectWithTag("missile");
		
		listNameRadarSatus = new List<string>();
		listNameSatus = new List<string>();
		
		tablePos = new Hashtable();
		tableTime = new Hashtable();
		tableFlage = new Hashtable();		
		tableSpeed = new Hashtable();
		tableRotationSpeed = new Hashtable();
		
		tableAngle = new Hashtable();
		tableRadarTime = new Hashtable();
		tableRadarRotateSpeed = new Hashtable();
		modelHashProperty = new Hashtable();
		
		listPos = SocketClient.listPos;
		listTime = SocketClient.listTime;
		listName = SocketClient.listName;
		listType = SocketClient.listType;
		listWave = SocketClient.listWave;
		listARadius = SocketClient.listARadius;
		listBRadius = SocketClient.listBRadius;
		
		listPosRadar = SocketClient.listPosRadar;
		listTimeRadar = SocketClient.listTimeRadar;
		listNameRadar = SocketClient.listNameRadar;
		listTypeRadar = SocketClient.listTypeRadar;
		listWaveRadar = SocketClient.listWaveRadar;
		listARadiusRadar = SocketClient.listARadiusRadar;
	 	listBRadiusRadar = SocketClient.listBRadiusRadar;
		
		modelHashProperty = Load.modelHashProperty;
		
		flag = true;
		flagRadar = true;
	}
	
	void Update () {
		/**
		 * 目标的运动方式:
		 * 1.首先对目标的初始化，当开始接收数据的时候，物体的速度是0，但是如何判断物体的速度是0？为什么判断，首先考虑特殊情况，每一次都是此次的数据减去上一次的数据，得到距离与时间，获得物体的速度。
		 * 但是初始时刻，首先获取目标的位置，与接收到的数据进行决策，因此第一次的飞行时不同的。回归如何判断，最好的方式是对已经进行过第一次试验的目标进行收集，当不存在容器中的时候，便进行第一次决策。
		 * 2.同理对速度的收集，因为速度与角速度是对物体运动进行描述，每个新物体的运动都会对产生新的速度，会影响下一次该物体的运动，因此要对速度进行保存，使相应的物体直接对应到相应的速度上。
		 * */
		if( listPos != null && listPos.Count > 3){
				nameModel = listName[0];
				missile = GameObject.Find(nameModel);
			if( !listNameSatus.Contains(nameModel) && listPos.Count > 3){
				listNameSatus.Add(nameModel);
				tablePos.Add(nameModel,listPos[0]);
				flyTime = 2;
				tableTime.Add(nameModel,listTime[0]);
				tempTime = listTime[0];
				flag = false;
				roation = getRotation((Vector3)tablePos[nameModel]);
				rotationSpeed = Quaternion.Angle(missile.transform.rotation,roation)/flyTime;
				tableRotationSpeed.Add(nameModel,rotationSpeed);
				speed = Vector3.Distance(missile.transform.position,(Vector3)tablePos[nameModel])/flyTime;
				tableSpeed.Add(nameModel,speed);
				deleteData();
			}
			
			if(Vector3.Distance(missile.transform.position,(Vector3)tablePos[nameModel]) < 30)
			{
				if (listPos.Count > 3 && flag){
					tablePos[nameModel]=listPos[0];
					flyTime = listTime[0] - tempTime;
					tempTime = listTime[0];
					flag = false;
					roation = getRotation((Vector3)tablePos[nameModel]);
					rotationSpeed = Quaternion.Angle(missile.transform.rotation,roation)/flyTime;
					tableRotationSpeed.Add(nameModel,rotationSpeed);
					speed = Vector3.Distance(missile.transform.position, (Vector3)tablePos[nameModel])/flyTime;
					tableSpeed[nameModel]=speed;
					deleteData();
				}
			}else
				flag = true;
			//missile.transform.rotation = Quaternion.Slerp(missile.transform.rotation,roation,Time.deltaTime*rotationSpeed);
			missile.transform.rotation = Quaternion.Slerp(missile.transform.rotation,roation,Time.deltaTime*(float)tableSpeed[nameModel]);
			//missile.transform.Translate(new Vector3(0,0,Time.deltaTime*speed));
			missile.transform.Translate(new Vector3(0,0,Time.deltaTime*(float)tableSpeed[nameModel]));
		}
		
		/*
		 * 雷达的运动方式
		 * 将所有以transform打头的物体换成旋转中心
		 * */
		if(listPosRadar != null && listPosRadar.Count > 3){
			
			nameModel = listNameRadar[0];
			string waveCenter = (string)modelHashProperty[nameModel];
			string[] waveCenters = waveCenter.Split(new char[1]{'+'});
			GameObject wave = GameObject.Find(waveCenters[1]);
			GameObject center = GameObject.Find(waveCenters[0]);
			
			if(!listNameRadarSatus.Contains(nameModel) && listPosRadar.Count > 3){
				listNameRadarSatus.Add(nameModel);
				tableAngle.Add(nameModel,listBRadiusRadar[0]);
				tableRadarTime.Add(nameModel,listTimeRadar[0]);
				float tempAngle = getAngleRadar(wave.transform.position,center.transform.position);
				roateRadarTime = 2;
				flagRadar = false;
				if(listBRadiusRadar[0] >= tempAngle)
					RadarRoatSpeed = (listBRadiusRadar[0] - tempAngle)/roateRadarTime;
				else
					RadarRoatSpeed = ( 360 - listBRadiusRadar[0] + tempAngle)/roateRadarTime;
				tableRadarRotateSpeed.Add(nameModel,RadarRoatSpeed);
				deleteDataRadar();
			}
			
			if(getAngleRadar(wave.transform.position,center.transform.position)-(float)tableAngle[nameModel] < 2){
				if(flagRadar && listPosRadar.Count > 3){
					tableAngle.Add(nameModel,listBRadiusRadar[0]);
					roateRadarTime = listTimeRadar[0]-(float)tableRadarTime[nameModel];
					float tempAngle = getAngleRadar(wave.transform.position, center.transform.position);
					if(listBRadiusRadar[0] >= tempAngle)
						RadarRoatSpeed = (listBRadiusRadar[0] - tempAngle)/roateRadarTime;
					else
						RadarRoatSpeed = ( 360 - listBRadiusRadar[0] + tempAngle)/roateRadarTime;
					tableRadarRotateSpeed.Add(nameModel,RadarRoatSpeed);
					deleteDataRadar();
				}
			}else
				flagRadar = true;
			
			wave.transform.RotateAround(center.transform.position,center.transform.forward,(float)tableRadarRotateSpeed[nameModel] * Time.deltaTime);
			
		}
	}
	
	/*
	 * 用来计算物体到目标位置的旋转的角度
	 * */
	public Quaternion getRotation(Vector3 targetPos1){
		print("---------------- targetPos1.x"+ targetPos1.x);
		print("---------------- missile.transform.position.x"+ missile.transform.position.x);
		float dx = targetPos1.x - missile.transform.position.x;
		float dy = targetPos1.y - missile.transform.position.y;
		float dz = targetPos1.z - missile.transform.position.z;
		
		float rotationTheta = Mathf.Atan(dx/dz)*180/Mathf.PI;
		float dxz = Mathf.Sqrt(dz*dz+dx*dx);
		float rotattionPhi = Mathf.Atan(dy/dxz) * 180/Mathf.PI;
		rotattionPhi = -rotattionPhi;
		return Quaternion.Euler(rotattionPhi,rotationTheta,0);
	
	}
	void OnGUI(){
		GUI.Label(new Rect(0,100,100,50),speed+"");
	}
	
	//得到雷达的方位角
	public float getAngleRadar(Vector3 targetPos1,Vector3 targetPos2){
		float rotationTheta = 0;
		float dx = targetPos1.x - targetPos2.x;
		float dy = targetPos1.y - targetPos2.y;
		float dz = targetPos1.z - targetPos2.z;

		if(dz == 0){
			if(dx > 0) rotationTheta = 90;
			else rotationTheta = 270;
		}else if(dx == 0){
			if(dz > 0) rotationTheta = 0;
			else rotationTheta = 180;
		}else if(dx > 0 && dz > 0)
			rotationTheta = Mathf.Atan(dx/dz)*180/Mathf.PI;
		else if(dx > 0 && dz < 0) 
			rotationTheta = Mathf.Atan(dx/dz)*180/Mathf.PI + 180;
		else if(dx < 0 && dz < 0)
			rotationTheta = Mathf.Atan(dx/dz)*180/Mathf.PI + 180;
		else if( dz < 0 && dx < 0)
			rotationTheta = Mathf.Atan(dx/dz)*180/Mathf.PI + 360;
		
		return rotationTheta;
	}
	
	public void deleteData(){
		listPos.RemoveAt(0);
		listTime.RemoveAt(0);
		listName.RemoveAt(0);
		listType.RemoveAt(0);
		listWave.RemoveAt(0);
		listARadius.RemoveAt(0);
		listBRadius.RemoveAt(0);
	}
	
	public void deleteDataRadar(){
		listPosRadar.RemoveAt(0);
		listTimeRadar.RemoveAt(0);
		listNameRadar.RemoveAt(0);
		listTypeRadar.RemoveAt(0);
		listWaveRadar.RemoveAt(0);
		listARadiusRadar.RemoveAt(0);
		listBRadiusRadar.RemoveAt(0);
	}


}
