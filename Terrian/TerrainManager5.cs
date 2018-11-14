using UnityEngine;
using System.Collections;

public class TerrainManager5 : MonoBehaviour {

	//材质和高度
	public Material diffuseMap;
	public Texture2D heightMap;
	
	//定点、UV、索引信息
	private Vector3[] vertives;
	private Vector2[] uvs;
	private int[] triangles;
	
	//生成信息
	private Vector2 size; //地形的长宽
	private float minHeight;
	private float maxHeight;
	private Vector2 segment;
	private float unitH;
		//地形的长与宽
	private float initiatWidth = 600;
	private float initiaLength = 600;
	//描绘地形时候的点的个数
	private int initiaWDot = 253;
	private int initiaHDot = 253;
	//面片
	private GameObject terrain;
	// Use this for initialization
	public Vector3 position; 
	private Vector2 tempPos;
	private bool flag = true;
	void Start () {
		
			Vector3 posPlane = Load.centerPos;
			float x = posPlane.x;
			float z = posPlane.z;
			float xPosFloor = Mathf.Floor(x/initiatWidth);
			float yPosFloor = Mathf.Floor(z/initiaLength);
			tempPos = new Vector2(xPosFloor*initiatWidth,yPosFloor*initiaLength);
			StartCoroutine(getTexture((int)xPosFloor+1,(int)yPosFloor+1));		
		//position = new Vector3(12000,0,12000);
			//SetTerrain(new Vector2(12000,12000));
	}
	
	
	// Update is called once per frame
	void Update () {
		if(heightMap != null && flag){
			SetTerrain(tempPos);
			flag = false;
		}
	}
	
	public void SetTerrain(Vector2 pos){
		position.x = pos.x;
		position.z = pos.y;
		setTerrain(initiatWidth,initiaLength,253,253,0,600);
	}
	
	public void setTerrain(float width,float height,uint segmentX,uint segmentY,int min,int max){
		Init(width,height,segmentX,segmentY,min,max);
		//用来获取mesh的定点坐标
		GetVertives();
		DrawMesh();
	}
	
	//设置地形的宽度，长度，宽度的段数，长度的段数,最低高度，最高高度
	private void Init(float width,float height,uint segmentX,uint segmentY,int min,int max){
		size = new Vector2(width,height);
		maxHeight = max;
		minHeight = min;
		unitH = maxHeight - minHeight;
		segment = new Vector2(segmentX,segmentY);
		if(terrain != null){
			Destroy(terrain);
		}
		terrain = new GameObject();
		terrain.name = "plan5";
		//在这里可以修改生成地形的位置terrain.transform.position = new Vector3(100,1,0);
		terrain.transform.position = position;
	}
	
	//绘制网格
	
	private void DrawMesh(){
		Mesh mesh = terrain.AddComponent<MeshFilter>().mesh;
		terrain.AddComponent<MeshRenderer>();

		
		if(diffuseMap == null){
			Debug.LogWarning("No heightMap");
		}
		terrain.renderer.material = diffuseMap;
		
		//给mesh赋值
		mesh.Clear();
		//得到的是mesh的定点坐标（三维向量）
		mesh.vertices = vertives;
		mesh.uv = uvs;
		mesh.triangles = triangles;
			//	terrain.AddComponent<MeshCollider>();
		//使用shade构建一个材质，并设置材质的颜色
		//Material material = new Material(Shader.Find(""));
		//material.SetColor("_Color",Color.yellow);
		diffuseMap.SetTexture("_Mask",heightMap);
		//重置法线
		//在修改完顶点后，通常会更新法线来反映新的变化。法线是根据共享的顶点计算出来的。导入到网格有时不共享所有的顶点。
		//例如：一个顶点在一个纹理坐标的接缝处将会被分成两个顶点。因此这个RecalculateNormals函数将会在纹理坐标接缝处创建一个不光滑的法线。
		//RecalculateNormals不会自动产生切线，因此bumpmap着色器在调用RecalculateNormals之后不会工作。然而你可以提取你自己的切线。
		mesh.RecalculateNormals();
		//在修改顶点后你应该点用这个函数以确保包围体是恰当的。赋值三角形将自动重新计算这个包围体
		mesh.RecalculateBounds();
	}
	
