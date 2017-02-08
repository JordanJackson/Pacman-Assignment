using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

[NodeInfo(category = "Custom/", icon = "DefaultAsset", description = "Blinky Scatter.")]
public class BlinkyScatterActionNode : ActionNode
{
    public FsmEvent chaseEvent;
    public FsmEvent deathEvent;

    public float runAwayFactor;

    Transform pacman;
    GhostController blinkyController;

    public override void Reset()
    {
        chaseEvent = new ConcreteFsmEvent();
        deathEvent = new ConcreteFsmEvent();

        pacman = GameObject.FindObjectOfType<PacmanController>().transform;
        blinkyController = owner.root.gameObject.GetComponent<GhostController>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override Status Update()
    {

        // death transition
        if (blinkyController.isDead)
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

        // Blinky runs in a direction away from pacman
        Vector3 diff = new Vector3(owner.root.transform.position.x - pacman.position.x, 0.0f, owner.root.transform.position.y - pacman.position.y);
        diff.Normalize();
        blinkyController.moveToLocation = blinkyController.transform.position + diff * runAwayFactor;

        if (blinkyController.moveToLocation.x <= 0 || blinkyController.moveToLocation.x >= 18 ||
            blinkyController.moveToLocation.y <= -21 || blinkyController.moveToLocation.y >= 0)
        {
            blinkyController.moveToLocation = blinkyController.transform.position + diff * -runAwayFactor;
        }

        return Status.Running;
    }
}
