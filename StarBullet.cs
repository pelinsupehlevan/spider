using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBullet : MonoBehaviour
{
    public float speed = 100f;
    private float lifetime = 2.5f;

    public bool enemyBullet = false;
    public GameObject hitEffect;

    private TrailRenderer trail;

    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
        if (trail != null)
        {
            trail.autodestruct = true;
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        lifetime -= Time.deltaTime;

        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (enemyBullet && other.CompareTag("Player"))
        {
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.TakeDamage(10f); 
            }

            if (hitEffect != null)
                Instantiate(hitEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
            return;
        }

        if (!enemyBullet && other.CompareTag("Enemy"))
        {
            Drone drone = other.GetComponent<Drone>() ?? other.GetComponentInParent<Drone>();
            if (drone != null)
            {
                drone.health -= 10f;
            }

            if (hitEffect != null)
                Instantiate(hitEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }

        if (!enemyBullet && other.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            if (hitEffect != null)
                Instantiate(hitEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
            return;
        }

        Destroy(gameObject);
    }
}
