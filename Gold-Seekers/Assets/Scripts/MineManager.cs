using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineManager : MonoBehaviour
{
    public int maxUnclaimedMines;
    public int maxMines;
    public float timeToSpawnMine;

    public TerrainGrid grid;
    public GameObject minePrefab;

    private float spawwnTimer;

    private int mineCount;

    private int unclaimedMineCount;
    // Start is called before the first frame update
    void Start()
    {
        Mine.OnMineClaimed += MineClaimed;
        Mine.OnMineDrained += MineDrained;
        spawwnTimer = 0f;
        mineCount = 0;
        unclaimedMineCount = 0;
    }

    void OnDestroy()
    {
        Mine.OnMineClaimed -= MineClaimed;
        Mine.OnMineDrained -= MineDrained;
    }

    // Update is called once per frame
    void Update()
    {
        spawwnTimer += Time.deltaTime;
        if (spawwnTimer >= timeToSpawnMine)
        {
            spawwnTimer = 0f;
            if (unclaimedMineCount < maxUnclaimedMines && mineCount < maxMines)
            {
                SpawnMine();
            }
        }
    }

    void SpawnMine()
    {
        bool foundSpot = false;
        int iterations = 0;
        Node targetNode = null;
        while (!foundSpot && iterations < 1000)
        {
            iterations++;
            Vector3 possibleSpot = new Vector3(grid.transform.position.x + Random.Range(-grid.gridWorldSize.x/2, grid.gridWorldSize.x/2), transform.position.y,
                grid.transform.position.z + Random.Range(-grid.gridWorldSize.y / 2, grid.gridWorldSize.y / 2));
            targetNode = grid.NodeFromWorldPoint(possibleSpot);
            if (targetNode != null && Physics.CheckSphere(possibleSpot, grid.nodeRadius, grid.unwalkableMask))
            {
                foundSpot = true;
                Instantiate(minePrefab, possibleSpot, Quaternion.identity);
                mineCount++;
                unclaimedMineCount++;
                break;
            }
        }
        
    }

    void MineDrained()
    {
        mineCount--;
    }

    void MineClaimed()
    {
        unclaimedMineCount--;
    }
}
