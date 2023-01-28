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

    public override void EnterState()
    {

        _ctx.Animator.SetBool(_ctx.IsJumpHash, true);
        _ctx.RequireNewJumpPress = true;
        _ctx.Rigidbody.AddForce(_ctx.transform.up * _ctx.InitialJumpVelocity, ForceMode.Impulse);
    }
    public override void UpdateState()
    {
        if (_airTimer >= _minimumAirTime)
            _enoughAirTime = true;

        _airTimer += Time.deltaTime;
    }

    public override void UpdatePhysics()
    {

    }

    public override void CheckSwitchState()
    {
        if (_ctx.IsGrounded && _enoughAirTime)
            SwitchStates(_factory.Grounded());
    }

    public override void InitializeSubState()
    {
        if (_ctx.IsMovementPressed && !_ctx.IsRunPressed)
            SetSubState(_factory.Walk());
        else if (_ctx.IsMovementPressed && _ctx.IsRunPressed)
            SetSubState(_factory.Run());
        else
            SetSubState(_factory.Idle());
    }

    public override void ExitState()
    {
        _ctx.Animator.SetBool(_ctx.IsJumpHash, false);
    }





}
