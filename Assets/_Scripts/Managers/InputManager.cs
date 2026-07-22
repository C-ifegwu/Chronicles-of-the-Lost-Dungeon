using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    private PlayerControls playerControls;
    public Vector2 MoveInput { get; private set; }
    public bool IsMeleeAttacking { get; private set; }
    public bool IsRangedAttacking { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerControls = new PlayerControls();
        
        // Listen to the input events
        playerControls.Gameplay.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        playerControls.Gameplay.Move.canceled += ctx => MoveInput = Vector2.zero;
        
        playerControls.Gameplay.MeleeAttack.started += ctx => IsMeleeAttacking = true;
        playerControls.Gameplay.MeleeAttack.canceled += ctx => IsMeleeAttacking = false;

        playerControls.Gameplay.RangedAttack.started += ctx => IsRangedAttacking = true;
        playerControls.Gameplay.RangedAttack.canceled += ctx => IsRangedAttacking = false;

        ConfigurePlatformSpecifics();
    }

    private void ConfigurePlatformSpecifics()
    {
        #if UNITY_ANDROID || UNITY_IOS
            Debug.Log("Mobile Build Detected: Touch controls will be enabled via UI.");
            // We will link the on-screen UI joysticks here later
        #elif UNITY_STANDALONE_WIN || UNITY_WEBGL || UNITY_EDITOR
            Debug.Log("PC/Web Build Detected: Keyboard and Mouse controls active.");
            // Touch UI will be hidden
        #endif
    }

    private void OnEnable()
    {
        playerControls?.Enable();
    }

    private void OnDisable()
    {
        playerControls?.Disable();
    }
}