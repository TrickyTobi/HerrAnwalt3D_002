
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public abstract class PlayerBaseState
{
    protected PlayerStateMachine _ctx; //gibt den Bezug zu Variablen und Inhalten (Context)
    protected PlayerStateFactory _factory; //gibt den Bezug zur StateFactory um neue States erstellen zu können
    public PlayerBaseState _currentSuperState; //hält den aktuellen SuperState
    public PlayerBaseState _currentSubState; //hält den aktuellen SubState

    protected bool isRootState = false; //legt fest ob es sich um ein Super oder ein SubState handelt. Wird im Constructor des jeweiligen SuperState geändert

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }


    public abstract void EnterState();
    public abstract void UpdateState();

    public abstract void UpdatePhysics();

    public abstract void CheckSwitchState();

    public abstract void InitializeSubState();

    public abstract void ExitState();

    public void UpdateStates() //wird aus der StateMachine heraus gecallt. 
    {
        
        UpdateState(); //Updated den SuperState

        if (_currentSubState != null) //checkt ob ein SubState vorhanden ist
        {
            _currentSubState.UpdateStates(); //Updated den SubState
        }
        CheckSwitchState();
    }

    public void UpdateStatePhysics() //wird aus der StateMachine heraus gecallt. 
    {
        UpdatePhysics(); // Updated den SuperState

        if (_currentSubState != null) //checkt ob ein SubState vorhanden ist
        {
            _currentSubState.UpdatePhysics(); //Updated den SubState
        }

    }

    protected void SwitchStates(PlayerBaseState newState) //wechselt sowohl Sub als auch SuperState
    {
        ExitState(); //callt die Exit Methode des jeweiligen State

        newState.EnterState(); //callt die Enter Methode des jeweiligen State
        if (isRootState) // wenn es sich um ein SuperState handelt soll die Variable in der StateMachine (Context) angeglichen werden
        {
            _ctx.CurrentState = newState;
        }
        else if (_currentSuperState != null) //wenn es einen SuperState gibt, dann soll "newState" zu einem Substate werden
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState; //gleicht den SuperState an
    }
    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState; //setzt den neuen SubState als SubState ein
        newSubState.SetSuperState(this); //gibt dem SubState den aktuellen SuperState
        newSubState.EnterState(); // callt die Enter Methode des aktuellen SubState
    }
}
