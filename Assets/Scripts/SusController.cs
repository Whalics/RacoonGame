using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusController : MonoBehaviour
{
    [Range(0f,10f)] 
    public float susmeter;
    public float susResetDuration;
    public float stomachGrowlInfluence;
    public float quietTrashInfluence;
    public float loudTrashInfluence;
    public float hideInfluence;

    public float tummyTimer = 20;
    public bool growled = false;
    [Tooltip("From left to right starting at 1")]
    public float locationindex;
    public CameraController cameracontroller;

    public bool insideLightOn; //2 sus
    public bool curtainsOpen;
    public bool blindsOpen;
    public bool cameraOn;
    public bool porchlightOn;
    public bool doorSilloutte;
    public bool doorOpen;
    

    void Awake(){
        cameracontroller = GameObject.Find("CameraAnchor").GetComponent<CameraController>();
    }

    void Start()
    {
        ResetTummyTimer();
    }

    void Update()
    {
        if(tummyTimer >= 0)
            DecreaseTummyTimer();
        
        if(susResetDuration > 0)
        susResetDuration-=Time.deltaTime;
        
        if(cameracontroller.hiding)
        LowerSus();

        if(susmeter > 3 && !cameracontroller.hiding){
            susmeter += Time.deltaTime/2;
        }
    }



    public void ResetTummyTimer(){
        growled = false;
        tummyTimer = 20;
    }

    public void DecreaseTummyTimer(){
        tummyTimer -= Time.deltaTime;
        
        if(tummyTimer <= 10 && !growled){
            growled = true;
            IncreaseStomachSus(stomachGrowlInfluence);
            AudioManager.PlaySound("StomachQuiet");
        }

        if(tummyTimer <= 0){
            IncreaseStomachSus(stomachGrowlInfluence);
            AudioManager.PlaySound("StomachLoud");
        }
    }

    public void IncreaseSus(float influence){
        ResetTummyTimer();
        susResetDuration += 0.2f;
        susmeter+=influence/4;
    }

   public void IncreaseStomachSus(float influence){
        susResetDuration += 10;
        susmeter+=influence;
    }

    public void ChooseSusIndex(){
        locationindex = Mathf.Floor(Random.Range(1f,5f));
        Debug.Log(locationindex);
        
        //curtains open
        if(locationindex == 1){
            insideLightOn = true;
        }
        //light on
        if(locationindex == 2){
            curtainsOpen = true;
        }

        if(locationindex == 3){
            blindsOpen = true;
            //AudioManager.PlaySound("DoorOpen");
        }

        if(locationindex == 4){
            cameraOn = true;
            //AudioManager.PlaySound("BlindsOpen");
        }
        //blinds peak
        if(locationindex == 5){
            porchlightOn = true;
        }
        //camera
        if(locationindex == 6){
            doorSilloutte = true;
        }
    }

    public void LowerSus(){
        if(susResetDuration <= 0 && susmeter > 0)
            susmeter-=Time.deltaTime;
    }


}
