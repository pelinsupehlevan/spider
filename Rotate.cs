using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 rotationAxis;

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(rotationAxis * speed * Time.deltaTime);
    }
}
