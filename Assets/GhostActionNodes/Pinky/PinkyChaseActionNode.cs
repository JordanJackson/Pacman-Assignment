using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

[NodeInfo(category = "Custom/", icon = "DefaultAsset", description = "Pinky Chase.")]
public class PinkyChaseActionNode : ActionNode
{

    public FsmEvent scatterEvent;
    public FsmEvent deathEvent;

    PacmanController pacman;
    GhostController controller;

    public override void Reset()
    {
        scatterEvent = new ConcreteFsmEvent();
        deathEvent = new ConcreteFsmEvent();

        pacman = GameObject.FindObjectOfType<PacmanController>();
        controller = owner.root.gameObject.GetComponent<GhostController>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override Status Update()
    {
        // death transition
        if (controller.isDead)
        {
            if (deathEvent.id != 0)
            {
                owner.root.SendEvent(deathEvent.id);
                return Status.Success;
            }
            return Status.Failure;
        }

        // scatter transition
        if (GameDirector.Instance.state == GameDirector.States.enState_PacmanInvincible)
        {
            if (scatterEvent.id != 0)
            {
                owner.root.SendEvent(scatterEvent.id);
                return Status.Success;
            }
            return Status.Failure;
        }

        // 4 tiles ahead in pacman's current direction
        controller.moveToLocation = new Vector2(pacman.transform.position.x, pacman.transform.position.y) + pacman.MoveDirections[(int)pacman.moveDirection] * 4.0f;

        return Status.Running;
    }
}
