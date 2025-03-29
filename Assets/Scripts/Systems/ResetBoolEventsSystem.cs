using Unity.Burst;
using Unity.Entities;


namespace Arenar.PocketFantasyWar
{
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    partial struct ResetBoolEventsSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (RefRW<SquadSelectedComponent> squadSelected in SystemAPI.Query<RefRW<SquadSelectedComponent>>()
                         .WithPresent<SquadSelectedComponent>())
            {
                squadSelected.ValueRW.onSelectedEvent = false;
                squadSelected.ValueRW.onDeselectedEvent = false;
            }
        }
    }
}