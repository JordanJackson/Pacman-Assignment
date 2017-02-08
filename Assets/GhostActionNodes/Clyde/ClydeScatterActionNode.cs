using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

[NodeInfo(category = "Custom/", icon = "DefaultAsset", description = "Clyde Scatter.")]
public class ClydeScatterActionNode : ActionNode
{

    public Vector2[] scatterLocations;

    public FsmEvent chaseEvent;
    public FsmEvent deathEvent;

    Transform pacman;

    int locationIndex = 0;

    GhostController controller;

    public override void Reset()
    {
        chaseEvent = new ConcreteFsmEvent();
        deathEvent = new ConcreteFsmEvent();

        pacman = GameObject.FindObjectOfType<PacmanController>().transform;
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

        // chase transition
        if (GameDirector.Instance.state == GameDirector.States.enState_Normal)
        {
            if (chaseEvent.id != 0)
            {
                owner.root.SendEvent(chaseEvent.id);
                return Status.Success;
            }
            return Status.Failure;
        }

        // Clyde aims for suicide
        controller.moveToLocation = pacman.position;

        return Status.Running;
    }
}
