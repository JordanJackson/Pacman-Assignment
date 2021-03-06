//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
//----------------------------------------------

using UnityEngine;
using System.Collections;

namespace BehaviourMachine {

    /// <summary>
    /// Returns Success if "A" is not equal to "B"; returns Failure otherwise.
    /// </summary>
    [NodeInfo ( category = "Condition/Blackboard/",
                icon = "Blackboard",
                description = "Returns Success if \"A\" is not equal to \"B\"; returns Failure otherwise")]
    public class IsRectNotEqual : ConditionNode {

        /// <summary>
        /// The first Rect.
        /// </summary>
    	[VariableInfo(tooltip = "The first Rect")]
        public RectVar a;

        /// <summary>
        /// The second Rect.
        /// </summary>
        [VariableInfo(tooltip = "The second Rect")]
        public RectVar b;

        public override void Reset () {
            base.Reset();

            a = new ConcreteRectVar();
            b = new ConcreteRectVar();
        }

        public override Status Update () {
            // Validate members
            if (a.isNone || b.isNone)
                return Status.Error;

            if (a.Value != b.Value) {
                // Send event?
                if (onSuccess.id != 0)
                    owner.root.SendEvent(onSuccess.id);

                return Status.Success;
            }
            else
                return Status.Failure;
        }
    }
}
