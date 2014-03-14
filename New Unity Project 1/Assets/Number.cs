using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Number : MonoBehaviour {
	static List<Texture2D> texNums;
	// Use this for initialization
	static Number(){
		texNums = new List<Texture2D> ();
		for (int i = 0; i<= 9; i++)
			texNums.Add(Resources.Load("Numbers/num" + i) as Texture2D);
	}
    public int numberDisplayed ;
	void Start () {
	
	}
    public void displayNum(int n)
    {
        numberDisplayed = n % 10;
        renderer.material.mainTexture = texNums[numberDisplayed];
    }
	// Update is called once per frame
	void Update () {
	}
}
