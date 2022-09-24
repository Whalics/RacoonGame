using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform camTransform;
    public Camera cam;
    public float upY = 9f;
    public float downY = 0f;
    public float closeOrtho = 3.7f;
    public float defaultOrtho = 5f;
    public float upOrtho = 6f;
    public RacoonGame racoongame;
    public bool inTrash = false;
    public bool hiding = false;

    public Transform LeftHand;
    public Transform RightHand;
    public Vector2 rightHidingHandPos = new Vector2(7.2f,-2.6f);
    public Vector2 leftHidingHandPos = new Vector2(-7.2f,-2.6f);

    public Vector2 defaultRightHandPos = new Vector2(0.25f,0);
    public Vector2 defaultLefttHandPos = new Vector2(-0.25f,0);

    public Vector2 lookingRightHandPos = new Vector2(4.6f,5.6f);
    public Vector2 lookingLefttHandPos = new Vector2(-4.6f,5.6f);
    public Vector3 upLeftRotation = new Vector3(0,0,25f);
    public Vector3 upRightRotation = new Vector3(0,0,-25f);

    public Image darkness;

    void Awake(){
        racoongame = new RacoonGame();
    }

    void Start()
    {
        inTrash = true;
        LookUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LookUp(){
        if(!hiding && inTrash)
        {
            inTrash = false;
            camTransform.DOMoveY(upY,0.5f).SetEase(Ease.OutExpo);
            cam.DOOrthoSize(upOrtho,0.3f).SetEase(Ease.OutQuad);
            LeftHand.DOMove(lookingLefttHandPos,0.5f).SetEase(Ease.OutExpo);
            RightHand.DOMove(lookingRightHandPos,0.5f).SetEase(Ease.OutExpo);
            LeftHand.DORotate(upLeftRotation,0.5f).SetEase(Ease.OutExpo);
            RightHand.DORotate(upRightRotation,0.5f).SetEase(Ease.OutExpo);
        }
    }

    public void LookDown(){
        if(!hiding && !inTrash){
            camTransform.DOMoveY(downY,0.5f).SetEase(Ease.OutExpo);
            cam.DOOrthoSize(defaultOrtho,0.3f).SetEase(Ease.OutQuad);
            LeftHand.DOMove(defaultLefttHandPos,0.5f).SetEase(Ease.OutExpo);
            RightHand.DOMove(defaultRightHandPos,0.5f).SetEase(Ease.OutExpo);
            LeftHand.DORotate(Vector3.zero,0.5f).SetEase(Ease.OutExpo);
            RightHand.DORotate(Vector3.zero ,0.5f).SetEase(Ease.OutExpo);
            inTrash = true;
        }
    }

    public void HideInTrash(InputAction.CallbackContext context)
    {
        if(context.started && inTrash)
        {
            hiding = true;
            cam.DOOrthoSize(closeOrtho,0.4f).SetEase(Ease.OutQuad);
            LeftHand.DOMove(leftHidingHandPos,0.5f).SetEase(Ease.OutExpo);
            RightHand.DOMove(rightHidingHandPos,0.5f).SetEase(Ease.OutExpo);
            darkness.DOFade(0.7f,0.3f).SetEase(Ease.OutExpo);
        }
        else if (context.canceled && inTrash)
        {
            hiding = false;
            darkness.DOFade(0.1f,0.3f).SetEase(Ease.OutExpo);
            cam.DOOrthoSize(defaultOrtho,0.3f).SetEase(Ease.OutQuad);
            LeftHand.DOMove(defaultLefttHandPos,0.5f).SetEase(Ease.OutExpo);
            RightHand.DOMove(defaultRightHandPos,0.5f).SetEase(Ease.OutExpo);
        }
    }
}
