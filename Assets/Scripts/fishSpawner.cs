using System.Collections;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    
    public  GameObject fishPrefab;
    public BoxCollider boxCollider;
    private float randomTimer;
    public Score scoreKeeper;

    public ReelController reelController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        for (int i = 0; i < 30 ; i++)
        {
            SpawnFish();
        }
        
    }
    
    public  Vector3 RandomPointInBounds() {
        return new Vector3(
            Random.Range(boxCollider.bounds.min.x, boxCollider.bounds.max.x),
            boxCollider.bounds.center.y,
            Random.Range(boxCollider.bounds.min.z, boxCollider.bounds.max.z)
        );
    }


    void SpawnFish()
    {
        var fishInstance =  Instantiate(fishPrefab, RandomPointInBounds(), transform.rotation);
        Selectable selectable = fishInstance.GetComponent<Selectable>();
       selectable.reelController = reelController;
       fishInstance.GetComponent<FishMover>().fishSpawner = this;

       selectable.onFishCaught += scoreKeeper.OnFishCaught;
       

    }

    // Update is called once per frame
    void Update()
    {
        randomTimer -= Time.deltaTime;
        if (randomTimer < 0)
        {
            randomTimer = Random.Range(1, 3);
            SpawnFish();
        }


    }

    IEnumerator waiter()
    {
        int wait_time = Random.Range(10, 50);
        
        yield return new WaitForSeconds(wait_time);
        SpawnFish();
        
    }
    
}
