using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportIndicator : MonoBehaviour
{
    [Header("Animation Settings")]
    public float pulseSpeed = 2f;
    public float pulseIntensity = 0.3f;
    public float rotationSpeed = 90f;

    [Header("Materials")]
    public Material validMaterial;
    public Material invalidMaterial;

    private Renderer indicatorRenderer;
    private Vector3 originalScale;
    private bool isValid = true;

    private void Awake()
    {
        indicatorRenderer = GetComponent<Renderer>();
        originalScale = transform.localScale;
    }

    private void Update()
    {
        AnimateIndicator();
    }

    private void AnimateIndicator()
    {
        // Pulse animation
        float pulseValue = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseIntensity;
        transform.localScale = originalScale * pulseValue;

        // Rotation animation
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public void SetValid(bool valid)
    {
        isValid = valid;

        if (indicatorRenderer != null)
        {
            if (valid && validMaterial != null)
            {
                indicatorRenderer.material = validMaterial;
            }
            else if (!valid && invalidMaterial != null)
            {
                indicatorRenderer.material = invalidMaterial;
            }
            else
            {
                // Fallback to color change if no materials are assigned
                indicatorRenderer.material.color = valid ? Color.green : Color.red;
            }
        }
    }

    private void OnEnable()
    {
        // Reset scale when enabled
        if (originalScale != Vector3.zero)
        {
            transform.localScale = originalScale;
        }
    }
}