using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public static bool IsPaused;
    
    [SerializeField] private RectTransform _pauseUI;
    
    private CanvasGroup _group;
    private bool _isPaused;
    private Coroutine _unpauseRoutine;

    private void Awake() {
        _group = _pauseUI.GetComponent<CanvasGroup>();
        IsPaused = false;
        _isPaused = false;
    }

    private void OnEnable() {
        UserInput.TogglePaused += TogglePaused;
    }
    private void OnDisable() {
        UserInput.TogglePaused -= TogglePaused;
    }

    private void OnDestroy()
    {
        IsPaused = false;
    }

    [Button(Mode = ButtonMode.InPlayMode)]
    public void TogglePaused() {
        if (_isPaused)
            UnPause();
        else
            Pause();
    }

    private void Pause() {
        _isPaused = true;
        IsPaused = true;
        if (_unpauseRoutine != null) StopCoroutine(_unpauseRoutine);
        Time.timeScale = 0f;
        if (_pauseUI) _pauseUI.gameObject.SetActive(true);
        if (_group) _group.DOFade(1f,0.3f).SetEase(Ease.OutSine).SetUpdate(true);
    }

    private void UnPause() {
        _isPaused = false;
        if (_pauseUI) _pauseUI.gameObject.SetActive(false);
        if (_group) _group.DOFade(0f,0.3f).SetEase(Ease.OutSine).SetUpdate(true);
        _unpauseRoutine = StartCoroutine(UnPauseRoutine());
    }
    
    private IEnumerator UnPauseRoutine(){
        yield return new WaitForSecondsRealtime(0.3f);
        IsPaused = false;
        _isPaused = false;
        Time.timeScale = 1f;
        _unpauseRoutine = null;
    }

    public void LoadMainMenu(){
        var mySequence = DOTween.Sequence();
        mySequence.AppendInterval(0.4f).SetUpdate(true);
        mySequence.Append(_pauseUI.DOAnchorPos(Vector2.up*-1080,0.8f,false).SetEase(Ease.InOutQuint));
        StartCoroutine(MainMenuController.SceneChange("MainMenu",1.3f));
    }
}
