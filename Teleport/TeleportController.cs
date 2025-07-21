using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportController : MonoBehaviour
{
    [Header("Teleport Settings")]
    public float teleportRange = 50f;
    public LayerMask teleportLayer = -1;
    //public Camera teleportPreviewCamera;
    //public RawImage teleportPreviewUI; 

    [Header("Crosshair")]
    public GameObject crosshair;
    public GameObject teleportCrosshair;

    private Camera playerCamera;
    private bool isTeleportMode = false;
    private Vector3 teleportTarget;
    private bool hasValidTarget = false;


    public LayerMask triggerLayer;
    public GameObject targetIndicator;
    public GameObject activeIndicator;

    private void Awake()
    {
        playerCamera = Camera.main;

        //if (teleportPreviewCamera != null)
        //    teleportPreviewCamera.gameObject.SetActive(false);

        //if (teleportPreviewUI != null)
        //{
        //    teleportPreviewUI.enabled = false;
        //}

        if (teleportCrosshair != null)
            teleportCrosshair.SetActive(false);
    }

    private void Update()
    {
        HandleTeleportInput();

        if (isTeleportMode)
        {
            UpdateTeleportTarget();
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
        //if (teleportPreviewCamera != null)
        //    teleportPreviewCamera.gameObject.SetActive(true);
        //if (teleportPreviewUI != null)
        //    teleportPreviewUI.enabled = true;
        //    teleportPreviewUI.gameObject.SetActive(true);

        if (targetIndicator != null && activeIndicator == null)
        {
            activeIndicator = Instantiate(targetIndicator);
            activeIndicator.SetActive(false); 
        }
    }

    private void UpdateTeleportTarget()
    {
        RaycastHit hit;
        Vector3 rayOrigin = playerCamera.transform.position;
        Vector3 rayDirection = playerCamera.transform.forward;

        Debug.DrawRay(rayOrigin, rayDirection * teleportRange, Color.green);

        if (Physics.Raycast(rayOrigin, rayDirection, out hit, teleportRange, teleportLayer))
        {
            if (hit.collider.GetComponent<Lava>() != null)
            {
                hasValidTarget = false;
                return;
            }

            if (IsValidTeleportLocation(hit.point))
            {
                teleportTarget = hit.point + Vector3.up * 0.1f;
                hasValidTarget = true;

                if (activeIndicator != null)
                {
                    activeIndicator.SetActive(true);
                    activeIndicator.transform.position = teleportTarget /*+ Vector3.up * 1.5f*/;
                }
            }
            else
            {
                hasValidTarget = false;

                if (activeIndicator != null)
                    activeIndicator.SetActive(false);
            }
        }

        else
        {
            hasValidTarget = false;
        }

        //if (hasValidTarget && teleportPreviewCamera != null)
        //{
        //    float eyeHeight = 8.25f;

        //    Vector3 targetPos = teleportTarget + Vector3.up * eyeHeight;
        //    Quaternion targetRot = playerCamera.transform.rotation;

        //    float smoothSpeed = 10f;
        //    teleportPreviewCamera.transform.position = Vector3.Lerp(
        //        teleportPreviewCamera.transform.position,
        //        targetPos,
        //        Time.deltaTime * smoothSpeed
        //    );

        //    teleportPreviewCamera.transform.rotation = Quaternion.Slerp(
        //        teleportPreviewCamera.transform.rotation,
        //        targetRot,
        //        Time.deltaTime * smoothSpeed
        //    );
        //}
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
            Vector3 direction = teleportTarget - transform.position;
            float distance = direction.magnitude;

            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, distance, triggerLayer);

            foreach (RaycastHit hit in hits)
            {
                Collider col = hit.collider;
                if (col.isTrigger)
                {
                    col.SendMessage("OnPlayerTeleportThrough", gameObject, SendMessageOptions.DontRequireReceiver);
                }
            }

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

        //if (teleportPreviewCamera != null)
        //    teleportPreviewCamera.gameObject.SetActive(false);

        //if (teleportPreviewUI != null)
        //{
        //    teleportPreviewUI.enabled = false;
        //    teleportPreviewUI.gameObject.SetActive(false); 
        //}

        if (activeIndicator != null)
        {
            Destroy(activeIndicator);
            activeIndicator = null;
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


