using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Number : MonoBehaviour {
	// Use this for initialization
	
    public int numberDisplayed ;
	void Start () {
        displayNum(numberDisplayed);
	}
    public void displayNum(int n)
    {
        numberDisplayed = n % 10;
        renderer.material.mainTexture = TextureNumbers.getTexture(numberDisplayed);
        //Debug.Log("Display num " + TextureNumbers.getTexture(numberDisplayed));
    }
	// Update is called once per frame
	void Update () {
	}
}
