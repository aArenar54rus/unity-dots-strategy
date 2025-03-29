using Unity.Entities;
using UnityEngine;


namespace Arenar.PocketFantasyWar
{
	public class UnitVisualAuthoring : MonoBehaviour
	{
		public GameObject unitVisual;
		
		[Space(10)]
		public GameObject unitSelectionVisual;
		public float unitSelectionVisualSize = 0.15f;
		
		
		public class UnitStateAuthoringBaker : Baker<UnitVisualAuthoring>
		{
			public override void Bake(UnitVisualAuthoring squadPointAuthoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new UnitVisualComponent()
				{
					unitVisualEntity = GetEntity(squadPointAuthoring.unitVisual, TransformUsageFlags.Dynamic),
					unitSelectionVisualEntity = GetEntity(squadPointAuthoring.unitSelectionVisual, TransformUsageFlags.Dynamic),
					selectedScale = squadPointAuthoring.unitSelectionVisualSize,
				});
			}
		}
	}
	
	
	public struct UnitVisualComponent : IComponentData
	{
		public Entity unitVisualEntity;
		public Entity unitSelectionVisualEntity;
		public float selectedScale;
	}
}