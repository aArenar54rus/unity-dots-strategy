using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;


namespace Arenar.PocketFantasyWar
{
	public class OutlineSettingsAuthoring : MonoBehaviour
	{
		public SpriteRenderer entitySprite;
		public Color outlineColor = Color.white;
		public float outlineOffset = 0.00f;
	}
	
	class OutlineSettingsAuthoringBaker : Baker<OutlineSettingsAuthoring>
	{
		public override void Bake(OutlineSettingsAuthoring authoring)
		{
			Material newMaterial = new Material(authoring.entitySprite.material);
			authoring.entitySprite.material = newMaterial;
			
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent(entity, new OutlineSettingsComponent()
			{
				Color = new float4(authoring.outlineColor.r, authoring.outlineColor.g, authoring.outlineColor.b, authoring.outlineColor.a),
				Offset = authoring.outlineOffset   
			});
		}
	}
	
	[MaterialProperty("_OutlineColor")]
	[MaterialProperty("_Offset")]
	public struct OutlineSettingsComponent : IComponentData
	{
		public float4 Color; // Цвет обводки
		public float Offset; // Толщина обводки (0 = отключена)
	}
}
