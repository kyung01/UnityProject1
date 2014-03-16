using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class TextureNumbers
{
    public static List<Texture2D> texNums;
	// Use this for initialization
    static TextureNumbers()
    {
        Debug.Log(" UnityNumber Ok Now I am referred");
		texNums = new List<Texture2D> ();
		for (int i = 0; i<= 9; i++)
			texNums.Add(Resources.Load("Numbers/num" + i) as Texture2D);
	}
    public static Texture2D getTexture(int n) { return texNums[n]; }
    public static void derp() { }
}