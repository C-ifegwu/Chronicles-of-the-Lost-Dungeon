using UnityEngine;
using UnityEngine.InputSystem;

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
        float x = 0;
        float y = 0;

        // Direct hardware polling: 100% immune to Action Map asset bugs
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) y = 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) y = -1;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) x = -1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x = 1;
        }

        MoveInput = new Vector2(x, y).normalized;

        if (Mouse.current != null)
        {
            MeleeTriggered = Mouse.current.leftButton.wasPressedThisFrame;
            RangedTriggered = Mouse.current.rightButton.wasPressedThisFrame;
        }
    }
}