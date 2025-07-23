//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class TeleportController : MonoBehaviour
//{
//    [Header("Teleport Settings")]
//    public float teleportRange = 50f;
//    private float cooldown = 0f;
//    private float cooldownTimer = 2f;
//    private bool isOnCooldown = false;
//    public LayerMask teleportLayer = -1;
//    //public Camera teleportPreviewCamera;
//    //public RawImage teleportPreviewUI; 


//    [Header("Crosshair")]
//    public GameObject crosshair;
//    public GameObject teleportCrosshair;
//    public Image cooldownRadial;

//    private Camera playerCamera;
//    private bool isTeleportMode = false;
//    private Vector3 teleportTarget;
//    private bool hasValidTarget = false;


//    public LayerMask triggerLayer;
//    public GameObject targetIndicator;
//    public GameObject activeIndicator;

//    private void Awake()
//    {
//        playerCamera = Camera.main;

//        //if (teleportPreviewCamera != null)
//        //    teleportPreviewCamera.gameObject.SetActive(false);

//        //if (teleportPreviewUI != null)
//        //{
//        //    teleportPreviewUI.enabled = false;
//        //}

//        if (teleportCrosshair != null)
//            teleportCrosshair.SetActive(false);

//        if (cooldownRadial != null)
//            cooldownRadial.gameObject.SetActive(false);

//        cooldown = 0f;
//    }

//    //private void Update()
//    //{
//    //    UpdateCooldown();
//    //    HandleTeleportInput();

//    //    if (isTeleportMode)
//    //    {
//    //        UpdateTeleportTarget();
//    //    }

//    //    if (Input.GetMouseButton(1) && isOnCooldown && cooldownRadial != null)
//    //    {
//    //        cooldownRadial.gameObject.SetActive(true);
//    //    }
//    //    else if (cooldownRadial != null)
//    //    {
//    //        cooldownRadial.gameObject.SetActive(false);
//    //    }
//    //}

//    //private void Update()
//    //{
//    //    UpdateCooldown();
//    //    HandleTeleportInput();

//    //    if (isTeleportMode)
//    //    {
//    //        UpdateTeleportTarget();
//    //    }

//    //    if (cooldownRadial != null)
//    //    {
//    //        if (Input.GetMouseButton(1) && isOnCooldown)
//    //        {
//    //            if (!cooldownRadial.gameObject.activeSelf)
//    //            {
//    //                Debug.Log("Showing cooldown radial");
//    //                cooldownRadial.gameObject.SetActive(true);
//    //            }
//    //        }
//    //        else
//    //        {
//    //            if (cooldownRadial.gameObject.activeSelf)
//    //            {
//    //                Debug.Log("Hiding cooldown radial");
//    //                cooldownRadial.gameObject.SetActive(false);
//    //            }
//    //        }
//    //    }
//    //}

//    private void Update()
//    {
//        UpdateCooldown();
//        HandleTeleportInput();

//        if (isTeleportMode)
//        {
//            UpdateTeleportTarget();
//        }

//        if (cooldownRadial != null)
//        {
//            bool isHoldingRightClick = Input.GetMouseButton(1);
//            cooldownRadial.gameObject.SetActive(isHoldingRightClick);

//            if (isOnCooldown)
//            {
//                cooldownRadial.fillAmount = 1f - Mathf.Clamp01(cooldownTimer / cooldown);
//            }
//            else
//            {
//                cooldownRadial.fillAmount = 1f; 
//            }
//        }
//    }




//    private void HandleTeleportInput()
//    {


//        if (Input.GetMouseButtonDown(1) && !isOnCooldown )
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
//        //if (teleportPreviewCamera != null)
//        //    teleportPreviewCamera.gameObject.SetActive(true);
//        //if (teleportPreviewUI != null)
//        //    teleportPreviewUI.enabled = true;
//        //    teleportPreviewUI.gameObject.SetActive(true);

//        if (targetIndicator != null && activeIndicator == null)
//        {
//            activeIndicator = Instantiate(targetIndicator);
//            activeIndicator.SetActive(false); 
//        }
//    }

//    private void UpdateTeleportTarget()
//    {
//        RaycastHit hit;
//        Vector3 rayOrigin = playerCamera.transform.position;
//        Vector3 rayDirection = playerCamera.transform.forward;

