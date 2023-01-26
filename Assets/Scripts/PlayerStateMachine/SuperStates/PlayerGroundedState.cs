using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        isRootState = true; //legt fest, dass es sich um ein PlayerGroundedState handelt
        InitializeSubState();
    }


    public override void EnterState() //Wird beim betreten des State aufgerufen
    {

    }

    public override void UpdateState() //Wird jeden Frame aufgerufen
    {
        ;
    }


    public override void UpdatePhysics() //Wird bei Physikalischen änderungen aufgerufen
    {

    }

    public override void CheckSwitchState() //checkt ob der PlayerGroundedState geändert werden muss, da dies eine PlayerGroundedState-Klasse ist
    {
        if (_ctx.IsJumpPressed && !_ctx.RequireNewJumpPress)
            SwitchStates(_factory.Jump()); //Ändert den PlayerGroundedState über die Methode aus dem BaseState
    }

    public override void InitializeSubState() //Initialisiert den Substate
    {
        if (_ctx.IsMovementPressed && !_ctx.IsRunPressed)
            SetSubState(_factory.Walk());
        else if (_ctx.IsMovementPressed && _ctx.IsRunPressed)
            SetSubState(_factory.Run());
        else
            SetSubState(_factory.Idle()); //erzeugt einen Substate mit hilfe der Methode aus dem BaseState
    }
    public override void ExitState() {; } //wird beim verlassen des State aufgerufen


}
