using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using EZCameraShake;
using UnityEngine.Audio;

public class CameraController : MonoBehaviour
{
    public Camera cam;
    public float upY = 9f;
    public float downY = 0f;
    public float closeOrtho = 3.7f;
    public float defaultOrtho = 5f;
    public float upOrtho = 6f;
    public bool inTrash = false;
    public bool hiding = false;

	public AudioMixer _mixer;
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

    public bool Hiding => hiding;
    public Camera MainCam => cam;
    public bool CanDigInTrash => inTrash && !hiding;

    private void OnEnable()
    {
        UserInput.LookUp += LookUp;
        UserInput.LookDown += LookDown;
        UserInput.Hide += HideInTrash;
    }

    private void OnDisable()
    {
        UserInput.LookUp -= LookUp;
        UserInput.LookDown -= LookDown;
        UserInput.Hide -= HideInTrash;
    }

    private void Start()
    {
        inTrash = true;
        LookUp();
    }

    public void LookUp()
    {
        if (hiding || !inTrash) return;
        transform.DOMoveY(upY,0.5f).SetEase(Ease.OutExpo);
        cam.DOOrthoSize(upOrtho,0.3f).SetEase(Ease.OutQuad);
        LeftHand.DOMove(lookingLefttHandPos,0.5f).SetEase(Ease.OutExpo);
        RightHand.DOMove(lookingRightHandPos,0.5f).SetEase(Ease.OutExpo);
        LeftHand.DORotate(upLeftRotation,0.5f).SetEase(Ease.OutExpo);
        RightHand.DORotate(upRightRotation,0.5f).SetEase(Ease.OutExpo);
	    inTrash = false;
    }

    private void LookDown()
    {
        if (hiding || inTrash) return;
        
        transform.DOMoveY(downY,0.5f).SetEase(Ease.OutExpo);
        cam.DOOrthoSize(defaultOrtho,0.3f).SetEase(Ease.OutQuad);
        LeftHand.DOMove(defaultLefttHandPos,0.5f).SetEase(Ease.OutExpo);
        RightHand.DOMove(defaultRightHandPos,0.5f).SetEase(Ease.OutExpo);
        LeftHand.DORotate(Vector3.zero,0.5f).SetEase(Ease.OutExpo);
        RightHand.DORotate(Vector3.zero ,0.5f).SetEase(Ease.OutExpo);
        inTrash = true;
    }

    private void HideInTrash(bool hide)
    {
        if (!inTrash) return;
        if(hide)
        {
            AudioManager.PlaySound("TrashLidClose");
            hiding = true;
            darkness.DOFade(0.85f,0.3f).SetEase(Ease.InCirc);
            cam.DOOrthoSize(closeOrtho,0.3f).SetEase(Ease.OutQuad);
            LeftHand.DOMove(leftHidingHandPos,0.5f).SetEase(Ease.OutExpo);
            RightHand.DOMove(rightHidingHandPos,0.5f).SetEase(Ease.OutExpo);
            _mixer.DOSetFloat("Muffle", 0.05f, 0.5f);
            StartCoroutine(ShakeWithDelay());
        }
        else
        {
            AudioManager.PlaySound("TrashLidOpen");
            hiding = false;
            darkness.DOFade(0.1f,0.3f).SetEase(Ease.OutCirc);
            cam.DOOrthoSize(defaultOrtho,0.3f).SetEase(Ease.OutQuad);
            LeftHand.DOMove(defaultLefttHandPos,0.5f).SetEase(Ease.OutExpo);
            RightHand.DOMove(defaultRightHandPos,0.5f).SetEase(Ease.OutExpo);
            _mixer.DOSetFloat("Muffle", 1, 0.5f);
        }
    }

    public IEnumerator ShakeWithDelay(){
        yield return new WaitForSeconds(0.34f);
        CameraShaker.Instance.ShakeOnce(3f,3f,0.1f,0.1f);
    }
}
