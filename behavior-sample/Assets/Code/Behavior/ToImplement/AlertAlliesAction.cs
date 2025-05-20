using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Alert Allies", story: "Alert [Allies] about [Target]", category: "Action", id: "43b0336a379384f4dd390aac89f87a7f")]
public partial class AlertAlliesAction : Action
{
    [SerializeReference] public BlackboardVariable<List<GameObject>> Allies;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

