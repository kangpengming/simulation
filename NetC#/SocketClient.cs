using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;

/**
 * 网络编程：网路编程原理都是一样的，发射端(可以是服务器或者客户端)：指定ip地址与端口号，然后将将要传输的数据转化成流的形式，利用TCP或者UDP进行打包进行传输，或者直接使用socket套接字进行传输，UDP与TCP是封装socket的。
 * 接收端（可以使服务器或者客户端）：指定需要监听的端口，在这个端口持续不断的进行监听，（注意：这也是一个操作也就也会占用一个线程），当数据流传到端口的时候，接受数据流然后将其传入字节，最后转化成想要的数据，完成传输。
 * 
 * 上面提到了端口的监听也会占用一个线程。但是程序中规定主界面占用主线程也就是UI线程，当UI界面长时间不工作的时候会造成界面的崩溃，所以对于一些耗时的操作需要另外开辟一个线程，例如下载。但是这里面会涉及到一个问题，如何
 * 将接受的数据返回，因为这是两个线程，线程之间的数据时不进行通信的，如果两个线程同时操作一个数据，会造成数据不一致的现象，因此会有了数据锁这个概念，就是防止两个线程同时访问一个数据。
 * 
 * C#也写的不多，也不是写C#的。
 * 网上的查找，看了几个数据返回的方法：
 * （1）直接共享内存，设置一个全局U3D变量。在U3D中是不允许实例的访问，例如transform、texture等，但是可以访问变量，float、int、Vector3等
 * （2）利用invole方法刷新界面，这个方法是在主线程结束之后进行刷新，类似于”串行“。还有一个类似的，beginInvoke方法是”并行“进行的，然后使用EndInvoke返回新线程的数据。但是这个方法却无法进行端口的监听，无法网络编程。
 * 还有其他的方法，我们这里面只用到了第一种，方便简单，其余的喜欢的自己参考。
 * */

public class SocketClient : MonoBehaviour {
	
	
	// Use this for initialization
	private Thread thread = null;
	private Socket netsocket = null;
	private IPFile ipfiles = null;
	private UdpClient client = null;
	private Transport transport = null;
	private Method method = null;
	private static string dataInfo = null;
	private string dataTemp = null;
	bool reciflag = false;
	bool isConnecting = false;
	private static bool flagFirst = false;
	private string btThreadstr = "运行";
	private string isConntString = "暂停";
	//目标的运动参数
	public static List<Vector3> listPos;
	public static List<float> listTime;
	public static List<string> listName;
	public static List<string> listType;
	public static List<string> listWave;
	public static List<float> listARadius;
	public static List<float> listBRadius;
	//雷达的运动参数
	public static List<Vector3> listPosRadar;
	public static List<float> listTimeRadar;
	public static List<string> listNameRadar;
	public static List<string> listTypeRadar;
	public static List<string> listWaveRadar;
	public static List<float> listARadiusRadar;
	public static List<float> listBRadiusRadar;
	//string 
	
	void Start(){
		
		listPos = new List<Vector3>();
		listTime = new List<float>();
		listName = new List<string>();
		listType = new List<string>();
		listWave = new List<string>();
		listARadius = new List<float>();
		listBRadius = new List<float>();
		
		listPosRadar = new List<Vector3>();
		listTimeRadar = new List<float>();
		listNameRadar = new List<string>();
	 	listTypeRadar = new List<string>();
	 	listWaveRadar = new List<string>();
		listARadiusRadar = new List<float>();
		listBRadiusRadar = new List<float>();
	//	listTime.Add(200);
		method = new Method();
		ipfiles = new IPFile();
		transport = new Transport();
		thread = new Thread(RecieveMessage);
		thread.IsBackground = true;
		this.client = new UdpClient(transport.getisRadar1Fusion()[1]);
	}
	void Update(){
		
		if(dataInfo != null){
				string[] paraNumber = dataInfo.Split(new char[1]{'+'});	
				Debug.Log(dataInfo);
				//在这里对雷达与目标进行区分。
				float lat = float.Parse(paraNumber[3]);
				float lon = float.Parse(paraNumber[4]);
				float height = float.Parse(paraNumber[5]);
				float timeTemp = float.Parse(paraNumber[6]);
				float aRadius = float.Parse(paraNumber[8]);
				float bRadius = float.Parse(paraNumber[9]);
				string nameModel = paraNumber[1];
				string typeModel = paraNumber[2];
				string waveModel = paraNumber[7];
				dataInfo = null;
				if(typeModel.Equals("1")){
					listPos.Add(method.getPosition(lat,lon,height));
					listTime.Add(timeTemp);
					listARadius.Add(aRadius);
					listBRadius.Add(bRadius);
					listName.Add(nameModel);
					listType.Add(typeModel);
					listWave.Add(waveModel);
				}else{
					listPosRadar.Add(method.getPosition(lat,lon,height));
					listTimeRadar.Add(timeTemp);
					listARadiusRadar.Add(aRadius);
					listBRadiusRadar.Add(bRadius);
					listNameRadar.Add(nameModel);
					listTypeRadar.Add(typeModel);
					listWaveRadar.Add(waveModel);
			}
			
			for(int i = 0; i< listPos.Count;i++)
				print(listPos[i]);

		}		 
	}
	
