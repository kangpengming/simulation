using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;

public class TerrainControl : MonoBehaviour {
	private GameObject plan = null;
	private GameObject terrain = null;
	TerrainManager2 terrainmanager = null;
	private int key = 2;
	
	string[] strName = null;
	string str = "0";
	
	// Use this for initialization
	void Start () {
		strName = new string[3];
	}
	
	// Update is called once per frame
	void Update () {
	if(plan == null){
			plan = GameObject.Find("plan"+key);
		}
		
		if(plan != null && Input.GetKey(KeyCode.P)){
			print("detroy the object");
			Destroy(plan);
		}
		
		if(plan == null && Input.GetKey(KeyCode.O)){
			terrain = GameObject.Find("Terrain2");
			terrainmanager = terrain.GetComponent<TerrainManager2>();
			Texture2D texture = (Texture2D)Resources.Load("MapTexture/terrain_001_001");
			print("the control is"+ texture);
			terrainmanager.heightMap = texture;
//			terrainmanager.SetTerrain();
		}
	}
	
	public void destroyTerrain(int[] keys){
		
		for(int i = 0; i < 3;i++){
			plan = GameObject.Find("plan"+keys[i]);
			if(plan != null){
				Destroy(plan);
			}
		}
	}
	
	/*
	 * 参数1：记录生成地形的中间地形矩阵中的数字
	 * 参数2：记录需要生成地形的位置坐标
	 * 参数3：记录生成地形的方向，主要与需要读取的贴图文件有关。
	 * */
	public void generateTerrain(int[] keys,Vector2[] posVec, int[] nameX, int[] nameZ, string direFlag){
		string strTemp = "";
		switch (direFlag){
			case "XG":
				strTemp = generateName(nameX[2]);
				for(int k = 0;k < 3; k++ ){
					strName[k] = "terrain_" + generateName(nameZ[k]) +  "_"+strTemp ;
				}				
				break;
			case "XL":
				strTemp = generateName(nameX[0]);
				for(int k = 0;k < 3; k++ ){
					strName[k] = "terrain_" + generateName(nameZ[k])+"_"+strTemp ;
				}
				break;
			case "ZG":
				strTemp = generateName(nameZ[0]);
				for(int k = 0;k < 3; k++ ){
					strName[k] = "terrain_" + strTemp + "_" + generateName(nameX[k]);
				}				
				break;
			case "ZL":
				strTemp = generateName(nameZ[2]);
				for(int k = 0;k < 3; k++ ){
					strName[k] = "terrain_" + strTemp+"_"+ generateName(nameX[k]);
				}
				break;
		}
		
		
	StartCoroutine(DownloadAsset(keys,posVec));
	}
	
	public string generateName(int key){
		
		string str = null;
		if(key/10 == 0)
			str = "00" + key;
		else if(key/100 == 0)
			str = "0" + key;
		else 
			str = ""+key;
		return str;
	
	}
	
	
		IEnumerator DownloadAsset(int[] keys,Vector2[] posVec){
		
		for(int i = 0; i < 3; i++){
			plan = GameObject.Find("plan"+keys[i]);                                                                                            
			if(plan == null){
				terrain = GameObject.Find("Terrain"+keys[i]);
				//terrain = GameObject.Find("Terrain1");
				MonoBehaviour[] monos = terrain.GetComponents<MonoBehaviour>();
				for(int k = 0; k < monos.Length; k++){
					if(monos[k].ToString().Contains("TerrainManager")){
						//这里面的设计需要将文件进行按照“名称”进行寻找texture。						
					string	BundleURL = "file:E:/invention/StreamingAssets/"+strName[i]+".assetbundle";
						str = strName[i];					
						Texture[] text = null;	
						using (WWW asset = new WWW(BundleURL)){
								yield return asset;
								AssetBundle bundle  = asset.assetBundle;
								Object[] objects = asset.assetBundle.LoadAll();
								text = new Texture[objects.Length];
							for(int m = 0; m < objects.Length ; m++){
								text[m] = objects[m] as Texture;
							} 
							//Instantiate(bundle.Load("Cube"));
							bundle.Unload(false);
							yield return new WaitForSeconds(0.1f);
						} 
						//获取方法并执行
						monos[k].GetType().GetField("heightMap").SetValue(monos[k],text[0]); 
						MethodInfo method =  monos[k].GetType().GetMethod("SetTerrain");
						method.Invoke(monos[k],new object[]{posVec[i]});
						break;
					}
				}
			}
		}
	}
}
