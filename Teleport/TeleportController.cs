//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class TeleportController : MonoBehaviour
//{
//    [Header("Teleport Settings")]
//    public float teleportRange = 50f;
//    public LayerMask teleportLayer = -1;
//    public Camera teleportPreviewCamera;
//    public RawImage teleportPreviewUI;

//    [Header("Crosshair")]
//    public GameObject crosshair;
//    public GameObject teleportCrosshair;

//    private Camera playerCamera;
//    private bool isTeleportMode = false;
//    private Vector3 teleportTarget;
//    private bool hasValidTarget = false;

//    private void Awake()
//    {
//        playerCamera = Camera.main;

//        if (teleportPreviewCamera != null)
//            teleportPreviewCamera.gameObject.SetActive(false);

//        if (teleportPreviewUI != null)
//            teleportPreviewUI.enabled = false;

//        if (teleportCrosshair != null)
//            teleportCrosshair.SetActive(false);
//    }

//    private void Update()
//    {
//        HandleTeleportInput();

//        if (isTeleportMode)
//        {
//            UpdateTeleportTarget();
//        }
//    }

//    private void HandleTeleportInput()
//    {
//        if (Input.GetMouseButtonDown(1))
//        {
//            EnterTeleportMode();
//        }

//        if (Input.GetMouseButtonUp(1) && isTeleportMode)
//        {
//            ExecuteTeleport();
//        }
//    }

//    private void EnterTeleportMode()
//    {
//        isTeleportMode = true;

//        if (crosshair != null)
//            crosshair.SetActive(false);
//        if (teleportCrosshair != null)
//            teleportCrosshair.SetActive(true);
//        if (teleportPreviewCamera != null)
//            teleportPreviewCamera.gameObject.SetActive(true);
//        if (teleportPreviewUI != null)
//            teleportPreviewUI.enabled = true;
//    }

//    private void UpdateTeleportTarget()
//    {
//        RaycastHit hit;
//        Vector3 rayOrigin = playerCamera.transform.position;
//        Vector3 rayDirection = playerCamera.transform.forward;

//        if (Physics.Raycast(rayOrigin, rayDirection, out hit, teleportRange, teleportLayer))
//        {
//            if (IsValidTeleportLocation(hit.point))
//            {
//                teleportTarget = hit.point + Vector3.up * 0.1f;
//                hasValidTarget = true;

//                if (teleportPreviewCamera != null)
//                {
//                    Vector3 camOffset = playerCamera.transform.forward * -2f + Vector3.up * 2f;
//                    teleportPreviewCamera.transform.position = teleportTarget + camOffset;
//                    teleportPreviewCamera.transform.LookAt(teleportTarget);
//                }
//            }
//            else
//            {
//                hasValidTarget = false;
//            }
//        }
//        else
//        {
//            hasValidTarget = false;
//        }
//    }

//    private bool IsValidTeleportLocation(Vector3 position)
//    {
//        float checkRadius = 0.5f;
//        float checkHeight = 2f;
//        Vector3 checkPosition = position + Vector3.up * checkHeight;

//        return !Physics.CheckSphere(checkPosition, checkRadius);
//    }

//    private void ExecuteTeleport()
//    {
//        if (hasValidTarget)
//        {
//            CharacterController controller = GetComponent<CharacterController>();
//            if (controller != null)
//            {
//                controller.enabled = false;
//                transform.position = teleportTarget;
//                controller.enabled = true;
//            }
//            else
//            {
//                transform.position = teleportTarget;
//            }
//        }

//        ExitTeleportMode();
//    }

//    private void ExitTeleportMode()
//    {
//        isTeleportMode = false;
//        hasValidTarget = false;

//        if (crosshair != null)
//            crosshair.SetActive(true);
//        if (teleportCrosshair != null)
//            teleportCrosshair.SetActive(false);
//        if (teleportPreviewCamera != null)
//            teleportPreviewCamera.gameObject.SetActive(false);
//        if (teleportPreviewUI != null)
//            teleportPreviewUI.enabled = false;
//    }

//    public void DisableTeleport()
//    {
//        if (isTeleportMode)
//        {
//            ExitTeleportMode();
//        }
//        this.enabled = false;
//    }

