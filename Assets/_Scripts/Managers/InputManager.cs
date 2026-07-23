using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    
    private PlayerControls playerControls;
    
    public Vector2 MoveInput { get; private set; }
    public bool MeleeTriggered { get; private set; }
    public bool RangedTriggered { get; private set; }

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
        ConfigurePlatformSpecifics();
    }

    private void Update()
    {
        // Read movement continuously every frame
        MoveInput = playerControls.Gameplay.Move.ReadValue<Vector2>();

        // .triggered guarantees the input fires exactly once per button press
        MeleeTriggered = playerControls.Gameplay.MeleeAttack.triggered;
        RangedTriggered = playerControls.Gameplay.RangedAttack.triggered;
    }

    private void ConfigurePlatformSpecifics()
    {
        #if UNITY_ANDROID || UNITY_IOS
            Debug.Log("Mobile Build Detected: Touch controls will be enabled via UI.");
        #elif UNITY_STANDALONE_WIN || UNITY_WEBGL || UNITY_EDITOR
            Debug.Log("PC/Web Build Detected: Keyboard and Mouse controls active.");
        #endif
    }

    private void OnEnable()
    {
        playerControls?.Enable();
        playerControls?.Gameplay.Enable(); // Explicitly force the Gameplay map on
    }

    private void OnDisable()
    {
        playerControls?.Disable();
    }
}