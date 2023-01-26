
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;

public class PlayerStateFactory
{
    PlayerStateMachine _context; // setzt bezug zur StateMachine, um Variablen und Inhalte(Context) in die Constructor der Super und SubStates weiterzuleiten

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }


    public PlayerBaseState Grounded()
    {
        return new PlayerGroundedState(_context, this); //Gibt einen SuperState zurück mit Bezug zur StateMachine und der Factory
    }

    public PlayerBaseState Jump()
    {
        return new PlayerJumpState(_context, this); //Gibt einen SubState zurück mit Bezug zur StateMachine und der Factory
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(_context, this); //Gibt einen SubState zurück mit Bezug zur StateMachine und der Factory
    }

    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(_context, this); //Gibt einen SubState zurück mit Bezug zur StateMachine und der Factory
    }

    public PlayerBaseState Run()
    {
        return new PlayerRunState(_context, this); //Gibt einen SubState zurück mit Bezug zur StateMachine und der Factory
    }




}
