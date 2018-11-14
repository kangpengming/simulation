using UnityEngine;
using System.Collections;
using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;


public class Method{	
	//地球的半径
	static  double EARTH_RADIUS = 6371f;
	private string returnStr = null;
	
	public float[] byteTofloat(byte[] datafloat,int length){

		byte[] bytedata = new byte[1024];
		string temp = null;
		float[] datatemp = new float[length];
		//用来记录数据的个数
		int m = 0;
		//用来记录存储在返回数组中数据的个数
		int dataNum = 0;
		for (int k = 0; k < length;k++){
			bytedata[m] = datafloat[k];
			m++;
			if(datafloat[k] == 46){
				for(int l = 0;l < 6;l++){
					bytedata[m]=datafloat[k+l+1];
					m++;
				}
				temp = System.Text.Encoding.ASCII.GetString(bytedata);
				datatemp[dataNum] = float.Parse(temp);
				dataNum++;
				bytedata = new byte[1024];
				m = 0;
				k=k+6;
				}
			}
		
		return datatemp;
	}
	
	//将经纬度转化成在距离上相对大小(X,Y坐标),经度、纬度(lo)、高度
	public Vector3 getPosition(double la, double lo, double height){
		Vector3 ret = new Vector3();
		
		double initLa = 31;
		double initLo = 116;
		
		//经纬度转化为弧度
		double lat1 = ConvertDegreeToRadius(initLa);
		double lon1 = ConvertDegreeToRadius(initLo);
		double lat2 = ConvertDegreeToRadius(la);
		double lon2 = ConvertDegreeToRadius(lo);
		
		double vlon = Math.Abs(lon2 - lon1);
		double vlat = Math.Abs(lat2 - lat1);
		
		float dis_lon =(float)(2 * EARTH_RADIUS * Math.Sin(vlat/2));
		float dis_lat =(float)(2 * EARTH_RADIUS * Math.Sin(vlon/2) * Math.Cos(lon2));
		ret.x = dis_lon*6;
		ret.z = -dis_lat*6;
		ret.y = (float)height;
		return ret;
	}
	
	//将角度转化为弧度
	private static double ConvertDegreeToRadius(double degree){
		return degree * Math.PI/180;
	
	}
	
	//将弧度转化为角度
	private static double ConvertRadiusToDegree(double radius){
		return radius * 180/Math.PI; 	
	}
	
	//从本地读取纹理与材质，注意，参数是路径：以直接路径为主
	public Texture2D ReadTexture2D(string path){
		Texture2D texture =  (Texture2D)Resources.Load(path);
		return texture;
	}
	
	public Material ReadMaterial(string path){
		Material material = Resources.Load(path,typeof(Material))as Material;
		return material;
	}
}