using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Arenar.PocketFantasyWar
{
    partial struct UnitMovableSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {}


        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            MovableJob job = new MovableJob()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
            };
            job.ScheduleParallel();
            
            /*
            foreach ((RefRW<LocalTransform> localTransform,
                      RefRO<UnitMovableComponent> movableComponent)
                     in SystemAPI.Query<
                         RefRW<LocalTransform>,
                         RefRO<UnitMovableComponent>>())
            {
                var direction = movableComponent.ValueRO.targetPosition
                    - new float2(localTransform.ValueRO.Position.x, localTransform.ValueRO.Position.y);

                float distance = math.length(direction);

                if (distance > 0.001f)
                {
                    float2 normalizedDirection = direction / distance;

                    localTransform.ValueRW.Position += new float3(normalizedDirection.x, normalizedDirection.y, 0)
                        * movableComponent.ValueRO.moveSpeed * SystemAPI.Time.DeltaTime;
                }
            }
            */
        }


        [BurstCompile]
        public void OnDestroy(ref SystemState state) {}
    }


    [BurstCompile]
    public partial struct MovableJob : IJobEntity
    {
        public float deltaTime;
        
        
        public void Execute(ref LocalTransform localTransform, in UnitMovableComponent movableComponent)
        {
            var direction = movableComponent.targetPosition
                - new float2(localTransform.Position.x, localTransform.Position.y);
                
            float distance = math.length(direction);
                
            if (distance > 0.001f)
            {
                float2 normalizedDirection = direction / distance;

                localTransform.Position += new float3(normalizedDirection.x, normalizedDirection.y, 0)
                    * movableComponent.moveSpeed
                    * deltaTime;
            }
        }
    }
}