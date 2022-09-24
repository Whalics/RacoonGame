using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SusController : MonoBehaviour
{
    [Range(0f,10f)] 
    public float susmeter;

    public float stomachGrowlInfluence;
    public float quietTrashInfluence;
    public float loudTrashInfluence;
    public float hideInfluence;

    public float tummyTimer = 20;
    public bool growled = false;
    [Tooltip("From left to right starting at 1")]
    public float locationindex;


    void Start()
    {
        ResetTummyTimer();
    }

    void Update()
    {
        if(tummyTimer >= 0)
            DecreaseTummyTimer();
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
            IncreaseSus(stomachGrowlInfluence*2);
            AudioManager.PlaySound("StomachLoud");
        }
        
    }

    public void IncreaseSus(float influence){
        susmeter+=influence;
    }

    public void ChooseSusIndex(){
        locationindex = Mathf.Floor(Random.Range(1f,5f));
        Debug.Log(locationindex);
    }

}
