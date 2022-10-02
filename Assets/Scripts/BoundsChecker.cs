using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsChecker : MonoBehaviour
{
    public bool isMatzohOutOfBounds = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Matzoh"))
        {
            isMatzohOutOfBounds = true;
            Debug.Log("Matzoh overflow.");
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Matzoh"))
        {
            isMatzohOutOfBounds = false;
            Debug.Log("Matzoh out.");
        }
    }
}