//        Debug.DrawRay(rayOrigin, rayDirection * teleportRange, Color.green);

//        if (Physics.Raycast(rayOrigin, rayDirection, out hit, teleportRange, teleportLayer))
//        {
//            if (hit.collider.GetComponent<Lava>() != null)
//            {
//                hasValidTarget = false;
//                return;
//            }

//            if (IsValidTeleportLocation(hit.point))
//            {
//                teleportTarget = hit.point + Vector3.up * 0.1f;
//                hasValidTarget = true;

//                if (activeIndicator != null)
//                {
//                    activeIndicator.SetActive(true);
//                    activeIndicator.transform.position = teleportTarget /*+ Vector3.up * 1.5f*/;
//                }
//            }
//            else
//            {
//                hasValidTarget = false;

//                if (activeIndicator != null)
//                    activeIndicator.SetActive(false);
//            }
//        }

//        else
//        {
//            hasValidTarget = false;
//        }

//        //if (hasValidTarget && teleportPreviewCamera != null)
//        //{
//        //    float eyeHeight = 8.25f;

//        //    Vector3 targetPos = teleportTarget + Vector3.up * eyeHeight;
//        //    Quaternion targetRot = playerCamera.transform.rotation;

//        //    float smoothSpeed = 10f;
//        //    teleportPreviewCamera.transform.position = Vector3.Lerp(
//        //        teleportPreviewCamera.transform.position,
//        //        targetPos,
//        //        Time.deltaTime * smoothSpeed
//        //    );

//        //    teleportPreviewCamera.transform.rotation = Quaternion.Slerp(
//        //        teleportPreviewCamera.transform.rotation,
//        //        targetRot,
//        //        Time.deltaTime * smoothSpeed
//        //    );
//        //}
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
//            Vector3 direction = teleportTarget - transform.position;
//            float distance = direction.magnitude;

//            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, distance, triggerLayer);

//            foreach (RaycastHit hit in hits)
//            {
//                Collider col = hit.collider;
//                if (col.isTrigger)
//                {
//                    col.SendMessage("OnPlayerTeleportThrough", gameObject, SendMessageOptions.DontRequireReceiver);
//                }
//            }

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

//        StartCooldown();
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

//        //if (teleportPreviewCamera != null)
//        //    teleportPreviewCamera.gameObject.SetActive(false);

//        //if (teleportPreviewUI != null)
//        //{
//        //    teleportPreviewUI.enabled = false;
//        //    teleportPreviewUI.gameObject.SetActive(false); 
//        //}

//        if (activeIndicator != null)
//        {
//            Destroy(activeIndicator);
//            activeIndicator = null;
//        }
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

//    public float GetCooldownRemaining()
//    {
//        return Mathf.Max(cooldown, 0f);
//    }

//    //private void UpdateCooldown()
//    //{
//    //    if (isOnCooldown)
//    //    {
//    //        cooldown -= Time.deltaTime;

//    //        if (cooldownRadial != null && cooldownRadial.gameObject.activeSelf)
//    //        {
//    //            cooldownRadial.fillAmount = Mathf.Clamp01(1 - (cooldown / cooldownTimer));
//    //        }

//    //        if (cooldown <= 0f)
//    //        {
//    //            isOnCooldown = false;
//    //            cooldown = 0f;

//    //            if (cooldownRadial != null)
//    //                cooldownRadial.fillAmount = 1f;
//    //        }
//    //    }
//    //}

//    private void UpdateCooldown()
//    {
//        if (isOnCooldown)
//        {
//            cooldownTimer -= Time.deltaTime;

//            if (cooldownRadial != null && cooldownRadial.gameObject.activeSelf)
//            {
//                cooldownRadial.fillAmount = 1 - Mathf.Clamp01(cooldownTimer / cooldown);
//            }

//            if (cooldownTimer <= 0f)
//            {
//                isOnCooldown = false;
//                cooldownTimer = 0f;

//                if (cooldownRadial != null)
//                    cooldownRadial.fillAmount = 1f;
//            }
//        }
//    }



//    private void StartCooldown()
//    {
//        cooldown = cooldownTimer; 
//        isOnCooldown = true;

//        if (cooldownRadial != null)
//            cooldownRadial.fillAmount = 0f;
//    }
//}


