using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    [Header("References")]
    private Transform player;
    public GameObject meshHolder; 
    public GameObject firePoint;
    public GameObject deathEffect;
    public GameObject bullet;

    [Header("Movement Settings")]
    public float followSpeed = 1f;
    public float followDistance = 5f;
    public float tooCloseDistance = 3f;
    public float retreatHeightOffset = 6f;
    public float verticalAdjustSpeed = 3f;
    public float orbitRadius = 15f;

    [Header("Combat Settings")]
    public float cooldownTime = 4f;
    private float cooldown;

    [Header("Health")]
    public float health = 100f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        HandleMovement();
        HandleShooting();
        HandleDeath();
    }

    private void HandleMovement()
    {
        Vector3 playerPosXZ = new Vector3(player.position.x, transform.position.y, player.position.z);
        float distance = Vector3.Distance(transform.position, playerPosXZ);

        if (distance > followDistance)
        {
            Follow(playerPosXZ);
        }
        else if (distance < tooCloseDistance)
        {
            Retreat();
        }
        else
        {
            Orbit();
        }
    }

    private void Follow(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, followSpeed * Time.deltaTime);
        LookAt(target);
    }

    private void Retreat()
    {
        Vector3 retreatPos = new Vector3(
            transform.position.x,
            Mathf.Lerp(transform.position.y, player.position.y + retreatHeightOffset, Time.deltaTime * verticalAdjustSpeed),
            transform.position.z
        );

        transform.position = Vector3.Lerp(transform.position, retreatPos, Time.deltaTime * followSpeed);
        LookAt(player.position);
    }

    private void Orbit()
    {
        Vector3 offset = (transform.position - player.position).normalized * orbitRadius;
        Vector3 orbitCenter = player.position + offset;

        float orbitSpeed = followSpeed * Random.Range(3f, 6f);
        transform.RotateAround(orbitCenter, Vector3.up, orbitSpeed * Time.deltaTime);

        LookAt(player.position);
    }

    private void LookAt(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            meshHolder.transform.rotation = Quaternion.Slerp(meshHolder.transform.rotation, targetRot, Time.deltaTime * 5f);
        }
    }

    private void HandleShooting()
    {
        if (cooldown > 0f)
        {
            cooldown -= Time.deltaTime;
            return;
        }

        cooldown = cooldownTime;

        Vector3 aimPoint = player.position + new Vector3(0, 8.5f, 0);
        Vector3 direction = aimPoint - firePoint.transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            firePoint.transform.rotation = Quaternion.LookRotation(direction);
        }

        Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
    }

    private void HandleDeath()
    {
        if (health <= 0f)
        {
            Instantiate(deathEffect, meshHolder.transform.position + Vector3.up * 1f, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
