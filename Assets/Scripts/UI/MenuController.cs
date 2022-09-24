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
       if(pnl_main != null){
            pnl_main.GetComponent<CanvasGroup>();
            TitleEnter();

       }
    }

    void TitleEnter(){
        Debug.Log("Entered");
        Sequence mySequence = DOTween.Sequence();
        CanvasGroup grp = pnl_main.GetComponent<CanvasGroup>();
        mySequence.AppendInterval(0.4f);
        mySequence.Append(grp.DOFade(1f,1f).SetEase(Ease.OutSine));
    }

    public void StartGame(RectTransform t){
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(0.4f);
        mySequence.Append(t.DOAnchorPos(Vector2.up*-1080,0.8f,false).SetEase(Ease.InOutQuint));
        StartCoroutine(SceneChange("Game", 1f));
    }

    public IEnumerator SceneChange(string scene, float s){
        yield return new WaitForSecondsRealtime(s);
        UnPause();
        SceneManager.LoadScene(scene);
    }

    public void Pause(){
        CanvasGroup grp;
        if(pauseUI != null && isPaused){ 
            UnPause();
        }
        else if(!isPaused && pauseUI != null){
            isPaused = true;
            Time.timeScale = 0f;
            pauseUI.SetActive(true);
            grp = pauseUI.GetComponent<CanvasGroup>();
            grp.DOFade(1f,0.3f).SetEase(Ease.OutSine).SetUpdate(true);
        }
    }

    public void UnPause(){
        CanvasGroup grp;
        if(pauseUI != null){
            pauseUI.SetActive(false);
            grp = pauseUI.GetComponent<CanvasGroup>();
            grp.DOFade(0f,0.3f).SetEase(Ease.OutSine).SetUpdate(true);
            StartCoroutine(UnPause2());
        }
    }
    
    public IEnumerator UnPause2(){
        yield return new WaitForSecondsRealtime(0.3f);
        isPaused = false;
        Time.timeScale = 1f;
    }


    public void MainMenu(RectTransform t){
        Sequence mySequence = DOTween.Sequence();
        mySequence.AppendInterval(0.4f).SetUpdate(true);
        mySequence.Append(t.DOAnchorPos(Vector2.up*-1080,0.8f,false).SetEase(Ease.InOutQuint));
        StartCoroutine(SceneChange("MainMenu",1f));
    }

    public void QuitGame(){
        Application.Quit();
    }  
}
