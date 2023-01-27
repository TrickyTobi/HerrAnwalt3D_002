using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        ;
    }


    public override void EnterState() //Wird beim betreten des State aufgerufen
    {
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, true);
        _ctx.Animator.SetBool(_ctx.IsRunningHash, true);
        _ctx.AudioBreathPlayer.Play();

    }
    public override void UpdateState() //Wird jeden Frame aufgerufen
    {
        SpeedControl();
        HandleAnimation();

        _ctx.IncreaseBreathSound();
        

        //  if (_ctx.MaxDownLookAngle < -40f)
        //      _ctx.MaxDownLookAngle = Mathf.Lerp(_ctx.MaxDownLookAngle, -40f, Time.deltaTime * 30);

    }

    public override void UpdatePhysics() //Wird bei Physikalischen änderungen aufgerufen
    {
        ;
    }

    public override void CheckSwitchState() //checkt ob der SubState geändert werden muss
    {
        /* if (_ctx.IsMovementPressed && !_ctx.IsRunPressed) //Ändert den SubState über die Methode aus dem BaseState
             SwitchStates(_factory.Walk());
         else if (!_ctx.IsMovementPressed && !_ctx.IsRunPressed)
             SwitchStates(_factory.Idle());*/

        if (_ctx.IsRunPressed) return;

        if (_ctx.IsMovementPressed)
        {
            SwitchStates(_factory.Walk());
            return;
        }

        SwitchStates(_factory.Idle());
    }

    public override void InitializeSubState() //im SubState nicht genutzt
    {
        ;
    }

    public override void ExitState() //wird beim verlassen des State aufgerufen
    {
        // _ctx.MaxDownLookAngle = -60f;
    }



    void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(_ctx.Rigidbody.velocity.x, 0f, _ctx.Rigidbody.velocity.z);


        if (flatVelocity.magnitude <= _ctx.MaxRunSpeed)
            return;

        Vector3 limitedVelocity = flatVelocity.normalized * _ctx.MaxRunSpeed;
        _ctx.Rigidbody.velocity = new Vector3(limitedVelocity.x, _ctx.Rigidbody.velocity.y, limitedVelocity.z);
    }


    void HandleAnimation()
    {

        if (_ctx.MoveLeft && _ctx.VelocityX <= _ctx.RunThreshold) //Wenn Links und die Geschwindigkeit kleiner als 0.5 ist.
            _ctx.VelocityX += Time.deltaTime * _ctx.Acceleration;
        else if (_ctx.MoveRight && _ctx.VelocityX >= -_ctx.RunThreshold) //Wenn Rechts und die Geschwindigkeit größer als 0.5 ist.
            _ctx.VelocityX -= Time.deltaTime * _ctx.Acceleration;
        else if (!_ctx.MoveRight && _ctx.VelocityX <= -_ctx.ZeroThreshold) //Wenn Rechts NICHT und die Geschwindigkeit kleiner als 0.05 ist. Dient der zuverlässigen Nullung.
            _ctx.VelocityX += Time.deltaTime * _ctx.Deceleration;
        else if (!_ctx.MoveLeft && _ctx.VelocityX >= _ctx.ZeroThreshold) //Wenn Links NICHT und die Geschwindigkeit kleiner als 0.05 ist. Dient der zuverlässigen Nullung.
            _ctx.VelocityX -= Time.deltaTime * _ctx.Deceleration;

        if (_ctx.MoveForward && _ctx.VelocityZ <= _ctx.RunThreshold) //Wenn Vor und die Geschwindigkeit kleiner als 0.5 ist und kleiner als -0.5
            _ctx.VelocityZ += Time.deltaTime * _ctx.Acceleration;
        else if (_ctx.MoveBackward && _ctx.VelocityZ >= -_ctx.RunThreshold) //Wenn Zurück und die Geschwindigkeit größer als -0.5 ist und größer als 0.5
            _ctx.VelocityZ -= Time.deltaTime * _ctx.Acceleration;
        else if (!_ctx.MoveBackward && _ctx.VelocityZ <= -_ctx.ZeroThreshold) //Wenn Zurück NICHT und die Geschwindigkeit kleiner als 0.05 ist. Dient der zuverlässigen Nullung.
            _ctx.VelocityZ += Time.deltaTime * _ctx.Deceleration;
        else if (!_ctx.MoveForward && _ctx.VelocityZ >= _ctx.ZeroThreshold) //Wenn Vor NICHT und die Geschwindigkeit größer als 0.05 ist.Dient der zuverlässigen Nullung.
            _ctx.VelocityZ -= Time.deltaTime * _ctx.Deceleration;

        if (_ctx.VelocityX <= _ctx.ZeroThreshold && _ctx.VelocityX >= -_ctx.ZeroThreshold && !_ctx.MoveLeft && !_ctx.MoveRight) // Wenn weder Rechts, noch Links und die Geschwindigleit kleiner als 0.05 ist
            _ctx.VelocityX = 0;

        if (_ctx.VelocityZ <= _ctx.ZeroThreshold && _ctx.VelocityZ >= -_ctx.ZeroThreshold && !_ctx.MoveForward && !_ctx.MoveBackward) // Wenn weder Vor, noch Zurück und die Geschwindigleit kleiner als 0.05 ist
            _ctx.VelocityZ = 0;

        _ctx.Animator.SetFloat(_ctx.VelocityXHash, _ctx.VelocityX, 0.2f, Time.deltaTime);
        _ctx.Animator.SetFloat(_ctx.VelocityZHash, _ctx.VelocityZ, 0.2f, Time.deltaTime);
    }


}