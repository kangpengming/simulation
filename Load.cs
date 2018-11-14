using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using LitJson;


/**
 * 1.读取配置文件
 * 2. 用来加载模型
 * */
public class Load : MonoBehaviour {
	
	public string modelName;
	public float xPos;
	public float yPos;
	public float zPos;
	public static Hashtable modelHashProperty = new Hashtable();
	//public static Hashtable cameraProperty = new Hashtable();
	public static Vector3 centerPos = new Vector3();
	public Method method = new Method();
	//文件加载
	public IEnumerator getModel(){
		string	BundleURL = "file:E:/invention/StreamingAssets/"+modelName+".assetbundle";
		using (WWW asset = new WWW(BundleURL)){
			yield return asset;
			AssetBundle bundle = asset.assetBundle;
			//GameObject gameObject = bundle.LoadAll()
			Object object1 = asset.assetBundle.Load(modelName,typeof(GameObject));
			//yield return Instantiate(object1,new Vector3(xPos,yPos,zPos),Quaternion.identity).name = modelName;
			yield return Instantiate(object1,method.getPosition(xPos,zPos,yPos),Quaternion.identity).name = modelName;
			bundle.Unload(false);
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	public string getFiles(string str){
		//读取配置文件
		string filePath = str;
		//FileStream fs = new FileStream(filePath,FileMode.Open);
		//StreamReader sr = new StreamReader(fs);
		StreamReader sr = null;
			sr = File.OpenText(filePath);
		string strLine = "";
		string line;
		while((line = sr.ReadLine()) != null){
			strLine += line;
		}
		return strLine;
	}
	
	//json文件解析
	public void parseFiles(string str){
		float sumx = 0;
		float sumz = 0;
		float sum = 0;
		Model model = JsonMapper.ToObject<Model>(str);
		foreach(Modelpropety info in model.modellist){
			modelHashProperty.Add(info.name,info.centerObject+ "+" +info.waveObject);
			this.modelName = info.name;
			this.xPos = float.Parse(info.lat);
			this.zPos = float.Parse(info.lon);
			this.yPos = float.Parse(info.height);
			Vector3 tempVec = method.getPosition(xPos,zPos,yPos);
			if(info.type.Equals("1")){
				sumx += tempVec.x;
				sumz += tempVec.z;
				sum += 1;
			}
			StartCoroutine(getModel());
		}
		print(sumx+"---------------22222222222------000"+sumz);
		centerPos += centerPos + new Vector3(sumx/sum ,0 , sumz/sum);
		
		/**
		 * 定出出中心位置是（x+6000/12000,y+6000/12000）
		 * 那么图片的信息是：(0,0)->(001,001)  (12000,0)->(001,002)
		 * */
	}
	
}
