using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

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
    }

    private void Update()
    {
        // Legacy Input System: Directly reads hardware, no Action Maps required.
        float x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right Arrows
        float y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down Arrows

        MoveInput = new Vector2(x, y).normalized;

        // 0 = Left Click, 1 = Right Click
        MeleeTriggered = Input.GetMouseButtonDown(0); 
        RangedTriggered = Input.GetMouseButtonDown(1);
    }
}