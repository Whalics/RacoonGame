using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuController : MonoBehaviour
{

    public RectTransform pnl_main;
    public Transform lerpTransform;
    public GameObject pauseUI;
    public bool isPaused = false;

    void Start(){
       // pauseUI = GameObject.Find("pnl_Paused");
        TitleEnter(pnl_main);
    }

    void TitleEnter(RectTransform t){
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(0.4f);
        mySequence.Append(t.DOAnchorPos(Vector2.zero,0.8f,false).SetEase(Ease.OutBounce));
    }

    public void StartGame(RectTransform t){
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(0.4f);
        mySequence.Append(t.DOAnchorPos(Vector2.up*-1080,0.8f,false).SetEase(Ease.InOutQuint));
        StartCoroutine(SceneChange("Game", 1f));
    }

    public IEnumerator SceneChange(string scene, float s){
        yield return new WaitForSeconds(s);
        SceneManager.LoadScene(scene);
    }

    public void Pause(){
        if(!isPaused){
            isPaused = true;
            Time.timeScale = 0f;
            pauseUI.SetActive(true);
        }
        else UnPause();
    }

    public void UnPause(){
        pauseUI.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void MainMenu(){
        StartCoroutine(SceneChange("MainMenu",0f));
    }

    public void QuitGame(){
        Application.Quit();
    }  
}
