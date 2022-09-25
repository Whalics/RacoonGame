using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenuController : MonoBehaviour
{
    public RectTransform pnl_main;
    public Image pnl_black;
    private CanvasGroup _group;
    public GameObject tutorial;

    private void Awake() {
        _group = pnl_main.GetComponent<CanvasGroup>();
    }

    private void Start() { 
        StartCoroutine(AudioManager.PlayFade("Cicades",4f,0f,0.5f));
        Time.timeScale = 1; 
        TitleEnter();
        
    }

    private void TitleEnter(){
        Debug.Log("Entered");
        _group.alpha = 0;
        var mySequence = DOTween.Sequence();
        var mySequence2 = DOTween.Sequence();
        mySequence.AppendInterval(0.4f);
        mySequence.Append(_group.DOFade(1f,1f).SetEase(Ease.OutSine));
        mySequence2.Append(pnl_black.DOFade(0f,1f).SetEase(Ease.InOutQuint));
        
    }

    public void StartGame(){
        AudioManager.PlaySound("Paper1");
        var mySequence = DOTween.Sequence();
        mySequence.AppendInterval(0.1f);
        mySequence.Append(pnl_black.DOFade(1f,0.5f).SetEase(Ease.OutSine));
        StartCoroutine(SceneChange("Game", 0.6f));
    }

    public static IEnumerator SceneChange(string scene, float s){
        yield return new WaitForSecondsRealtime(s);
        SceneManager.LoadScene(scene);
    }

    public void Tutorial(){
        tutorial.SetActive(true);
        AudioManager.PlaySound("Paper1");
        var mySequence = DOTween.Sequence();
        var mySequence2 = DOTween.Sequence();
        mySequence.AppendInterval(0.4f);
        mySequence2.AppendInterval(0.6f);
        mySequence.Append(_group.DOFade(0f,0.5f).SetEase(Ease.OutSine));
        mySequence2.Append(pnl_black.DOFade(1f,0.5f).SetEase(Ease.OutSine));
        
        mySequence.Append(tutorial.GetComponent<Image>().DOFade(1f,0.4f).SetEase(Ease.OutSine));
         mySequence2.Append(pnl_black.DOFade(0f,0.5f).SetEase(Ease.OutSine));
        //mySequence.AppendInterval(0.4f);
        //mySequence.Append(tutorial.GetComponent<Image>().DOFade(1f,0.5f).SetEase(Ease.OutSine));
    }

    public void QuitGame(){
        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }  
}
