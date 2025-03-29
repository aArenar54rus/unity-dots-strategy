using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace Arenar.PocketFantasyWar
{
    public class UnitMovableAuthoring : MonoBehaviour
    {
        public float moveSpeed;
        public UnitDataContainer unitDataContainer;

        
        public float MoveSpeed => unitDataContainer != null ? unitDataContainer.UnitData.UnitSpeed : moveSpeed;

        
        
        public class MovableAuthoringBaker : Baker<UnitMovableAuthoring>
        {
            public override void Bake(UnitMovableAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new UnitMovableComponent()
                {
                    moveSpeed = authoring.MoveSpeed,
                    targetPosition = new float2(authoring.transform.position.x, authoring.transform.position.y), 
                });
            }
        }
    }
    

    public struct UnitMovableComponent : IComponentData
    {
        public float moveSpeed;
        public float2 targetPosition;
    }
}