using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("EventChannelSO"))]
public class EventChannelSO : ScriptableObject
{
    public delegate void EventChannel();
    public static event EventChannel OnAddLife;
    public static event EventChannel OnAddParagraph;
    public static event EventChannel OnLoseLife;
    public static event EventChannel OnGameOver;
    public static event EventChannel OnGameEnded;
    public static event EventChannel OnEnableInput;
    public static event EventChannel OnDisableInput;

    // Gets called if a life is added to the player.
    public void AddLife()
    {
        if (OnAddLife != null)
            OnAddLife.Invoke();
    }

    // Gets called if an paragraph is collected.
    public void AddParagraph()
    {
        if (OnAddParagraph != null)
            OnAddParagraph.Invoke();
    }


    public void LoseLife()
    {
        if (OnLoseLife != null)
            OnLoseLife.Invoke();
    }

    public void GameOver()
    {
        if (OnGameOver != null)
            OnGameOver.Invoke();
    }

    public void GameEnded()
    {
        if (OnGameOver != null)
            OnGameEnded.Invoke();
    }

    public void EnableInput()
    {

        if (OnEnableInput != null)
            OnEnableInput.Invoke();
    }

    public void DisableInput()
    {
        if (OnDisableInput != null)
            OnDisableInput.Invoke();

    }

}
