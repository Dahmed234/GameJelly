using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


//Controls Lasso to collect fish.
// vertices placed down when holding left click. Once released, the vertices are used to make a collision shapeS
public class ReelController : MonoBehaviour
{
    public LinkedList<Vector3> netPoints;
    public PolygonCollider2D polygonCollider;
    public LineRenderer lineRenderer;
    
    public  const float DISTTHRESHOLD = 0.05f;
    public const float MAXLENGTH = 10f;
    private int MAXVERTICES = (int) ( MAXLENGTH / DISTTHRESHOLD);
    
    
    InputAction mousePosAction;
    InputAction mouseClickAction;
    private Camera _camera;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
        netPoints = new LinkedList<Vector3>();
        mousePosAction = InputSystem.actions.FindAction("lassoPosition");
        mouseClickAction = InputSystem.actions.FindAction("startLasso");

    }


    Vector3 TransformToWorld(Vector2 position)
    {
        Vector3 output = new Vector3(position.x,position.y ,_camera.nearClipPlane +2);
        return _camera.ScreenToWorldPoint(output);
    }

    // Update is called once per frame
    void Update()
    {
        if (mouseClickAction.IsPressed())
        {
            Vector2 mousePos = mousePosAction.ReadValue<Vector2>();
            
            // transform mouse vector to be aligned to camera position
            addVertex(mousePos);
            Debug.Log(mousePos);
        }
        
        
        if (mouseClickAction.WasReleasedThisFrame())
        {
            polygonCollider.points = netPoints.Select(p =>  new Vector2(p.x, p.y)).ToArray(); ;
            netPoints.Clear();
        }
        
        
        // redraw lasso
        lineRenderer.positionCount = netPoints.Count;
        lineRenderer.SetPositions(netPoints.Select(p => TransformToWorld(p)).ToArray());
    }
    
    
    public Boolean withinSelection(Vector3 position)
    {
        return polygonCollider.OverlapPoint(_camera.WorldToScreenPoint(position));
    }
    
    void addVertex(Vector3 pos)
    {
        if (netPoints.Count == 0 || (Vector3.Distance(netPoints.Last.Value, pos) > DISTTHRESHOLD))
        {
            if(netPoints.Count >= MAXVERTICES)
                netPoints.RemoveFirst();
            netPoints.AddLast(pos);
        }
       
    }
    

    
}
