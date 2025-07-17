using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Column : MonoBehaviour
{
    public Transform checker;
    public LayerMask playerLayer;
    public float radius = 0.5f;
    private Vector3 velocity;

    private void Update()
    { 
        if (Physics.CheckBox(checker.position, new Vector3(radius,2,radius), Quaternion.identity, playerLayer))
        {
            velocity.y -= Time.deltaTime;
            transform.Translate(velocity);
        }
    }

}
