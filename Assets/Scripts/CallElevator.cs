﻿using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
public class CallElevator : MonoBehaviour, SlidingDoorListener, ElevatorListener, Interactable {

    public Floor floorOfCallButton;
    public Elevator elevator;
    public Animator animator;

    public AudioClip openingSound;
    public AudioClip closingSound;
    public AudioClip buttonClickSound;
    
    public Renderer callButton;
    public Renderer statusButtonRend;
    public Texture buttonsActiveTex;
    private Texture buttonsInActiveTex;
    private Color buttonsInActiveColor;
    
    private Renderer callButtonRend;
    new private AudioSource audio;

    public Collider doorColliders;

    private bool blinking;
    public float blinkInterval = .3f;
    private float currentBlinkTime;
    private bool interruptToOpen;
    private bool waitingForElevator;

    private bool statusBtnIsOn;
    private bool callBtnIsOn;

    void Start() {
        callButtonRend = callButton.GetComponent<Renderer>();
        audio = GetComponent<AudioSource>();
        foreach (SlidingDoorStateBehaviour behaviour in animator.GetBehaviours<SlidingDoorStateBehaviour>()) {
            behaviour.AddListener(this);
        }
        buttonsInActiveTex = callButtonRend.material.mainTexture;
        buttonsInActiveColor = callButtonRend.material.GetColor("_EmissionColor");
        currentBlinkTime = 0;
        elevator.AddListener(this);
    }

    void Update() {
        if (blinking) {
            currentBlinkTime += Time.deltaTime;
            if (currentBlinkTime >= blinkInterval) {
                currentBlinkTime = 0;
                CallBtnLight(!callBtnIsOn);
            }
        }
    }
    

    public void InterruptClosing() {
        float normTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        animator.Play("Opening", 0, 1 - normTime);
        elevator.InterruptClosing(1-normTime);
        interruptToOpen = true;
    }

    void SlidingDoorListener.DoorClosing() {
        doorColliders.isTrigger = false;
        audio.PlayOneShot(closingSound);
    }

    void SlidingDoorListener.DoorOpening() {
        doorColliders.isTrigger = true;
        waitingForElevator = false;
        audio.PlayOneShot(openingSound);
    }

    void SlidingDoorListener.DoorOpened() {
        blinking = false;
        CallBtnLight(false);

    }

    void SlidingDoorListener.DoorClosed() {

    }


    void ElevatorListener.ElevatorOpening() {
        if (elevator.currentFloor == floorOfCallButton) {
            if (interruptToOpen) {
                interruptToOpen = false;
            }
            else {
                animator.SetTrigger("Open");
            }
        }
    }

    void ElevatorListener.ElevatorClosing() {

    }

    void ElevatorListener.FloorArrived(Floor arrived) {
        if (floorOfCallButton == arrived) {
            StatusBtnLight(true);
            blinking = true;
        }
    }

    void ElevatorListener.FloorPassed(Floor passed) {
        
    }

    void ElevatorListener.ElevatorOpened() {
        
    }

    void ElevatorListener.ElevatorClosed() {
        
    }

    void ElevatorListener.DrivingUp() {
        //Debug.Log("start driving up");
        StatusBtnLight(false);
        
    }

    void ElevatorListener.DrivingDown() {
        //Debug.Log("start driving down");
        StatusBtnLight(false);
    }

    public void Interact(GameObject source) {
        audio.PlayOneShot(buttonClickSound);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Closing")) {
           // Debug.Log("interrupt closing");
            InterruptClosing();
        }
        else if (!waitingForElevator && animator.GetCurrentAnimatorStateInfo(0).IsName("Closed")) {
            CallBtnLight(true);
            elevator.CallToFloor(floorOfCallButton);
            waitingForElevator = true;
        }
    }

    private void StatusBtnLight(bool shouldLit) {
        statusButtonRend.material.mainTexture = shouldLit ? buttonsActiveTex : buttonsInActiveTex;
        statusButtonRend.material.SetColor("_EmissionColor", shouldLit ? Color.white : buttonsInActiveColor);

        statusBtnIsOn = shouldLit;
    }
    private void CallBtnLight(bool shouldLit) {
        callButtonRend.material.mainTexture = shouldLit ? buttonsActiveTex : buttonsInActiveTex;
        callButtonRend.material.SetColor("_EmissionColor", shouldLit ? Color.white : buttonsInActiveColor);
        
        callBtnIsOn = shouldLit;
    }
}
