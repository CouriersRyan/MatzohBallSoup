using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class KeyDisplay : MonoBehaviour
{
    [SerializeField] private DisplayButton btnSpin;
    void OnSpin(InputValue value)
    {
        btnSpin.Pressed(value.Get<float>() > 0.5f);
    }
    
    [SerializeField] private DisplayButton btnLine;
    void OnLine(InputValue value)
    {
        btnLine.Pressed(value.Get<float>() > 0.5f);
    }
    
    [SerializeField] private DisplayButton btnVector;
    void OnVector(InputValue value)
    {
        btnVector.Pressed(value.Get<float>() > 0.5f);
    }
    
    [SerializeField] private DisplayButton btnEat;
    void OnEat(InputValue value)
    {
        btnEat.Pressed(value.Get<float>() > 0.5f);
    }
}
