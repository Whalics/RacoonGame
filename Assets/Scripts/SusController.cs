using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SusController : MonoBehaviour
{
    [SerializeField] private CameraController _player;

    [Header("Dead")]
    [SerializeField] private bool _goingToDie;
    [SerializeField] private bool _dead;
    [SerializeField] private Transform face;
    [SerializeField] private Image img_face;

    [Header("Hunger")]
    [SerializeField, ReadOnly] private float _hunger;
    [SerializeField, ReadOnly] private int _timesGrowled;
    [SerializeField] private float _hungerRate = 1;
    [SerializeField] private float _hungerToGrowl = 6;
    [SerializeField] private float _foodSatisfaction = 8;
    [SerializeField] private float _timeToGetHungry = 1.5f;

    [Header("Sus")]
    [SerializeField, ReadOnly] private float _sus;
    [SerializeField] private float _activeSusTime = 2;
    [SerializeField] private float _susHidingRate = -0.1f;
    [SerializeField] private float _susNormalRate = 0.01f;
    [SerializeField] private float _susFromEating = 0.1f;
    [SerializeField] private float _susFromGrowl = 3.0f;
    [SerializeField] private float _susFromSniff = 0.25f;
    [SerializeField] private float _susFromClickingGarbage = 0.1f;
    [SerializeField] private float _susFromMovingGarbage = 0.02f;

    [Header("Sus Levels")]
    [SerializeField, ReadOnly] private int _susLevel;
    [SerializeField] private GameObject houseLightsOff;
    [SerializeField] private float _level1 = 1.2f;
    [SerializeField] private GameObject houseLightsOn;
    [SerializeField] private float _level2 = 3.5f;
    [SerializeField] private GameObject houseCurtainsOpen;
    [SerializeField] private float _level3 = 6.5f;
    [SerializeField] private GameObject houseBlindsOpen;
    [SerializeField] private float _level4 = 9f;
    [SerializeField] private GameObject houseCameraOn;
    [SerializeField] private float _level5 = 11f;
    [SerializeField] private GameObject housePorchlightOn;
    [SerializeField] private float _level6 = 13f;
    [SerializeField] private GameObject houseDoorSillouette;
    [SerializeField] private float _level7 = 15f;
    [SerializeField] private GameObject houseDoorOpen;

    [Header("Sniff")]
    [SerializeField] private float _timeToSniff = 8;
    [SerializeField] private float _timeToSniffVariation = 5;
    [SerializeField] private List<string> _sniffSounds;
    
    private float _lastTimeEaten;
    private float _timeSinceSus;
    private float _sniffTime;

    private void Start()
    {
        _lastTimeEaten = Time.time;
        _timeSinceSus = Time.time;
        _hunger = 0;
        _sus = 0;
        _timesGrowled = 0;
        UpdateHouseArt();
        SetSniffTime();
    }
    
    private void OnValidate()
    {
        if (_player == null) _player = FindObjectOfType<CameraController>();
    }

    private void Update()
    {
        if (_dead) return;
        if (_goingToDie)
        {
            if (!_player.Hiding)
            {
                Caught();
            }
            return;
        }
        UpdateHunger();
        UpdateSus();
        UpdateSniff();
    }
    
    public void OnEatItem()
    {
        if (_dead) return;
        if (_goingToDie || _dead) return;
        _lastTimeEaten = Time.time;
        _hunger -= _foodSatisfaction;
        if (_timesGrowled > 0) _timesGrowled--;
        AdjustSus(_susFromEating);
    }
    
    public void OnClickGarbage() => AdjustSus(_susFromClickingGarbage);
    public void OnMoveGarbage(float dist) => AdjustSus(dist * _susFromMovingGarbage);

    private void UpdateHunger()
    {
        float timeSinceEaten = Time.time - _lastTimeEaten;
        if (timeSinceEaten < _timeToGetHungry) return;
        
        float hungerMultiplier = Mathf.Clamp01(timeSinceEaten - _timeToGetHungry);
        _hunger += _hungerRate * hungerMultiplier * Time.deltaTime;

        if (_hunger > _hungerToGrowl)
        {
            Growl();
        }
    }

    private void Growl()
    {
        _timesGrowled++;
        AdjustSus(_susFromGrowl * _timesGrowled);
    }

    private void UpdateSus()
    {
        if (Time.time - _timeSinceSus < _activeSusTime) return;

        // Decrease Sus
        AdjustSus(-(_player.Hiding ? _susHidingRate : _susNormalRate) * Time.deltaTime);
    }
    
    private void AdjustSus(float amount)
    {
        if (_dead) return;
        if (amount > 0) _timeSinceSus = Time.time;
        _sus += amount;

        int susLevel = GetSusLevel(_sus);
        if (susLevel != _susLevel)
        {
            _susLevel = susLevel;
            UpdateHouseArt();

            if (_susLevel == 7) _goingToDie = true;
        }
    }

    private void UpdateHouseArt()
    {
        houseLightsOff.SetActive(_susLevel == 0);
        houseLightsOn.SetActive(_susLevel == 1);
        houseCurtainsOpen.SetActive(_susLevel == 2);
        houseBlindsOpen.SetActive(_susLevel == 3);
        houseCameraOn.SetActive(_susLevel == 4);
        housePorchlightOn.SetActive(_susLevel == 5);
        houseDoorSillouette.SetActive(_susLevel == 6);
        houseDoorOpen.SetActive(_susLevel == 7);
    }

    private int GetSusLevel(float sus)
    {
        if (sus < _level1) return 0;
        if (sus < _level2) return 1;
        if (sus < _level3) return 2;
        if (sus < _level4) return 3;
        if (sus < _level5) return 4;
        if (sus < _level6) return 5;
        return sus < _level7 ? 6 : 7;
    }

    private void UpdateSniff()
    {
        if (Time.time > _sniffTime)
        {
            SetSniffTime();
            Sniff();
        }
    }

    private void SetSniffTime()
    {
        _sniffTime = Time.time + _timeToSniff + Random.Range(-_timeToSniffVariation, _timeToSniffVariation);
    }

    private void Sniff()
    {
        AudioManager.PlaySound(_sniffSounds[Random.Range(0, _sniffSounds.Count)]);
        AdjustSus(_susFromSniff);
    }
    
    public void Caught()
    {
        _dead = true;
        img_face.DOFade(1f, 0.4f).SetEase(Ease.OutCirc);
        face.DOScale(1f, 0.4f).SetEase(Ease.OutCirc);
    }
}
