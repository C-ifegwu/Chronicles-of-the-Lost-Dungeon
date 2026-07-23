using UnityEngine;

public class RangedAbility : MonoBehaviour, IAbility
{
    public void Execute()
    {
        // Later, we will fetch the 3D arrow from the ObjectPool here
        Debug.Log("Firing 3D Arrow!");
    }
}