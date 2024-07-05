using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseMoveState
{
    public override void EnterState(MovementStateManager movement)
    {

    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.dir.magnitude > 0.1f)
        {
            movement.SwitchState(movement.Walk);
        }
    }
}
