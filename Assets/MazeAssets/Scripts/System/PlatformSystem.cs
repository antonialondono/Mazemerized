using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// System that handles button trigger interactions with platforms.
/// When player approaches a button, it triggers associated platform movement.
/// </summary>
public partial struct PlatformTriggerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // System requires these components to exist in the world
        state.RequireForUpdate<Platform>();
        state.RequireForUpdate<ButtonTrigger>();
        state.RequireForUpdate<PlayerTag>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Get player position
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        var playerPosition = SystemAPI.GetComponentRO<LocalTransform>(playerEntity).ValueRO.Position;

        // Create command buffer for structural changes
        var entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

        // Query all button triggers
        foreach (var (transform, ButtonTrigger, entity) in
                 SystemAPI.Query<RefRW<LocalTransform>, RefRO<ButtonTrigger>>()
                     .WithEntityAccess())
        {
            // Calculate 2D distance to player (ignoring Y-axis)
            float distance = math.distance(transform.ValueRO.Position.xz, playerPosition.xz);


            // If player is within 2 units of button
            if (distance < 2f)
            {
                // Verify platform entity exists and has Platform component
                if (SystemAPI.HasComponent<Platform>(ButtonTrigger.ValueRO.PlatformEntity))
                {
                    // Activate platform movement
                    SystemAPI.GetComponentRW<Platform>(ButtonTrigger.ValueRO.PlatformEntity).ValueRW.Opening = true;
                    // Remove the DoorTrigger component to prevent multiple triggers
                    entityCommandBuffer.RemoveComponent<ButtonTrigger>(entity);
                }
            }
        }

        // Execute all commands
        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}

/// <summary>
/// System that handles platform movement when activated.
/// Smoothly elevates platforms over time when triggered.
/// </summary>
public partial struct PlatformSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Platform>();
    }

    // Process all platforms
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (platform, transform) in SystemAPI.Query<RefRW<Platform>, RefRW<LocalTransform>>())
        {

            if (platform.ValueRO.Opening == true)
            {

                // Update movement timer
                platform.ValueRW.CurrentTimer += SystemAPI.Time.DeltaTime;

                // Calculate interpolation factor (0-1)
                var t = platform.ValueRO.CurrentTimer / platform.ValueRO.OpeningTime;

                // Smoothly move platform vertically
                float3 pos = transform.ValueRW.Position;
                pos.y = math.lerp(0, platform.ValueRO.FinalDisplacementY, t);
                transform.ValueRW.Position = pos;


                if (t >= 0.9f)
                {
                    platform.ValueRW.Opening = false; // Stops movement
                    
                }
            }

         

        }
    }
}
