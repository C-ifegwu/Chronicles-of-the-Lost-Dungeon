public interface IInteractable
{
    // What happens when the King interacts with this object?
    void Interact(PlayerController player);

    // Optional: What text should show up on the UI? (e.g., "Press E to Open")
    string GetInteractText();
}