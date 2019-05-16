using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public PlayerControl PlayerHealth; //get player's health
    public GameObject enemy; //Prefab of the enemy to spawn
    public float spawnTime = 3f; //Delay on each spawn, init 3f
    private Transform[] spawnPoints; //Spawn location

    // Start is called before the first frame update
    void Start()
    {
        //get spawn locations from children
        spawnPoints = GetComponentsInChildren<Transform>();
        //call the Spawn function continuously
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    //Spawn
    void Spawn()
    {
        //stop spawning if player died
        if (PlayerHealth.health <= 0) return;

        //find a random location to spawn
        int spawnPointIndex = Random.Range(1, spawnPoints.Length);

        //Create an instance of the enemy prefab at the spawn location
        Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }

}
