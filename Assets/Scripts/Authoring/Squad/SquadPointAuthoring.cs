using Unity.Entities;
using UnityEngine;


namespace Arenar.PocketFantasyWar
{
	public class SquadPointAuthoring : MonoBehaviour
	{
		public int squadIndex;
		public int pointIndex;
		
		
		public class SquadPointAuthoringBaker : Baker<SquadPointAuthoring>
		{
			public override void Bake(SquadPointAuthoring squadPointAuthoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new SquadPointComponent()
				{
					squadIndex = squadPointAuthoring.squadIndex,
					pointIndex = squadPointAuthoring.pointIndex,
				});
			}
		}
	}
	

	public struct SquadPointComponent : IComponentData
	{
		public int squadIndex;
		public int pointIndex;
	}
}