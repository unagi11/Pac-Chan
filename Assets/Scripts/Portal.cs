using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal Out;

    public bool isOuting;

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Player") && isOuting is false)
        {
            Out.isOuting = true;
            other.transform.position = Out.transform.position;
        }
    }

    private void OnTriggerExit(Collider other) {
        
        if (other.CompareTag("Player") && isOuting)
        {
            isOuting = false;
        }
        
    }
}
