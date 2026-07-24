using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public bool MeleeTriggered { get; private set; }
    public bool SpecialTriggered { get; private set; }
    public bool IsBlocking { get; private set; }

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
        // Movement
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        MoveInput = new Vector2(x, y).normalized;

        // Combat Actions (Old Input System)
        MeleeTriggered = Input.GetMouseButtonDown(0); // Left Click to swing
        IsBlocking = Input.GetMouseButton(1);         // Hold Right Click to block
        SpecialTriggered = Input.GetKeyDown(KeyCode.E); // Press 'E' for Special Attack
    }
}