	//生成定点信息  
	private Vector3[] GetVertives(){
		//向下舍位取整
		int sum = Mathf.FloorToInt((segment.x+1)*(segment.y+1));
		float w = size.x/segment.x;
		float h = size.y/segment.y;
		
		int index = 0;
		GetUV();
		//这里并没有代码执行顺序的问题
		GetTriangles();
		//可以在这里得到树木的空间坐标
		vertives = new Vector3[sum];
		for(int i = 0;i < segment.x+1;i++){
			for(int j = 0;j < segment.y+1;j++){
				float tempHeight = 0;
				if(heightMap != null){
						tempHeight = GetHeight(heightMap,uvs[index]);
							if(i == segment.x)
								tempHeight = GetHeight(heightMap,uvs[index - (int)segment.x]);
							if(j == segment.x && i != segment.x)
								tempHeight = GetHeight(heightMap,uvs[index - 1]);
				}
				if(tempHeight > 586 && tempHeight < 587)
					tempHeight = 125.0282f;
				vertives[index] = new Vector3(j*w,tempHeight,i*h);
				index++;
			}
		}
		return vertives;
	}
	
	//在这里生成贴图的做标矩阵，将贴图的长宽归为1，那么按照mesh长与宽的个数取数据，则与贴图上的数据进行对应。
	private Vector2[] GetUV(){
		int sum = Mathf.FloorToInt((segment.x+1)*(segment.y+1));
		uvs = new Vector2[sum];
		float u = 1.0f/segment.x;
		float v = 1.0f/segment.y;
		uint index = 0;
		
		for(int i = 0; i < segment.y+1;i++){
			for (int j = 0; j < segment.x+1;j++){
				uvs[index] = new Vector2(j*u,i*v);
				index++;
			}
		}
		return uvs;
	}
	
	//生成索引信息，主要就是生成mesh的链接顺序
	private int[] GetTriangles(){
		int sum = Mathf.FloorToInt(segment.x*segment.y*6);
		triangles = new int[sum];
		uint index = 0;
		
		for(int i = 0; i < segment.y;i++){
			for(int j = 0; j < segment.x;j++){
				int role = Mathf.FloorToInt(segment.x)+1;
				int self = j + (i*role);
				int next = j+((i+1)*role);
				//存储的是点的链接顺序
				triangles[index] = self;
				triangles[index+1] = next + 1;
				triangles[index+2] = self + 1;
				triangles[index+3] = self;
				triangles[index+4] = next;
				triangles[index+5] = next + 1;
				index += 6;
			}
		}
		return triangles;
	}
	
	private float GetHeight(Texture2D texture,Vector2 uv){
		if(texture != null){
			Color c = GetColor(texture,uv);
			
			float gray  = c.grayscale;
			float h = unitH*gray;
			return h;
		}else{
			return 0;
		}
	}
	
	
	private Color GetColor(Texture2D texture,Vector2 uv){
		Color color = texture.GetPixel(Mathf.FloorToInt(texture.width*uv.x),Mathf.FloorToInt(texture.height*uv.y));
		return color;
	}
	
	public void SetPos(Vector3 pos){
		if(terrain){
		terrain.transform.position = pos;
		}else{
		//	SetTerrain();
			terrain.transform.position = pos;
		}
	}
	
	//协线程加载资源
	 IEnumerator getTexture(int xPosFloor,int yPosFloor){
		string nameX = generateName(xPosFloor);
		string nameY = generateName(yPosFloor);
			string	BundleURL = "file:D:/invention/StreamingAssets/terrain_"+nameX+"_"+nameY+".assetbundle";
					Texture[] text = null;	
					using (WWW asset = new WWW(BundleURL)){
							yield return asset;
							AssetBundle bundle  = asset.assetBundle;
							Object[] objects = asset.assetBundle.LoadAll();
							text = new Texture[objects.Length];
						for(int m = 0; m < objects.Length ; m++){
							text[m] = objects[m] as Texture;
						}
						bundle.Unload(false);
						yield return new WaitForSeconds(0.1f);
					}
			print(BundleURL);
			heightMap = (Texture2D)text[0];
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
}
