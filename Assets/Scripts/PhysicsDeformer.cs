using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDeformer : MonoBehaviour
{
    [SerializeField] private DeformableMesh matzhoBall;

    private void OnCollisionStay(Collision collisionInfo)
    {
        foreach (var contact in collisionInfo.contacts)
        {
            matzhoBall.AddDepression(contact.point, 0.0f);
        }
    }
}
