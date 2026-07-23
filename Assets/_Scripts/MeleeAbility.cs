using UnityEngine;

public class MeleeAbility : MonoBehaviour, IAbility
{
    public void Execute()
    {
        // Later, we will trigger the Mixamo Sword Swing animation here
        Debug.Log("Executing Sword Swing!");
    }
}