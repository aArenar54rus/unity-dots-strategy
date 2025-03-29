using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


namespace Arenar.PocketFantasyWar.InputSystem
{
    public class InputReaderManager : MonoBehaviour
    {
        public event Action OnActionButtonDown;
        public event Action<float> OnActionButtonUp;
        
        
        [SerializeField]
        private Camera camera = default;
        
        private InputSystemActions inputActions;
        
        private EntityManager entityManager;
        
        private Entity inputEntity;

        private float mouseDownTime = 0;
        
        
        public Vector2 MousePosition { get; private set; } = new Vector2(0, 0);
        public Vector2 MouseScreenPosition { get; private set; } = new Vector2(0, 0);
        public bool LeftMouseClicked { get; private set; } = false;


        public void Awake()
        {
            Initialize();
        }

        public void Start()
        {
            entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            if (!entityManager.CreateEntityQuery(typeof(InputSingleton)).HasSingleton<InputSingleton>())
            {
                inputEntity = entityManager.CreateEntity(typeof(InputSingleton));
            }
            
            if (!entityManager.CreateEntityQuery(typeof(InputStrategySingleton)).HasSingleton<InputStrategySingleton>())
            {
                inputEntity = entityManager.CreateEntity(typeof(InputStrategySingleton));
            }
        }

        public void Initialize()
        {
            inputActions = new InputSystemActions();
            //inputActions.Player.Enable();
            inputActions.Strategy.Enable();
        }
        
        private void Update()
        {
            StrategyInput();
        }
        
        private void ActionInput()
        {
            Unity.Mathematics.float2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
            Unity.Mathematics.float2 lookInput = inputActions.Player.Look.ReadValue<Vector2>();
            bool jumpInput = inputActions.Player.Jump.triggered;

            if (entityManager.Exists(inputEntity))
            {
                entityManager.SetComponentData(inputEntity, new InputSingleton
                {
                    move = moveInput,
                    look = lookInput,
                    jump = jumpInput,
                });
            }
        }
        
        private void StrategyInput()
        {
            MousePosition = inputActions.Strategy.MousePosition.ReadValue<Vector2>();
            MouseScreenPosition = inputActions.Strategy.MousePositionScreen.ReadValue<Vector2>();
            MousePosition = camera.ScreenToWorldPoint(MousePosition);
            Unity.Mathematics.float2 leftTapInput = new float2(MousePosition.x, MousePosition.y);
            
            LeftMouseClicked = inputActions.Strategy.MouseClick.triggered;
            bool activePauseInput = inputActions.Strategy.ActivePause.triggered;
            
            
            if (inputActions.Strategy.MouseClick.WasPressedThisFrame())
            {
                OnActionButtonDown?.Invoke();

                mouseDownTime = Time.time;
            }
            else if (inputActions.Strategy.MouseClick.WasReleasedThisFrame())
            {
                var timeBetweenClicks = Time.time - mouseDownTime;
                
                OnActionButtonUp?.Invoke(timeBetweenClicks);
            }
            
            
            if (entityManager.Exists(inputEntity))
            {
                entityManager.SetComponentData(inputEntity, new InputStrategySingleton
                {
                    mousePosition = leftTapInput,
                    mouseClick = LeftMouseClicked,
                    activePause = activePauseInput,
                });
            }
        }
    }
    
    
    public struct InputSingleton : IComponentData
    {
        public Unity.Mathematics.float2 move;
        public Unity.Mathematics.float2 look;
        public bool jump;
    }

    
    public struct InputStrategySingleton : IComponentData
    {
        public Unity.Mathematics.float2 mousePosition;
        public bool mouseClick;
        public bool activePause;
    }
}