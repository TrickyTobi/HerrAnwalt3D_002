using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        isRootState = true;
        InitializeSubState();
    }

    float _airTimer = 0f;
    float _minimumAirTime = 0.3f;
    bool _enoughAirTime = false;

    public override void EnterState() //Wird beim betreten des State aufgerufen
    {

        _ctx.Animator.SetBool(_ctx.IsJumpHash, true);
        _ctx.RequireNewJumpPress = true;
        _ctx.Rigidbody.AddForce(_ctx.transform.up * _ctx.InitialJumpVelocity, ForceMode.Impulse);
        Debug.Log("Here");
    }
    public override void UpdateState() //Wird jeden Frame aufgerufen
    {
        Debug.Log("Jump");
        if (_airTimer >= _minimumAirTime)
            _enoughAirTime = true;

        _airTimer += Time.deltaTime;
    }

    public override void UpdatePhysics() //Wird bei Physikalischen änderungen aufgerufen
    {

    }

    public override void CheckSwitchState() //checkt ob der PlayerGroundedState geändert werden muss, da dies eine PlayerGroundedState-Klasse ist
    {
        if (_ctx.IsGrounded && _enoughAirTime)
            SwitchStates(_factory.Grounded());
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

    public override void ExitState() //wird beim verlassen des State aufgerufen
    {
        _ctx.Animator.SetBool(_ctx.IsJumpHash, false);
    }





}
