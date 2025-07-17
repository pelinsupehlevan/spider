using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandControl : MonoBehaviour
{
    RaycastHit hit;

    public LayerMask obstacleLayer;
    public Vector3 offset = Vector3.zero;
    public GameObject starBullet;
    public Transform firePoint;
    public GameObject hand;

    public float rotationSpeed = 10f;

    private Animator wandAnimator;
    private float cooldown = 0.255f;

    public AudioClip wandshot;

    private void Awake()
    {
        wandAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        AimWand();

        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Trigger wand animation only
            wandAnimator.SetTrigger("Shoot");
        }
    }

    private void AimWand()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, obstacleLayer))
        {
            Vector3 direction = (hit.point - hand.transform.position).normalized;

            float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float pitch = Mathf.Asin(direction.y) * Mathf.Rad2Deg;

            float minPitch = -45f;
            float maxPitch = 45f;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            Quaternion targetRotation = Quaternion.Euler(-pitch, yaw, 0);
            targetRotation *= Quaternion.Euler(offset);

            hand.transform.rotation = Quaternion.Slerp(hand.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    //  Call this from Animation Event (on the correct frame)
    public void ShootBullet()
    {
        if (cooldown <= 0)
        {
            Quaternion cameraForward = Quaternion.LookRotation(Camera.main.transform.forward);
            Instantiate(starBullet, firePoint.position, cameraForward);
            cooldown = 0.25f;

            GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().PlayOneShot(wandshot);    
        }

    }
}
