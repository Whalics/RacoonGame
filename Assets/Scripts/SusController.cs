using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

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

    public bool lightSound;
    public bool porchLightSound;
    public bool blindsSound;

    public GameObject houseLightsOn;
    public GameObject houseCurtainsOpen;
    public GameObject houseBlindsOpen;
    public GameObject houseCameraOn;
    public GameObject housePorchlightOn;
    public GameObject houseDoorSillouette;
    public GameObject houseDoorOpen;

    public Transform face;
    public Image img_face;

    public float soundRandomizer;
    

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
        
        if(cameracontroller.hiding && tummyTimer > 0){
            LowerSus();
        }

        if(tummyTimer < 0){
            susmeter+=Time.deltaTime;
        }

        if(susmeter > 3 && !cameracontroller.hiding){
            susmeter += Time.deltaTime/2;
            UpdateSceneSus();
        }

        if(soundRandomizer > 0){
            soundRandomizer-=Time.deltaTime;
        }
        else{
            soundRandomizer = Random.Range(3,12);
            int soundRandom = Random.Range(1,5);
            if(soundRandom == 1){
                AudioManager.PlaySound("Sniff1");
            }
            if(soundRandom == 2)
                AudioManager.PlaySound("Sniff2");
            if(soundRandom == 3)
                AudioManager.PlaySound("Sniff3");
            if(soundRandom == 4)
                AudioManager.PlaySound("Sniff4");
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
            IncreaseStomachSus(stomachGrowlInfluence*2.5f);
            AudioManager.PlaySound("StomachLoud");
        }
    }

    public void IncreaseSus(float influence){
        ResetTummyTimer();
        if(susResetDuration < 4)
            susResetDuration += 0.1f;
        if(susmeter < 15){
            susmeter+=influence/3;
        }
        else{
            Caught();
        }
        UpdateSceneSus();
    }

   public void IncreaseStomachSus(float influence){
        susResetDuration += 3;
        susmeter+=influence;
        UpdateSceneSus();
    }

    public void UpdateSceneSus(){
        // locationindex = Mathf.Floor(Random.Range(1f,5f));
        // Debug.Log(locationindex);
        
        //curtains open
        if(susmeter >= 1.2f){
            insideLightOn = true;
            if(!lightSound){
                AudioManager.PlaySound("Light1");
                lightSound = true;
            }
        }
        //light on
        if(susmeter >= 3.5){
            curtainsOpen = true;
        }
        else{
            curtainsOpen = false;
            blindsSound = false;
        }

        if(susmeter >= 6.5f){
            blindsOpen = true;
            curtainsOpen = false;
            if(!blindsSound){
                blindsSound = true;
                AudioManager.PlaySound("Blinds");
            }
            //AudioManager.PlaySound("DoorOpen");
        }
        else{ blindsOpen = false;
        porchLightSound = false;
        }

        if(susmeter >= 9f){
            cameraOn = true;
            blindsOpen = false;
            curtainsOpen = false;
            //AudioManager.PlaySound("BlindsOpen");
        }
        else{
            cameraOn = false;
            
        }
        //blinds peak
        if(susmeter >= 11f){
            porchlightOn = true;
            cameraOn = false;
            if(!porchLightSound){
                porchLightSound = true;
                AudioManager.PlaySound("Light2");
            }
        }
        else{
            porchlightOn = false;
        }
        //camera
        if(susmeter >= 13f && !doorSilloutte){
            doorSilloutte = true;
            AudioManager.PlaySound("Running");
        }
        else doorSilloutte = false;

        if(susmeter >= 15f && !doorOpen){
            doorOpen = true;
            AudioManager.PlaySound("Running");
            Caught();
        }
    }

    public void LowerSus(){
        if(susResetDuration <= 0 && ((susmeter > 0 && !insideLightOn) || (susmeter > 2 && insideLightOn)))
            susmeter-=Time.deltaTime;
    }

    public void Caught(){
        var mySequence = DOTween.Sequence();
        var mySequence2 = DOTween.Sequence();
        mySequence.Append(img_face.DOFade(1f,0.4f).SetEase(Ease.OutCirc));
        mySequence2.Append(face.DOScale(1f,0.4f).SetEase(Ease.OutCirc));

    }


}
