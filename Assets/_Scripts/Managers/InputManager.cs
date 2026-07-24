using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public bool MeleeTriggered { get; private set; }
    public bool SpecialTriggered { get; private set; }
    public bool IsBlocking { get; private set; }
    public bool IsSprinting { get; private set; }
    public bool JumpTriggered { get; private set; }
    public bool DodgeTriggered { get; private set; }

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
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        MoveInput = new Vector2(x, y).normalized;

        MeleeTriggered = Input.GetMouseButtonDown(0);
        IsBlocking = Input.GetMouseButton(1);
        SpecialTriggered = Input.GetKeyDown(KeyCode.E);
        IsSprinting = Input.GetKey(KeyCode.LeftShift); 
        
        JumpTriggered = Input.GetKeyDown(KeyCode.Space);
        DodgeTriggered = Input.GetKeyDown(KeyCode.LeftAlt);
    }
}