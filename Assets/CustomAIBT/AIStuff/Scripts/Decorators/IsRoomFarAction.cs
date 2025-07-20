using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "IsRoomFar", story: "IsRoomFar [Self] [TargetRoom] [MinDistance]", category: "Action", id: "be001f2e102472c35ac77d7814535a5a")]
public partial class IsRoomFarAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> TargetRoom;
    [SerializeReference] public BlackboardVariable<float> MinDistance =  new BlackboardVariable<float>(2f);

    protected override Status OnUpdate()
    {
        GameObject self = Self?.Value;
        GameObject room = TargetRoom?.Value;

        if (self == null || room == null)
        {
            Debug.LogWarning("CheckRoomIsFar: Self or TargetRoom is null.");
            return Status.Failure;
        }

        float distance = Vector3.Distance(self.transform.position, room.transform.position);

        if (distance >= MinDistance)
        {
            Debug.Log($"CheckRoomIsFar: Room is far. Distance = {distance}");
            return Status.Success;
        }

        //Debug.Log($"CheckRoomIsFar: Room is too close. Distance = {distance}");
        return Status.Failure;
    }
}