//    public void EnableTeleport()
//    {
//        this.enabled = true;
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportController : MonoBehaviour
{
    [Header("Teleport Settings")]
    public float teleportRange = 50f;
    public LayerMask teleportLayer = -1;
    public Camera teleportPreviewCamera;
    public Image teleportPreviewUI; 

    [Header("Crosshair")]
    public GameObject crosshair;
    public GameObject teleportCrosshair;

    private Camera playerCamera;
    private bool isTeleportMode = false;
    private Vector3 teleportTarget;
    private bool hasValidTarget = false;

    // Pulse parameters
    public float pulseSpeed = 2f;
    public float maxAlpha = 0.6f;
    private Color originalColor;

    private void Awake()
    {
        playerCamera = Camera.main;

        if (teleportPreviewCamera != null)
            teleportPreviewCamera.gameObject.SetActive(false);

        if (teleportPreviewUI != null)
        {
            teleportPreviewUI.enabled = false;
            originalColor = teleportPreviewUI.color;
        }

        if (teleportCrosshair != null)
            teleportCrosshair.SetActive(false);
    }

    private void Update()
    {
        HandleTeleportInput();

        if (isTeleportMode)
        {
            UpdateTeleportTarget();
            AnimatePulse();
        }
    }

    private void AnimatePulse()
    {
        if (teleportPreviewUI != null)
        {
            float alpha = maxAlpha * (1f + 0.5f * Mathf.Sin(Time.time * pulseSpeed));
            Color newColor = originalColor;
            newColor.a = alpha;
            teleportPreviewUI.color = newColor;
        }
    }

    private void HandleTeleportInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            EnterTeleportMode();
        }

        if (Input.GetMouseButtonUp(1) && isTeleportMode)
        {
            ExecuteTeleport();
        }
    }

    private void EnterTeleportMode()
    {
        isTeleportMode = true;

        if (crosshair != null)
            crosshair.SetActive(false);
        if (teleportCrosshair != null)
            teleportCrosshair.SetActive(true);
        if (teleportPreviewCamera != null)
            teleportPreviewCamera.gameObject.SetActive(true);
        if (teleportPreviewUI != null)
            teleportPreviewUI.enabled = true;
            teleportPreviewUI.gameObject.SetActive(true);
    }

    private void UpdateTeleportTarget()
    {
        RaycastHit hit;
        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward;

        Debug.DrawRay(rayOrigin, rayDirection * teleportRange, Color.green);

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, teleportRange, teleportLayer))
        {
            Debug.Log("Raycast HIT: " + hit.collider.name + " at " + hit.point);

            if (IsValidTeleportLocation(hit.point))
            {
                teleportTarget = hit.point + Vector3.up * 0.1f;
                hasValidTarget = true;
            }
            else
            {
                hasValidTarget = false;
            }
        }
        else
        {
            Debug.LogWarning("Raycast FAILED. Nothing hit in teleportLayer.");
            hasValidTarget = false;
        }

        if (hasValidTarget && teleportPreviewCamera != null)
        {
            float eyeHeight = 8.25f;

            Vector3 targetPos = teleportTarget + Vector3.up * eyeHeight;
            Quaternion targetRot = playerCamera.transform.rotation;

            float smoothSpeed = 10f;
            teleportPreviewCamera.transform.position = Vector3.Lerp(
                teleportPreviewCamera.transform.position,
                targetPos,
                Time.deltaTime * smoothSpeed
            );

            teleportPreviewCamera.transform.rotation = Quaternion.Slerp(
                teleportPreviewCamera.transform.rotation,
                targetRot,
                Time.deltaTime * smoothSpeed
            );
        }
    }

    private bool IsValidTeleportLocation(Vector3 position)
    {
        float checkRadius = 0.5f;
        float checkHeight = 2f;
        Vector3 checkPosition = position + Vector3.up * checkHeight;

        return !Physics.CheckSphere(checkPosition, checkRadius);
    }

    private void ExecuteTeleport()
    {
        if (hasValidTarget)
        {
            CharacterController controller = GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
                transform.position = teleportTarget;
                controller.enabled = true;
            }
            else
            {
                transform.position = teleportTarget;
            }
        }

        ExitTeleportMode();
    }

    private void ExitTeleportMode()
    {
        isTeleportMode = false;
        hasValidTarget = false;

        if (crosshair != null)
            crosshair.SetActive(true);

        if (teleportCrosshair != null)
            teleportCrosshair.SetActive(false);

        if (teleportPreviewCamera != null)
            teleportPreviewCamera.gameObject.SetActive(false);

        if (teleportPreviewUI != null)
        {
            teleportPreviewUI.enabled = false;
            teleportPreviewUI.color = originalColor;
            teleportPreviewUI.gameObject.SetActive(false); // reset alpha on exit
        }
    }

    public void DisableTeleport()
    {
        if (isTeleportMode)
        {
            ExitTeleportMode();
        }
        this.enabled = false;
    }

    public void EnableTeleport()
    {
        this.enabled = true;
    }
}


