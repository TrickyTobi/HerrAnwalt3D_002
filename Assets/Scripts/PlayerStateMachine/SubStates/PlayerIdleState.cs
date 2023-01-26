using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        ;
    }


    public override void EnterState() //Wird beim betreten des State aufgerufen
    {
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, false);
    }
    public override void UpdateState() //Wird jeden Frame aufgerufen
    {

        HandleAnimation();

        _ctx.HandleAttack();

        _ctx.HandleBlock();

        _ctx.DecreaseBreathSound();
    }



    public override void UpdatePhysics() //Wird bei Physikalischen änderungen aufgerufen
    {
        ;
    }

    public override void CheckSwitchState() //checkt ob der SubState geändert werden muss
    {
        if (_ctx.IsMovementPressed && !_ctx.IsRunPressed) //Ändert den SubState über die Methode aus dem BaseState
            SwitchStates(_factory.Walk());
        else if (_ctx.IsMovementPressed && _ctx.IsRunPressed)
            SwitchStates(_factory.Run());
    }

    public override void InitializeSubState() //im SubState nicht genutzt
    {
        ;
    }

    public override void ExitState() //wird beim verlassen des State aufgerufen
    {
        ;
    }

    void HandleAnimation()
    {
        if (_ctx.VelocityX >= 0.05f)
            _ctx.VelocityX -= Time.deltaTime * _ctx.Deceleration;
        else if (_ctx.VelocityX <= -0.05f)
            _ctx.VelocityX += Time.deltaTime * _ctx.Deceleration;

        if (_ctx.VelocityZ >= 0.05f)
            _ctx.VelocityZ -= Time.deltaTime * _ctx.Deceleration;
        else if (_ctx.VelocityZ <= -0.05f)
            _ctx.VelocityZ += Time.deltaTime * _ctx.Deceleration;

        if (_ctx.VelocityX <= 0.05f && _ctx.VelocityX >= -0.05f && _ctx.VelocityZ <= 0.05f && _ctx.VelocityZ >= -0.05f)
        {
            _ctx.VelocityX = 0;
            _ctx.VelocityZ = 0;
        }


        _ctx.Animator.SetFloat(_ctx.VelocityXHash, _ctx.VelocityX);
        _ctx.Animator.SetFloat(_ctx.VelocityZHash, _ctx.VelocityZ);



    }

    


}