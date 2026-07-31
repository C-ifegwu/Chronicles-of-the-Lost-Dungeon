using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    [Header("Animation Settings")]
    public float moveSpeed = 2f;
    public float disappearTimer = 1f;
    
    private TextMeshPro textMesh;
    private Color textColor;
    private Camera mainCamera;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        mainCamera = Camera.main;

        if (textMesh != null)
        {
            textColor = textMesh.color;
        }
        
        Destroy(gameObject, disappearTimer);
    }

    public void Setup(int damageAmount)
    {
        if (textMesh != null)
        {
            textMesh.text = damageAmount.ToString();
        }
    }

    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        if (textMesh != null)
        {
            textColor.a -= (Time.deltaTime / disappearTimer);
            textMesh.color = textColor;
        }

        if (mainCamera != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
        }
    }
}