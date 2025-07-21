using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLaser : MonoBehaviour
{
    public LayerMask obstacleLayer;
    public LayerMask playerLayer;
    private float range = 200f;

    private void Update()
    {
        RaycastHit hit;
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.enabled = false;

        
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            lr.enabled = true;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit.point);

            if (((1 << hit.collider.gameObject.layer) & playerLayer) != 0)
            {
                PlayerManager playerManager = hit.collider.GetComponent<PlayerManager>();
                if (playerManager != null)
                {
                    playerManager.Die();
                }
            }
        }
    }
}
