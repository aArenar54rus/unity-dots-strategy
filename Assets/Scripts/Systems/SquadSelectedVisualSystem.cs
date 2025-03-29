using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;


namespace Arenar.PocketFantasyWar
{
    partial struct SquadSelectedVisualSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            foreach ((RefRO<SquadSelectedComponent> selected, RefRO<SquadComponent> squad)
                     in SystemAPI.Query<RefRO<SquadSelectedComponent>, RefRO<SquadComponent>>()
                         .WithDisabled<SquadSelectedComponent>())
            {
                foreach ((RefRO<SquadUnitComponent> squadUnit, RefRO<UnitVisualComponent> unitVisual)
                         in SystemAPI.Query<RefRO<SquadUnitComponent>, RefRO<UnitVisualComponent>>())
                {
                    if (squad.ValueRO.squadIndex != squadUnit.ValueRO.squadIndex)
                        continue;

                    RefRW<LocalTransform> unitSelectedVisualTransform =
                        SystemAPI.GetComponentRW<LocalTransform>(unitVisual.ValueRO.unitSelectionVisualEntity);

                    unitSelectedVisualTransform.ValueRW.Scale = 0;
                }
            }
        }


        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((RefRO<SquadSelectedComponent> selected, RefRO<SquadComponent> squad)
                     in SystemAPI.Query<RefRO<SquadSelectedComponent>, RefRO<SquadComponent>>())
            {
                if (!selected.ValueRO.onSelectedEvent)
                    continue;
                
                foreach ((RefRO<SquadUnitComponent> squadUnit, RefRO<UnitVisualComponent> unitVisual)
                         in SystemAPI.Query<RefRO<SquadUnitComponent>, RefRO<UnitVisualComponent>>())
                {
                    if (squad.ValueRO.squadIndex != squadUnit.ValueRO.squadIndex)
                        continue;

                    RefRW<LocalTransform> unitSelectedVisualTransform =
                        SystemAPI.GetComponentRW<LocalTransform>(unitVisual.ValueRO.unitSelectionVisualEntity);

                    unitSelectedVisualTransform.ValueRW.Scale = unitVisual.ValueRO.selectedScale;
                }
            }
            
            foreach ((RefRO<SquadSelectedComponent> selected, RefRO<SquadComponent> squad)
                     in SystemAPI.Query<RefRO<SquadSelectedComponent>, RefRO<SquadComponent>>().WithDisabled<SquadSelectedComponent>())
            {
                if (!selected.ValueRO.onDeselectedEvent)
                    continue;
                
                foreach ((RefRO<SquadUnitComponent> squadUnit, RefRO<UnitVisualComponent> unitVisual)
                         in SystemAPI.Query<RefRO<SquadUnitComponent>, RefRO<UnitVisualComponent>>())
                {
                    if (squad.ValueRO.squadIndex != squadUnit.ValueRO.squadIndex)
                        continue;

                    RefRW<LocalTransform> unitSelectedVisualTransform =
                        SystemAPI.GetComponentRW<LocalTransform>(unitVisual.ValueRO.unitSelectionVisualEntity);

                    unitSelectedVisualTransform.ValueRW.Scale = 0;
                }
            }
        }


        [BurstCompile]
        public void OnDestroy(ref SystemState state) {}
    }
}