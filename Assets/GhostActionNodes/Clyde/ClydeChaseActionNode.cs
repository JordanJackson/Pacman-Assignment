using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

[NodeInfo(category = "Custom/", icon = "DefaultAsset", description = "Clyde Chase.")]
public class ClydeChaseActionNode : ActionNode
{

    public FsmEvent scatterEvent;
    public FsmEvent deathEvent;

    bool scatter = false;

    float currentTime;
    public float chaseTime;

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


        currentTime = 0.0f;
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

        // if further than 8 tiles from Pacman
        if (Vector2.Distance(new Vector2(pacman.transform.position.x, pacman.transform.position.y), controller.transform.position) >= 8)
        {
            controller.moveToLocation = pacman.transform.position;
        }
        else
        {
            controller.moveToLocation = new Vector2(1, -20);
        }

        return Status.Running;
    }
}
