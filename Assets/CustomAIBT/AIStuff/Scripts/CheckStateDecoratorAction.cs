using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "CheckStateDecorator ", story: "CheckStateDecorator", category: "Action",
        id: "3cfd324e58affbba5f73c7842ef4b0d1")]
    public partial class CheckStateDecoratorAction : Composite
    {
        [SerializeField] public AIBehaviorState RequiredState;
    }
}