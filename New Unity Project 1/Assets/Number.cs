using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Number : MonoBehaviour {
	// Use this for initialization
	
    public int numberDisplayed ;
	void Start () {
        TextureNumbers.derp();

        displayNum(5);
	}
    public void displayNum(int n)
    {
        numberDisplayed = n % 10;
        renderer.material.mainTexture = TextureNumbers.getTexture(numberDisplayed);
    }
	// Update is called once per frame
	void Update () {
	}
}
