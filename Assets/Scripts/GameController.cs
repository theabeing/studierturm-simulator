﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {


    public GameObject cursorCanvas;
    public static float floorHeight = 3.103f;

	// Use this for initialization
	void Start () {
		Application.targetFrameRate = 60;
        Cursor.visible = false;
        //cursorCanvas.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
