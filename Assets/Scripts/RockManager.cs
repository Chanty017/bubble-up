using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RockManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public List<Transform> spawnPoints = new List<Transform>();
    public int distance;
    Vector3 StartPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Spawn());
        

    }


    IEnumerator Spawn()
    {
        while (true)
        {
            var spawnLocationIndex = Random.Range(0, spawnPoints.Count);
            Transform spawnLocations = spawnPoints[spawnLocationIndex];
            
         
            var clone = Instantiate(enemyPrefab, spawnLocations.position, quaternion.identity);
            yield return new WaitForSeconds(3);
            Destroy(clone);

        }

    }
}
