using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectSpawner : MonoBehaviour {
    public float minOffset = -3;
    public float maxOffset = 2;
    public List<GameObject> objectsAvalible;
    private System.Random random;
	// Use this for initialization
    void Start()
    {
        random = new System.Random();
    }
	public void spawnObject(int[] indexsToSpawnAtSameTime,int numberToSpawn = 20, float secondsTillEachSpawn = .5f)
    {
        StartCoroutine(SpawnOverTime(indexsToSpawnAtSameTime, numberToSpawn,  secondsTillEachSpawn));
    }

    private IEnumerator SpawnOverTime(int[] indexsToSpawnAtSameTime, int numberToSpawn, float wait)
    {
        for(int i = 0; i < numberToSpawn; i++)
        {
            for(int j = 0; j < indexsToSpawnAtSameTime.Length; j++)
            {
                if(j >= 0 && j < objectsAvalible.Count)
                {
                    if (objectsAvalible[j] != null)
                    {
                        Vector3 location = transform.position;
                        location.x += (float)GetRandomDouble(minOffset, maxOffset + 1);
                        location.z += (float)GetRandomDouble(minOffset, maxOffset + 1);
                        location.y += (float)GetRandomDouble(-2, 0 + 1);
                        Instantiate(objectsAvalible[j], location, objectsAvalible[j].gameObject.transform.rotation);
                        Debug.Log("location: (" + location.x + "," + location.y + "," + location.z + ")");
                    }
                }
                else
                {
                    Debug.Log("Tried to spawn something not in array");
                }
                yield return new WaitForSeconds (wait); 
            }
        }
    }

    private double GetRandomDouble(double minimum, double maximum)
    {
        return random.NextDouble() * (maximum - minimum) + minimum;
    }
}