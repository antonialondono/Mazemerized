using Unity.Entities;
using UnityEngine;

/// <summary>
/// This class is used to create a collectable entity in the ECS world.
/// </summary>
public class DoorAuthoring : MonoBehaviour
{
    public class Baker : Baker<DoorAuthoring>
    {
        public override void Bake(DoorAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
            AddComponent(entity, new DiamondKeys());
        }
    }
}

public struct Door : IComponentData
{

}