//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;

//public class TeleportController : MonoBehaviour
//{
//    [Header("Teleport Settings")]
//    public float teleportRange = 50f;
//    public float cooldownDuration = 1f; 
//    private float cooldownTimer = 0f;
//    private bool isOnCooldown = false;
//    public LayerMask teleportLayer = -1;

//    [Header("Crosshair")]
//    public GameObject crosshair;
//    public GameObject teleportCrosshair;
//    public Image cooldownRadial;

//    private Camera playerCamera;
//    private bool isTeleportMode = false;
//    private Vector3 teleportTarget;
//    private bool hasValidTarget = false;

//    public LayerMask triggerLayer;
//    public GameObject targetIndicator;
//    public GameObject activeIndicator;

//    private void Awake()
//    {
//        playerCamera = Camera.main;

//        if (teleportCrosshair != null)
//            teleportCrosshair.SetActive(false);

//        if (cooldownRadial != null)
//        {
//            cooldownRadial.gameObject.SetActive(false);
//            cooldownRadial.fillAmount = 1f;
//        }
//    }

//    private void Update()
//    {
//        UpdateCooldown();
//        HandleTeleportInput();

//        if (isTeleportMode)
//            UpdateTeleportTarget();

//        if (cooldownRadial != null)
//        {
//            bool showRadial = Input.GetMouseButton(1) && isOnCooldown;
//            cooldownRadial.gameObject.SetActive(showRadial);

//            if (showRadial)
//                cooldownRadial.fillAmount = 1f - Mathf.Clamp01(cooldownTimer / cooldownDuration);
//        }
//    }

//    private void HandleTeleportInput()
//    {
//        if (Input.GetMouseButtonDown(1) && !isOnCooldown)
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

//        if (crosshair != null) crosshair.SetActive(false);
//        if (teleportCrosshair != null) teleportCrosshair.SetActive(true);

//        if (targetIndicator != null && activeIndicator == null)
//        {
//            activeIndicator = Instantiate(targetIndicator);
//            activeIndicator.SetActive(false);
//        }
//    }

//    private void UpdateTeleportTarget()
//    {
//        RaycastHit hit;
//        Vector3 rayOrigin = playerCamera.transform.position;
//        Vector3 rayDirection = playerCamera.transform.forward;

//        Debug.DrawRay(rayOrigin, rayDirection * teleportRange, Color.green);

//        if (Physics.Raycast(rayOrigin, rayDirection, out hit, teleportRange, teleportLayer))
//        {
//            if (hit.collider.GetComponent<Lava>() != null)
//            {
//                hasValidTarget = false;
//                return;
//            }

//            if (IsValidTeleportLocation(hit.point))
//            {
//                teleportTarget = hit.point + Vector3.up * 0.1f;
//                hasValidTarget = true;

//                if (activeIndicator != null)
//                {
//                    activeIndicator.SetActive(true);
//                    activeIndicator.transform.position = teleportTarget;
//                }
//            }
//            else
//            {
//                hasValidTarget = false;
//                if (activeIndicator != null) activeIndicator.SetActive(false);
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
//            Vector3 direction = teleportTarget - transform.position;
//            float distance = direction.magnitude;

//            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, distance, triggerLayer);
//            foreach (RaycastHit hit in hits)
//            {
//                if (hit.collider.isTrigger)
//                    hit.collider.SendMessage("OnPlayerTeleportThrough", gameObject, SendMessageOptions.DontRequireReceiver);
//            }

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

//        StartCooldown();
//        ExitTeleportMode();
//    }

//    private void ExitTeleportMode()
//    {
//        isTeleportMode = false;
//        hasValidTarget = false;

//        if (crosshair != null) crosshair.SetActive(true);
//        if (teleportCrosshair != null) teleportCrosshair.SetActive(false);

//        if (activeIndicator != null)
//        {
//            Destroy(activeIndicator);
//            activeIndicator = null;
//        }
//    }

//    private void StartCooldown()
//    {
//        cooldownTimer = cooldownDuration;
//        isOnCooldown = true;

//        if (cooldownRadial != null)
//            cooldownRadial.fillAmount = 0f;
//    }

//    private void UpdateCooldown()
//    {
//        if (!isOnCooldown) return;

