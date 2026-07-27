using UnityEngine;

public class CellDoor : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public bool isOpen = false;
    public float openAngle = 90f;
    public float swingSpeed = 2f;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    private void Update()
    {
        // Smoothly swing the door open or closed based on its state
        Quaternion targetRotation = isOpen ? openRotation : closedRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * swingSpeed);
    }

    public void Interact(PlayerController player)
    {
        isOpen = !isOpen; // Toggle the door open/closed
        
        if (isOpen)
        {
            Debug.Log("Cell Door Opened!");
        }
        else
        {
            Debug.Log("Cell Door Closed!");
        }
    }

    public string GetInteractText()
    {
        return isOpen ? "Close Door" : "Open Cell Door";
    }
}