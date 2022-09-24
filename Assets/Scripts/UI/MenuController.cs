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
        yield return new WaitForSeconds(s);
        SceneManager.LoadScene(scene);
    }

    public void Pause(){
        if(!isPaused && pauseUI != null){
            isPaused = true;
            Time.timeScale = 0f;
            pauseUI.SetActive(true);
        }
        else if(pauseUI != null) UnPause();
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
