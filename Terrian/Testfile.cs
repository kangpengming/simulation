using UnityEngine;
using System.Collections;

public class Testfile : MonoBehaviour
{
	Texture2D texture = null;
	Method method = null;
	// Use this for initialization
	void Start ()
	{
		method = new Method();
		texture = method.ReadTexture2D("Textures/kp2bak");
		print("the texture is "+texture.ToString());
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

