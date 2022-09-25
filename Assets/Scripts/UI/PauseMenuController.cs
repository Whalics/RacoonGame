using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public static bool IsPaused;
    
    [SerializeField] private RectTransform _pauseUI;
    
    [SerializeField] private CanvasGroup _winGroup;
    [SerializeField] private Image _face;
    [SerializeField] private CanvasGroup _loseGroup;
    [SerializeField] private CanvasGroup _fadeBlackGroup;

    public static bool GameOver;
    
    private CanvasGroup _group;
    private bool _isPaused;
    private Coroutine _unpauseRoutine;
    
    private void Awake() {
        _group = _pauseUI.GetComponent<CanvasGroup>();
        IsPaused = false;
        _isPaused = false;
    }
    
    private void Start() {
        _fadeBlackGroup.gameObject.SetActive(true);
        _fadeBlackGroup.alpha = 1;
        _fadeBlackGroup.DOFade(0f, 1f).SetEase(Ease.InOutQuint);
        AudioManager.PlaySound("TrashEnter");
    }

    private void OnEnable() {
        UserInput.TogglePaused += TogglePaused;
    }
    private void OnDisable() {
        UserInput.TogglePaused -= TogglePaused;
    }

    private void Update()
    {
        if (!GameOver && TrashEdible.EdibleItemsRemaining <= 0) Win();
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

    private void Win()
    {
        GameOver = true;
        _winGroup.gameObject.SetActive(true);
        _fadeBlackGroup.gameObject.SetActive(true);
        _winGroup.alpha = 0;
        _fadeBlackGroup.alpha = 0;
        var mySequence = DOTween.Sequence();
        mySequence.Append(_fadeBlackGroup.DOFade(1f,1f).SetEase(Ease.OutSine));
        mySequence.Append(_winGroup.DOFade(1f,1f).SetEase(Ease.OutSine));
    }
    
    public void Lose()
    {
        GameOver = true;
        _loseGroup.gameObject.SetActive(true);
        _fadeBlackGroup.gameObject.SetActive(true);
        _loseGroup.alpha = 0;
        _fadeBlackGroup.alpha = 0;
        _face.gameObject.SetActive(true);
        var mySequence = DOTween.Sequence();
        mySequence.Append(_face.DOFade(1f, 0.4f).SetEase(Ease.OutCirc));
        mySequence.AppendInterval(2f);
        mySequence.Append(_face.DOFade(0f, 0.4f).SetEase(Ease.OutCirc));

        var mySequence2 = DOTween.Sequence();
        mySequence2.Append(_face.transform.DOScale(1f, 0.4f).SetEase(Ease.OutCirc));
        mySequence2.AppendInterval(2f);
        mySequence2.Append(_fadeBlackGroup.DOFade(1f,0.5f).SetEase(Ease.OutSine));
        mySequence2.Append(_loseGroup.DOFade(1f,1f).SetEase(Ease.OutSine));
    }
}