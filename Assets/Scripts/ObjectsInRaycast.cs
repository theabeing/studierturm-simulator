﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ObjectsInRaycast : MonoBehaviour {

    public Sprite defaultCursor;
    public Sprite interactCursor;

    public Image uiCursor;
    public Texture cursorTexture;
    public Texture actionCursorTexture;
    private Texture currTexture;

    public RaycastHit hits;
    public bool hasObjects;


    public LayerMask interactiveObjectsMask;
    public LayerMask ignoreLayersMask;

    private Rect texRect;

	// Use this for initialization
	void Start () {
        texRect = new Rect(Screen.width * .5f-16, Screen.height * .5f-16, 32, 32);
    }
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width*0.5f, Screen.height*0.5f));

        
        //Debug.DrawRay(ray.origin, ray.direction);
        hasObjects = false;
        // uiCursor.sprite = defaultCursor;
        currTexture = cursorTexture;
        if (Physics.Raycast(ray.origin, ray.direction, out hits, 1.1f, ~ignoreLayersMask.value)) {
            if((hits.collider.gameObject.layer & ~interactiveObjectsMask.value) != 0) {
                hasObjects = true;
                //       uiCursor.sprite = interactCursor;
                currTexture = actionCursorTexture;
            }
        }
        
	}
    
    void OnGUI() {
           GUI.DrawTexture(texRect, currTexture);
    }

    void OnDisable() {
        if(uiCursor != null)
            uiCursor.enabled = false;
    }
    void OnEnable() {
        if (uiCursor != null)
            uiCursor.enabled = true;
    }
}
