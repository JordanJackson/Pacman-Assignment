using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

[NodeInfo(category = "Custom/", icon = "DefaultAsset", description = "Blinky Chase.")]
public class BlinkyChaseActionNode : ActionNode
{

    public FsmEvent scatterEvent;
    public FsmEvent deathEvent;

    bool scatter = false;

    float currentTime;
    public float chaseTime;

    Transform pacman;
    GhostController blinkyController;

    public override void Reset()
    {
        scatterEvent = new ConcreteFsmEvent();
        deathEvent = new ConcreteFsmEvent();

        pacman = GameObject.FindObjectOfType<PacmanController>().transform;
        blinkyController = owner.root.gameObject.GetComponent<GhostController>();
    }

    public override void OnEnable()
    {
        base.OnEnable();


        currentTime = 0.0f;   
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

        // scatter transition
        currentTime += Time.deltaTime;

        if (currentTime > chaseTime)
        {
            if (scatterEvent.id != 0)
            {
                owner.root.SendEvent(scatterEvent.id);
                return Status.Success;
            }
            return Status.Failure;
        }

        // check for Cruise Elroy
        blinkyController.moveToLocation = pacman.position;

        return Status.Running;
    }
}
