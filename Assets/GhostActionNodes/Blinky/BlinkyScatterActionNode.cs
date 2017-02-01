using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourMachine;

[NodeInfo(category = "Custom/", icon = "DefaultAsset", description = "Blinky Scatter.")]
public class BlinkyScatterActionNode : ActionNode
{
    public Vector2[] scatterLocations;

    public FsmEvent chaseEvent;
    public FsmEvent deathEvent;

    bool chase = false;

    float currentTime;
    public float scatterTime;

    int locationIndex = 0;

    GhostController blinkyController;

    public override void Reset()
    {
        chaseEvent = new ConcreteFsmEvent();
        deathEvent = new ConcreteFsmEvent();

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

        // chase transition
        currentTime += Time.deltaTime;
        if (currentTime >= scatterTime)
        {
            if (chaseEvent.id != 0)
            {
                owner.root.SendEvent(chaseEvent.id);
                return Status.Success;
            }
            return Status.Failure;
        }

        if (new Vector2(blinkyController.transform.position.x, blinkyController.transform.position.y) == scatterLocations[locationIndex])
        {
            locationIndex++;
            if (locationIndex >= scatterLocations.Length)
            {
                locationIndex = 0;
            }
        }

        // check for Cruise Elroy
        blinkyController.moveToLocation = scatterLocations[locationIndex];

        return Status.Running;
    }
}
