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

    bool _breath;
    float _breathTimer;
    float _minimumBreathTime = 3;

    public override void EnterState()
    {
        _ctx.Animator.SetBool(_ctx.IsWalkingHash, true);
        _ctx.Animator.SetBool(_ctx.IsRunningHash, true);
        _breath = false;
        _breathTimer = 0;
        _ctx.StartCoroutine(BreathTimer());

    }
    public override void UpdateState()
    {
        SpeedControl();
        HandleAnimation();

        _ctx.IncreaseChromaticAberation();

        if (!_breath)
            return;

        _ctx.IncreaseBreathSound();



    }

    public override void UpdatePhysics()
    {
        ;
    }

    public override void CheckSwitchState()
    {
        if (_ctx.IsRunPressed) return;

        if (_ctx.IsMovementPressed)
        {
            SwitchStates(_factory.Walk());
            return;
        }

        SwitchStates(_factory.Idle());
    }

    public override void InitializeSubState()
    {
        ;
    }

    public override void ExitState()
    {

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

    IEnumerator BreathTimer()
    {
        yield return new WaitForSeconds(3f);
        _breath = true;
    }

}