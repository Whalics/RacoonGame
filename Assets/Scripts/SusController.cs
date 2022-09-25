using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusController : MonoBehaviour
{
    [Range(0f,15f)] 
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
    public bool caught = false;


    public GameObject houseLightsOn;
    public GameObject houseCurtainsOpen;
    public GameObject houseBlindsOpen;
    public GameObject houseCameraOn;
    public GameObject housePorchlightOn;
    public GameObject houseDoorSillouette;
    public GameObject houseDoorOpen;
    

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
        
        if(susResetDuration > 0 && cameracontroller.hiding)
        susResetDuration-=Time.deltaTime;
        
        if(cameracontroller.hiding)
        LowerSus();

        if(susmeter > 3 && !cameracontroller.hiding){
            susmeter += Time.deltaTime/2;
            UpdateSceneSus();
        }

        houseLightsOn.SetActive(insideLightOn);
        
        houseCurtainsOpen.SetActive(curtainsOpen);

        houseBlindsOpen.SetActive(blindsOpen);

        houseCameraOn.SetActive(cameraOn);
        
        housePorchlightOn.SetActive(porchlightOn);

        houseDoorSillouette.SetActive(doorSilloutte);

        houseDoorOpen.SetActive(doorOpen);

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
        if(susResetDuration < 6)
            susResetDuration += 0.2f;
        if(susmeter < 15){
            susmeter+=influence/4;
        }
        else{
            Caught();
        }
        UpdateSceneSus();
    }

   public void IncreaseStomachSus(float influence){
        susResetDuration += 6;
        susmeter+=influence;
        UpdateSceneSus();
    }

    public void UpdateSceneSus(){
        // locationindex = Mathf.Floor(Random.Range(1f,5f));
        // Debug.Log(locationindex);
        
        //curtains open
        if(susmeter >= 1.2f){
            insideLightOn = true;
        }
        //light on
        if(susmeter >= 3.5){
            curtainsOpen = true;
        }
        else
        curtainsOpen = false;

        if(susmeter >= 6.5f){
            blindsOpen = true;
            curtainsOpen = false;
            //AudioManager.PlaySound("DoorOpen");
        }
        else blindsOpen = false;

        if(susmeter >= 9f){
            cameraOn = true;
            blindsOpen = false;
            curtainsOpen = false;
            //AudioManager.PlaySound("BlindsOpen");
        }
        else cameraOn = false;
        //blinds peak
        if(susmeter >= 11f){
            porchlightOn = true;
            cameraOn = false;
        }
        else porchlightOn = false;
        //camera
        if(susmeter >= 13f){
            doorSilloutte = true;
        }
        else doorSilloutte = false;

        if(susmeter >= 15f){
            doorOpen = true;
        }
    }

    public void LowerSus(){
        if(susResetDuration <= 0 && ((susmeter > 0 && !insideLightOn) || (susmeter > 2 && insideLightOn)))
            susmeter-=Time.deltaTime;
    }

    public void Caught(){

    }


}
