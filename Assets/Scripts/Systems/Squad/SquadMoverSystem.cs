using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


namespace Arenar.PocketFantasyWar
{
    partial struct SquadMoverSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) {}


        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((RefRO<SquadComponent> squadComponent,
                      RefRO<SquadStateComponent> squadStateComponent,
                      RefRO<LocalToWorld> localToWorld)
                     in SystemAPI.Query<
                         RefRO<SquadComponent>,
                         RefRO<SquadStateComponent>,
                         RefRO<LocalToWorld>>())
            {
                if (squadStateComponent.ValueRO.unitState != UnitState.Squad)
                    continue;

                float epsilon = 0.0001f;
                var position1 = squadComponent.ValueRO.lastSquadPosition;
                var position2 = localToWorld.ValueRO.Position;
                if (math.abs(position1.x - position2.x) < epsilon
                    && math.abs(position1.y - position2.y) < epsilon)
                {
                    continue;
                }

                foreach ((RefRW<UnitMovableComponent> movableComponent,
                          RefRO<SquadUnitComponent> squadUnitComponent)
                         in SystemAPI.Query<
                             RefRW<UnitMovableComponent>,
                             RefRO<SquadUnitComponent>>())
                {
                    if (squadUnitComponent.ValueRO.squadIndex != squadComponent.ValueRO.squadIndex)
                        continue;
                    
                    foreach ((RefRO<SquadPointComponent> squadPointComponent,
                              RefRO<LocalToWorld> LocalToWorld)
                             in SystemAPI.Query<
                                 RefRO<SquadPointComponent>,
                                 RefRO<LocalToWorld>>())
                    {
                        if (squadPointComponent.ValueRO.squadIndex != squadUnitComponent.ValueRO.squadIndex
                            || squadPointComponent.ValueRO.pointIndex != squadUnitComponent.ValueRO.pointIndex)
                            continue;

                        movableComponent.ValueRW.targetPosition = new float2(LocalToWorld.ValueRO.Position.x, LocalToWorld.ValueRO.Position.y);
                    }
                }
            }
        }


        [BurstCompile]
        public void OnDestroy(ref SystemState state) {}
    }
}