//        cooldownTimer -= Time.deltaTime;

//        if (cooldownTimer <= 0f)
//        {
//            cooldownTimer = 0f;
//            isOnCooldown = false;

//            if (cooldownRadial != null)
//                cooldownRadial.fillAmount = 1f;
//        }
//    }

//    public void DisableTeleport()
//    {
//        if (isTeleportMode) ExitTeleportMode();
//        this.enabled = false;
//    }

//    public void EnableTeleport()
//    {
//        this.enabled = true;
//    }

//    public float GetCooldownRemaining()
//    {
//        return Mathf.Max(cooldownTimer, 0f);
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportController : MonoBehaviour
{
    [Header("Teleport Settings")]
    public float teleportRange = 200f;
    public float cooldownDuration = 1f;
    private float cooldownRemaining = 0f;

    public LayerMask teleportLayer = -1;
    public LayerMask triggerLayer;

    [Header("Crosshair")]
    public GameObject crosshair;
    public GameObject teleportCrosshair;
    public Image cooldownRadial;

    public GameObject targetIndicator;
    private GameObject activeIndicator;

    private Camera playerCamera;
    private bool isTeleportMode = false;
    private Vector3 teleportTarget;
    private bool hasValidTarget = false;

    private bool IsCooldownActive => cooldownRemaining > 0f;

    private void Awake()
    {
        playerCamera = Camera.main;

        if (teleportCrosshair != null)
            teleportCrosshair.SetActive(false);

        if (cooldownRadial != null)
        {
            cooldownRadial.fillAmount = 1f;
            cooldownRadial.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        UpdateCooldown();
        HandleTeleportInput();

        if (isTeleportMode)
            UpdateTeleportTarget();

        if (cooldownRadial != null)
        {
            cooldownRadial.gameObject.SetActive(Input.GetMouseButton(1) && IsCooldownActive);
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
            if (!IsCooldownActive)
                ExecuteTeleport();
            else
                ExitTeleportMode(); 
        }
    }

    private void EnterTeleportMode()
    {
        isTeleportMode = true;

        if (crosshair != null)
            crosshair.SetActive(false);

        if (teleportCrosshair != null)
            teleportCrosshair.SetActive(true);

        if (targetIndicator != null && activeIndicator == null)
        {
            activeIndicator = Instantiate(targetIndicator);
            activeIndicator.SetActive(false);
        }
    }

    private void UpdateTeleportTarget()
    {
        RaycastHit hit;
        Vector3 origin = playerCamera.transform.position;
        Vector3 direction = playerCamera.transform.forward;

        if (Physics.Raycast(origin, direction, out hit, teleportRange, teleportLayer))
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
                    activeIndicator.transform.position = teleportTarget;
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
            if (activeIndicator != null)
                activeIndicator.SetActive(false);
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
            Vector3 direction = teleportTarget - transform.position;
            float distance = direction.magnitude;

            RaycastHit[] hits = Physics.RaycastAll(transform.position, direction.normalized, distance, triggerLayer);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.isTrigger)
                    hit.collider.SendMessage("OnPlayerTeleportThrough", gameObject, SendMessageOptions.DontRequireReceiver);
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

        StartCooldown();
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

        if (activeIndicator != null)
        {
            Destroy(activeIndicator);
            activeIndicator = null;
        }
    }

    private void StartCooldown()
    {
        cooldownRemaining = cooldownDuration;

        if (cooldownRadial != null)
            cooldownRadial.fillAmount = 0f;
    }

    private void UpdateCooldown()
    {
        if (IsCooldownActive)
        {
            cooldownRemaining -= Time.deltaTime;

            if (cooldownRadial != null)
                cooldownRadial.fillAmount = 1f - Mathf.Clamp01(cooldownRemaining / cooldownDuration);

            if (cooldownRemaining <= 0f)
            {
                cooldownRemaining = 0f;
                if (cooldownRadial != null)
                    cooldownRadial.fillAmount = 1f;
            }
        }
    }

    public void DisableTeleport()
    {
        if (isTeleportMode)
            ExitTeleportMode();

        enabled = false;
    }

    public void EnableTeleport()
    {
        enabled = true;
    }

    public float GetCooldownRemaining()
    {
        return Mathf.Max(cooldownRemaining, 0f);
    }
}
