using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = ("PlayerStatsSO"))]
public class PlayerStatsSO : ScriptableObject
{
    [SerializeField] EventChannelSO _event;

    public int PlayerHealth { get => _playerHealth; set => _playerHealth = value; }
    [SerializeField] int _playerHealth;

    public int PlayerMaxHealth { get => _playerMaxHealth; }
    [SerializeField] int _playerMaxHealth;


    [SerializeField] int _playerParagraphCount;
    public int PlayerParagraphCount { get => _playerParagraphCount; set { _playerParagraphCount = value; } }

    [SerializeField] int _paragraphsToAddLife;

    [SerializeField] int _freedChilds; public int FreedChilds { get => _freedChilds; set => _freedChilds = value; }
    [SerializeField] int _childsToFree; public int ChildsToFree { get => _childsToFree; }

    [SerializeField] int _teacherAlive; public int TeacherAlive { get => _teacherAlive; set => _teacherAlive = value; }
    



    public void AddParagraph()
    {
        _playerParagraphCount++;

        if (_playerParagraphCount >= _paragraphsToAddLife)
        {
            _playerParagraphCount = 0;
            _playerHealth++;
            _event.AddLife();
        }
    }

    public void LoseLife()
    {
        _playerHealth--;
        if (_playerHealth <= 0)
        {
            _event.GameOver();
        }
    }

    private void OnEnable()
    {

        EventChannelSO.OnAddParagraph += AddParagraph;
        EventChannelSO.OnLoseLife += LoseLife;
    }

    private void OnDisable()
    {
        EventChannelSO.OnAddParagraph -= AddParagraph;
        EventChannelSO.OnLoseLife -= LoseLife;
    }
}
