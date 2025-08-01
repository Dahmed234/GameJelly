using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = System.Diagnostics.Debug;


//Controls Lasso to collect fish.
// vertices placed down when holding left click. Once released, the vertices are used to make a collision shapeS
public class ReelController : MonoBehaviour
{
    LinkedList<Vector2> netPoints;
    public Vector2? collisionPoint;
    public PolygonCollider2D polygonCollider;
    public LineRenderer lineRenderer;
    
    
    private Gradient baseLineColor;
    public Gradient alternateLineColor;
    
    const float DISTTHRESHOLD = 1f;
    const float MAXLENGTH = 350f;
    private int MAXVERTICES = (int) ( MAXLENGTH / DISTTHRESHOLD);
    
    
    InputAction mousePosAction;
    InputAction mouseClickAction;
    private Camera _camera;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
        netPoints = new LinkedList<Vector2>();
        mousePosAction = InputSystem.actions.FindAction("lassoPosition");
        mouseClickAction = InputSystem.actions.FindAction("startLasso");
        baseLineColor = lineRenderer.colorGradient;

    }


    Vector3 TransformToWorld(Vector2 position)
    {
        Vector3 output = new Vector3(position.x,position.y ,_camera.nearClipPlane +2);
        return _camera.ScreenToWorldPoint(output);
    }


    
        
    // checks whether 3 points are in counter clockwise order
    //I copied this code from a random blogpost guys don't ask me how it works 
    private bool ccOrder(Vector2 a, Vector2 b, Vector2 c)
    {
        return ((c.y - a.y)*(b.x - a.x)) > ((b.y - a.y)*(c.x - a.x));
    }

    //checks whether 2 lines intersect
    // treat as magic tbh idk whats goin on guys
    private bool intersect(Vector2 a1, Vector2 a2,Vector2 b1, Vector2 b2)
    {
        return (ccOrder(a1, b1, b2) != ccOrder(a2, b1, b2)) && (ccOrder(a1, a2, b1) != ccOrder(a1, a2, b2));

    }

    // Update is called once per frame
    void Update()
    {
        if (mouseClickAction.IsPressed())
        {
            Vector2 mousePos = mousePosAction.ReadValue<Vector2>();
            while (addVertex(mousePos))
            {

                // if there are no current intersections
                if (collisionPoint == null && netPoints.Count > 4)
                {
                    //check whether new line closes the shape 
                    Vector2 z1 = netPoints.Last.Previous.Value;
                    Vector2 z2 = netPoints.Last.Value;



                    var a1 = netPoints.First;

                    //iterate through all line segments to check for any collisions
                    for (int i = 0; i < netPoints.Count - 4; i++)
                    {
                        var a2 = a1.Next.Value;
                        if (intersect(a1.Value, a2, z1, z2))
                        {
                            collisionPoint = a1.Value;
                            break;
                        }

                        a1 = a1.Next;
                    }
                }
            }



            //  Debug.Log(mousePos);
        }
        
        
        if (mouseClickAction.WasReleasedThisFrame())
        {
            
            //find whether this creates a closed shape
            // if shape is closed: trim edges and create polygon collider
            
            polygonCollider.points = netPoints.Select(p =>  new Vector2(p.x, p.y)).ToArray(); ;
            netPoints.Clear();
        }
        
        
        // render lasso
        if (collisionPoint != null) lineRenderer.colorGradient = alternateLineColor;
        else lineRenderer.colorGradient = baseLineColor;
        lineRenderer.positionCount = netPoints.Count;
        lineRenderer.SetPositions(netPoints.Select(p => TransformToWorld(p)).ToArray());
    }
    
    
    public Boolean withinSelection(Vector3 position)
    {
        return polygonCollider.OverlapPoint(_camera.WorldToScreenPoint(position));
    }
    
    Boolean addVertex(Vector2 pos)
    {
        Boolean added = false;
        if (netPoints.Count == 0 || (Vector2.Distance(netPoints.Last.Value, pos) > DISTTHRESHOLD))
        {
            added = true;

            if (netPoints.Count == 0)
            {
                netPoints.AddLast(pos);
                return added;
                
                
            }

            if (netPoints.Count >= MAXVERTICES)
            {
                var first = netPoints.First?.Value; 
                if (first == collisionPoint) 
                    collisionPoint = null;
                netPoints.RemoveFirst();
            }

            Vector2 newVertex = Vector2.MoveTowards(netPoints.Last.Value, pos, DISTTHRESHOLD); ;
            
            netPoints.AddLast(newVertex);
        }
        return added;
       
    }
    

    
}
