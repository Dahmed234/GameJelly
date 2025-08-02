using System;
using UnityEngine;
using UnityEngine.InputSystem;

public  class Selectable : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    InputAction moveAction;
    Material unselectedMaterial;
    public Material selectedMaterial;
    
    public ReelController reelController;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unselectedMaterial = meshRenderer.material;
        Debug.Log($"fuck you bro");
        moveAction = InputSystem.actions.FindAction("move");

        //sets up connected events
        reelController.OnLassoCreation += TestLassoCreation;
        reelController.OnLassoPreview += TestLassoPreview;
    }


    void TestLassoPreview(object sender, ReelController.LassoCreationEventArg e)
    {
            onLassoPreview((e.withinBounds(transform.position)));
    }

    void TestLassoCreation(object sender, ReelController.LassoCreationEventArg e)
    {
        onLassoPreview(false);
        if (e.withinBounds(transform.position))
            onLassoCreation();
    }

    void onLassoCreation()
    {
        Debug.Log("I ATE");
    }

    void onLassoPreview(Boolean within)
    {
        if (within) meshRenderer.material = selectedMaterial;
        else meshRenderer.material = unselectedMaterial;
    }





    // Update is called once per frame
    void Update()
    {
       
    }
}
