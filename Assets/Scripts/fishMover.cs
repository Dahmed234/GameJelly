using UnityEngine;

public class FishMover : MonoBehaviour
{

    private Vector3 velocity;
    private Vector3 acceleration;
    

    private Vector3 targetPoint;
    private const float RANGE = 40f;
    public FishSpawner fishSpawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        targetPoint = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, targetPoint) <= RANGE)
            targetPoint = fishSpawner.RandomPointInBounds();
        acceleration = (targetPoint - transform.position).normalized * 100;
            velocity += acceleration * Time.deltaTime;
            velocity = Vector3.ClampMagnitude(velocity, 80);
        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(velocity);
        
        
    }
}