	void OnGUI(){
		
		
		if(GUI.Button(new Rect(50,0,100,25),"开始/暂停")){
			
			if(Time.timeScale == 1)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
			
		}
		
		if(GUI.Button(new Rect(0,25,50,25),isConntString)){
			
			//另一端的传输地址与端口
			//IPAddress iplistener = IPAddress.Parse(ipfiles.getRadar1IP());
			IPAddress iplistener = IPAddress.Parse("127.0.0.1");
			IPEndPoint ip_reciver = new IPEndPoint(iplistener,transport.getisRadar1Fusion()[1]);
			this.client.Connect(ip_reciver);
			if(!isConnecting){
			isConnecting = true;
			sendMessage("1 1");
			isConntString = "断开";
				
			}else{	
			isConnecting = false;
			sendMessage("1 0");
			isConntString = "连接";
			}
		}
		
		
		if(GUI.Button(new Rect(0,50,50,25),btThreadstr)){
			if(reciflag == false){
			//启动数据传输的线程
			reciflag = true;
			thread.Start();
			btThreadstr =  "暂停";
			}else{
				//关闭数据传输的线程
			reciflag  = false;
			thread.Abort();
			btThreadstr = "运行";
			}
		}		
	}
	
	
	void OnDestroy(){
		if(thread != null)
			thread.Abort();
		
		if (netsocket != null)
			netsocket.Close();
	}
	
	//sendMessage to Server
	void sendMessage(string str){
		//因为matlab程序是利用fscanf接收数据，而这种接收数据的形式是asiic码值进行解码的。但是C#封装的一些简单的函数并不是asiic值（或许不准确），它会自动编码成4四字节的编码。接收方会根据接收的数据类型对asiic码值进行过滤
		//所以在传输非数字字符串的时候会将其过滤掉,在这里面使用“空格”会在MATLAB接收方直接转换为矩阵：matlab接收形式是data = fscanf(t,'%f\n');
		try{
			byte[] data= System.Text.Encoding.ASCII.GetBytes(str);
			//byte[] data= System.Text.Encoding.ASCII.GetBytes(str);
			this.client.Send(data,data.Length);
			Debug.Log(str);
			
		}catch(Exception e){
			Debug.Log(e.Message);
			Debug.Log("发送消息失败，请重新发送");
		}
	}
	
	
	//recieve message from server
	public void RecieveMessage(){
		
		IPAddress iplistener = IPAddress.Parse(ipfiles.getGenerateResultIP());
		//监听端口1
		IPEndPoint ip_reciver = new IPEndPoint(iplistener,transport.getRadar1Results()[1]);
		netsocket = new Socket(AddressFamily.InterNetwork,SocketType.Dgram,ProtocolType.Udp);
		netsocket.Bind(ip_reciver);
		EndPoint point = (EndPoint)ip_reciver;
	
		
		while(true){
			byte[] data = new byte[1024];
			int recv = netsocket.ReceiveFrom(data,ref point);
			float[] paramterRadar  = method.byteTofloat(data,recv);
			for(int i = 0; i < 9;i++){
				dataInfo += "+" + paramterRadar[i];
			}

		}
//		netsocket.Close();
	}
	
	
	//数据测试
	public void getPos(){
		
		for(int i =0 ; i < 3; i++){
			System.Random rd = new System.Random();
			float xGain = rd.Next(100,200);
			//float yGain = rd.Next(100,200);
			float yGain = 0;
			float zGain = rd.Next(100,200);
			float time = rd.Next(2,4); 
			float v2 = listTime[listTime.Count-1];
			Vector3 v1 = listPos[listPos.Count-1];
			listPos.Add(new Vector3(v1.x+xGain,v1.y+yGain,v1.z+zGain));
			listTime.Add(time+v2);
			if(listPos.Count > 3)
				break;
		}
	} 


}


