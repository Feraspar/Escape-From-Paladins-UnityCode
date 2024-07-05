using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : BaseMoveState
{
    float backwardSpeed;
    public override void EnterState(MovementStateManager movement)
    {
        movement.anim.SetBool("Walking", true);
    }

    public override void UpdateState(MovementStateManager movement)
    {
        if (movement.sprint)
        {
            if (movement.vInput < 0)
            {
                backwardSpeed = movement.originalMoveSpeed;
                movement.moveSpeed = backwardSpeed;
            }
            else
            {
                movement.moveSpeed = movement.originalMoveSpeed * 2;
            }
        }
        else
        {
            if (movement.vInput < 0)
            {
                backwardSpeed = movement.originalMoveSpeed / 2;
                movement.moveSpeed = backwardSpeed;
            }
            else
            {
                movement.moveSpeed = movement.originalMoveSpeed;
            }
        }
    }
}
