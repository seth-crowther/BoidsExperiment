using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myDest : MonoBehaviour
{
    public Vector3 myTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Environment"))
        {
            Destroy(gameObject);
        }
    }
}
