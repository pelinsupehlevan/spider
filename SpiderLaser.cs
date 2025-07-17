//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SpiderLaser : MonoBehaviour
//{

//    RaycastHit hit;
//    public LayerMask obstacleLayer;
//    public LayerMask playerLayer;
//    private bool isHit;
//    private float range = 200f; 

//    private void Update()
//    {
//        if (Physics.Raycast(transform.position, transform.forward, out hit, range, obstacleLayer))
//        {
//            var lr = GetComponent<LineRenderer>();
//            lr.enabled = true;
//            isHit = true;
//            lr.SetPosition(0, transform.position);
//            lr.SetPosition(1, hit.point);

//        }
//        else
//        {
//            GetComponent<LineRenderer>().enabled = false;
//            isHit = false;
//        }

//        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, range);
//        foreach (RaycastHit h in hits)
//        {
//            if (isHit)
//            {
//                 if (((1 << h.collider.gameObject.layer) & playerLayer) != 0)
//                {
//                    h.collider.GetComponent<PlayerManager>()?.Die();
//                    break;
//                }
//            }

//        }
//    }


//}

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

        // Cast forward
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            lr.enabled = true;
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, hit.point);

            // Check if the thing hit is the player
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
