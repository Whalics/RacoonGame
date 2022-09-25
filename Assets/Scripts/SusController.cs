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
            IncreaseSus(stomachGrowlInfluence);
            AudioManager.PlaySound("StomachQuiet");
        }

        if(tummyTimer <= 0){
            IncreaseSus(stomachGrowlInfluence);
            AudioManager.PlaySound("StomachLoud");
        }
    }

    public void IncreaseSus(float influence){
        ResetTummyTimer();
        susResetDuration = 2;
        susmeter+=influence/2;
    }

    public void ChooseSusIndex(){
        locationindex = Mathf.Floor(Random.Range(1f,5f));
        Debug.Log(locationindex);
    }

    public void LowerSus(){
        if(susResetDuration <= 0 && susmeter > 0)
            susmeter-=Time.deltaTime;
    }
}
