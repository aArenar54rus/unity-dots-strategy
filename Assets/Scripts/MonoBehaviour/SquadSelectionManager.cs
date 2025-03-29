using Arenar.PocketFantasyWar.InputSystem;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Arenar.PocketFantasyWar
{
    public class SquadSelectionManager : MonoBehaviour
    {
        [SerializeField] // TODO: remove after adding camera service
        private Camera camera;
        [SerializeField]
        private InputReaderManager inputReaderManager;

        [Space(10), Header("Action button settings")]
        [SerializeField]
        private float longPressTimeDetected = 0.8f;
        [SerializeField]
        private float dropShortButtonCounterTime = 1f;

        private Vector2 lastMouseScreenPosition;
        private int shortActionButtonPressCounter = 0;
        private float lastShortActionButtonPressTimer = 0f;

        
        private void Start()
        {
            if (inputReaderManager == null)
            {
                Debug.LogError("InputReaderManager is null!");
                return;
            }

            inputReaderManager.OnActionButtonDown += ActionButtonDownHandler;
            inputReaderManager.OnActionButtonUp += ActionButtonUpHandler;
        }
        
        private void Update()
        {
            if (lastShortActionButtonPressTimer < dropShortButtonCounterTime)
            {
                lastShortActionButtonPressTimer += Time.deltaTime;
                if (lastShortActionButtonPressTimer >= dropShortButtonCounterTime)
                {
                    if (shortActionButtonPressCounter == 1)
                        MakeShortActionTap();
                    shortActionButtonPressCounter = 0;
                }
            }
        }

        private void OnDestroy()
        {
            if (inputReaderManager == null)
                return;
            
            inputReaderManager.OnActionButtonDown -= ActionButtonDownHandler;
            inputReaderManager.OnActionButtonUp -= ActionButtonUpHandler;
        }

        private void ActionButtonDownHandler()
        {
            lastMouseScreenPosition = inputReaderManager.MouseScreenPosition;
        }

        private void ActionButtonUpHandler(float buttonLastPressedTime)
        {
            // if it's long tap
            if (buttonLastPressedTime >= longPressTimeDetected)
            {
                shortActionButtonPressCounter = 0;
                lastShortActionButtonPressTimer = dropShortButtonCounterTime;

                MakeLongActionTap();
            }
            else
            {
                lastShortActionButtonPressTimer = 0;
                shortActionButtonPressCounter++;

                if (shortActionButtonPressCounter == 2)
                {
                    MakeShortDoubleActionTap();
                    shortActionButtonPressCounter = 0;
                }
            }
            
            
        }


        private void MakeLongActionTap()
        {
            
        }


        private void MakeShortActionTap()
        {
            EntityManager defaultEntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<SquadComponent, SquadSelectedComponent, LocalToWorld>()
                .Build(defaultEntityManager);

            NativeArray<Entity> entities = entityQuery.ToEntityArray(Allocator.Temp);
            
            DeselectSquads();
            
            if (TrySelectSquads())
            {
                
            }
            else
            {
                // TrySelectBuilding();
            }
            
            
            void DeselectSquads()
            {
                NativeArray<SquadComponent> squads = entityQuery.ToComponentDataArray<SquadComponent>(Allocator.Temp);

                for (int i = 0; i < squads.Length; i++)
                {
                    Entity squadEntity = entities[i];
                    defaultEntityManager.SetComponentEnabled<SquadSelectedComponent>(squadEntity, false);
                    
                    SquadSelectedComponent selectedComponent = defaultEntityManager
                        .GetComponentData<SquadSelectedComponent>(squadEntity);
                    selectedComponent.onDeselectedEvent = true;
                    defaultEntityManager.SetComponentData(squadEntity, selectedComponent);
                }
            }
            
            bool TrySelectSquads()
            {
                bool isHaveSelectedSquads = false;
                
                EntityQuery physicsQuery = defaultEntityManager.CreateEntityQuery(typeof(PhysicsWorldSingleton));
                PhysicsWorldSingleton physicsWorldSingleton = physicsQuery.GetSingleton<PhysicsWorldSingleton>();
                CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;

                UnityEngine.Ray ray = camera.ScreenPointToRay(lastMouseScreenPosition);
                uint unitLayer = 1u << 6;
                RaycastInput raycastInput = new RaycastInput
                {
                    Start = ray.GetPoint(0.0f),
                    End = ray.GetPoint(9999.0f),
                    Filter = new CollisionFilter
                    {
                        BelongsTo = ~0u,
                        CollidesWith = unitLayer,
                        GroupIndex = 1,
                    },
                };

                if (collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit raycastHit)
                    && defaultEntityManager.HasComponent<SquadUnitComponent>(raycastHit.Entity))
                {
                    SquadUnitComponent unitComponent = defaultEntityManager.GetComponentData<SquadUnitComponent>(raycastHit.Entity);
                    
                    EntityQuery allSquadsQuery = new EntityQueryBuilder(Allocator.Temp)
                        .WithAll<SquadComponent>()
                        .Build(defaultEntityManager);
                    NativeArray<Entity> squadEntities = allSquadsQuery.ToEntityArray(Allocator.Temp);
                    NativeArray<SquadComponent> squads = allSquadsQuery.ToComponentDataArray<SquadComponent>(Allocator.Temp);

                    for (int j = 0; j < squads.Length; j++)
                    {
                        if (unitComponent.squadIndex != squads[j].squadIndex)
                            continue;
                        
                        defaultEntityManager.SetComponentEnabled<SquadSelectedComponent>(squadEntities[j], true);
                        
                        SquadSelectedComponent selectedComponent = defaultEntityManager
                            .GetComponentData<SquadSelectedComponent>(squadEntities[j]);
                        selectedComponent.onSelectedEvent = true;
                        defaultEntityManager.SetComponentData(squadEntities[j], selectedComponent);
                        
                        isHaveSelectedSquads = true;
                        break;
                    }
                }

                return isHaveSelectedSquads;
            }
        }

        private void MakeShortDoubleActionTap()
        {
            EntityManager defaultEntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery selectedSquadEntityQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<SquadComponent, SquadSelectedComponent, LocalToWorld>()
                .Build(defaultEntityManager);
            NativeArray<Entity> selectedSquadEntities = selectedSquadEntityQuery.ToEntityArray(Allocator.Temp);

            if (selectedSquadEntities.Length > 0)
            {
                SquadCommand();
                Debug.LogError("squadCommand");
            }
            


            void SquadCommand()
            {
                NativeArray<SquadComponent> squads = selectedSquadEntityQuery
                    .ToComponentDataArray<SquadComponent>(Allocator.Temp);

                var mousePosition = inputReaderManager.MousePosition;
                
                for (int i = 0; i < squads.Length; i++)
                {
                    SquadComponent squadComponent = squads[i];
                    Entity squadEntity = selectedSquadEntities[i];
                    
                    LocalToWorld localToWorld = defaultEntityManager.GetComponentData<LocalToWorld>(squadEntity);
                    localToWorld.Value.c3.xyz = new float3(mousePosition.x, mousePosition.y, 0);

                    
                    defaultEntityManager.SetComponentData(squadEntity, localToWorld);
                    break;
                }
            }
        }
        
        private NativeArray<Entity> GetSelectedSquads()
        {
            EntityManager defaultEntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<SquadComponent, SquadSelectedComponent, LocalToWorld>()
                .Build(defaultEntityManager);

            return entityQuery.ToEntityArray(Allocator.Temp);
        }
    }
}