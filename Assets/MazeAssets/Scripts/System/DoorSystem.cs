using System.Diagnostics;
using System.Numerics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using System.Collections;


/// <summary>
/// Tag component identifying key entities
/// </summary>

public struct DiamondKeys : IComponentData { }

/// <summary>
/// System that handles door opening behavior when the diamond key is nearby.
/// Operates after transform updates to ensure correct positioning.
/// </summary>

[UpdateAfter(typeof(TransformSystemGroup))]
public partial struct DoorSystem : ISystem
{
    /// <summary>
    /// System creation callback - specifies we need DiamondKey components to exist
    /// </summary>

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<DiamondKey>();
    }

    /// <summary>
    /// Main update loop that checks key proximity and opens doors
    /// </summary>

    public void OnUpdate(ref SystemState state)
    {
        // Get the single diamond key entity and its position
        var diamondKeyEntity = SystemAPI.GetSingletonEntity<DiamondKey>();
        var diamondKeyPosition = SystemAPI.GetComponentRO<LocalTransform>(diamondKeyEntity).ValueRO.Position;

        // Query all door entities (marked with Keys component)
        foreach (var (transform, diamonddoorEntity) in
                 SystemAPI.Query<RefRW<LocalTransform>>()
                     .WithAll<DiamondKeys>()
                     .WithEntityAccess())
        {
            // Calculate 2D distance (ignoring Y-axis) between key and door
            float distance = math.distance(transform.ValueRO.Position.xz, diamondKeyPosition.xz);


            // If key is within 3 units, rotate door to "open" position (90 degrees on Y-axis)
            if (distance < 3f)
            {
                UnityEngine.Debug.Log("funciona");

               

                transform.ValueRW.Rotation = quaternion.RotateY(math.radians(90));
            }
        }


    }
}


