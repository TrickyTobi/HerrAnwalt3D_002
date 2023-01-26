using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUI : MonoBehaviour
{

    [SerializeField] GameObject[] _sandclock;
    [SerializeField] Transform _sandclockPartcileSpawn;
    [SerializeField] GameObject[] _hammer;
    [SerializeField] Transform[] _hammerParticleSpawn;
    [Space(20)]
    [SerializeField] PlayerStatsSO _playerStats;
    [Space(20)]
    [SerializeField] ParticleSystem _addParagraphEffect;
    [SerializeField] ParticleSystem _addLifeEffect;
    [SerializeField] ParticleSystem _loseLifeEffect;




    int _previoudLife;


    private void Start()
    {
        _previoudLife = _playerStats.PlayerHealth;
    }

    void CheckSandclock()
    {
        switch (_playerStats.PlayerParagraphCount)
        {
            case 0:
                _sandclock[0].SetActive(true);
                _sandclock[1].SetActive(false);
                _sandclock[2].SetActive(false);
                _sandclock[3].SetActive(false);
                _sandclock[4].SetActive(false);
                ParticleEffectParagraph(_sandclock[0]);

                break;
            case 1:
                _sandclock[0].SetActive(false);
                _sandclock[1].SetActive(true);
                _sandclock[2].SetActive(false);
                _sandclock[3].SetActive(false);
                _sandclock[4].SetActive(false);
                ParticleEffectParagraph(_sandclock[0]);
                break;
            case 2:
                _sandclock[0].SetActive(false);
                _sandclock[1].SetActive(false);
                _sandclock[2].SetActive(true);
                _sandclock[3].SetActive(false);
                _sandclock[4].SetActive(false);
                ParticleEffectParagraph(_sandclock[0]);
                break;
            case 3:
                _sandclock[0].SetActive(false);
                _sandclock[1].SetActive(false);
                _sandclock[2].SetActive(false);
                _sandclock[3].SetActive(true);
                _sandclock[4].SetActive(false);
                ParticleEffectParagraph(_sandclock[0]);
                break;
            case 4:
                _sandclock[0].SetActive(false);
                _sandclock[1].SetActive(false);
                _sandclock[2].SetActive(false);
                _sandclock[3].SetActive(false);
                _sandclock[4].SetActive(true);
                ParticleEffectParagraph(_sandclock[0]);
                break;
            default:
                break;
        }
    }


    void CheckHammer()
    {
        switch (_playerStats.PlayerHealth)
        {
            case 0:
                _hammer[0].SetActive(false);
                _hammer[1].SetActive(false);
                _hammer[2].SetActive(false);
                _hammer[3].SetActive(false);
                ParticleEffectLife(_hammer[0]);
                _previoudLife = _playerStats.PlayerHealth;

                break;
            case 1:
                _hammer[0].SetActive(true);
                _hammer[1].SetActive(false);
                _hammer[2].SetActive(false);
                _hammer[3].SetActive(false);
                ParticleEffectLife(_hammer[1]);
                _previoudLife = _playerStats.PlayerHealth;
                break;
            case 2:
                _hammer[0].SetActive(true);
                _hammer[1].SetActive(true);
                _hammer[2].SetActive(false);
                _hammer[3].SetActive(false);
                ParticleEffectLife(_hammer[2]);
                _previoudLife = _playerStats.PlayerHealth;
                break;
            case 3:
                _hammer[0].SetActive(true);
                _hammer[1].SetActive(true);
                _hammer[2].SetActive(true);
                _hammer[3].SetActive(false);
                ParticleEffectLife(_hammer[3]);
                _previoudLife = _playerStats.PlayerHealth;
                break;
            case 4:
                _hammer[0].SetActive(true);
                _hammer[1].SetActive(true);
                _hammer[2].SetActive(true);
                _hammer[3].SetActive(true);
                ParticleEffectLife(_hammer[3]);
                _previoudLife = _playerStats.PlayerHealth;
                break;
            default:
                break;
        }
    }
    void ParticleEffectLife(GameObject positionHammer)
    {
        ParticleSystem effect = null;

        // player gained life.
        if (_previoudLife < _playerStats.PlayerHealth)
        {
            effect = Instantiate(_addLifeEffect, positionHammer.transform.position + Vector3.up * 0.5f, Quaternion.identity);

        }
        // player lost life.
        else if (_previoudLife > _playerStats.PlayerHealth)
        {
            effect = Instantiate(_loseLifeEffect, positionHammer.transform.position + Vector3.up * 0.5f, Quaternion.identity);

        }


        effect.Play();
    }

    void ParticleEffectParagraph(GameObject positionSandclock)
    {
        var effect = Instantiate(_addParagraphEffect, positionSandclock.transform.position + Vector3.up * 0.16f, Quaternion.identity);
        effect.Play();
    }

    private void OnEnable()
    {
        EventChannelSO.OnAddParagraph += CheckSandclock;
        EventChannelSO.OnAddLife += CheckHammer;
        EventChannelSO.OnLoseLife += CheckHammer;
    }

    private void OnDisable()
    {
        EventChannelSO.OnAddParagraph -= CheckSandclock;
        EventChannelSO.OnAddLife -= CheckHammer;
        EventChannelSO.OnLoseLife -= CheckHammer;
    }
}
