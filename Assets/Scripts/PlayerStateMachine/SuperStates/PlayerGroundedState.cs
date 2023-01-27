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
        isRootState = true;
        InitializeSubState();
    }


    public override void EnterState()
    {
     
    }

    public override void UpdateState() 
    {
      
    }


    public override void UpdatePhysics()
    {

    }

    public override void CheckSwitchState()
    {
        if (_ctx.IsJumpPressed && !_ctx.RequireNewJumpPress)
        {
            SwitchStates(_factory.Jump());
        }
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
    public override void ExitState() {; }


}
