using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GayAssScript : MonoBehaviour
{
    
    InputAction moveAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log($"fuck you bro");
        moveAction = InputSystem.actions.FindAction("move");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        this.transform.Translate(moveValue.normalized * (5 * Time.deltaTime) , Space.World);
    }
}
