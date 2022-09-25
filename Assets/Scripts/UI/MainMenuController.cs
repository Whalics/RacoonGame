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
    private CanvasGroup _group;

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
        mySequence.AppendInterval(0.4f);
        mySequence.Append(_group.DOFade(1f,1f).SetEase(Ease.OutSine));
    }

    public void StartGame(){
        var mySequence = DOTween.Sequence();
        mySequence.AppendInterval(0.4f);
        mySequence.Append(pnl_main.DOAnchorPos(Vector2.up*-1080,0.8f,false).SetEase(Ease.InOutQuint));
        StartCoroutine(SceneChange("Game", 1.3f));
    }

    public static IEnumerator SceneChange(string scene, float s){
        yield return new WaitForSecondsRealtime(s);
        SceneManager.LoadScene(scene);
    }

    public void QuitGame(){
        Application.Quit();
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }  
}
