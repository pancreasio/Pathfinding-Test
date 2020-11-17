using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public int maxSeekers;
    public int maxMiners;

    public int seekerPrice;
    public int minerPrice;

    public GameObject seekerPrefab;
    public GameObject minerPrefab;
    public Transform seekerSpawnPoint;
    public Transform minerSpawnPoint;

    public Pathfinding pathfinder;

    private int gold;

    private int seekerCount;

    private int minerCount;

    private void Start()
    {
        gold = 0;
        seekerCount = 0;
        minerCount = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpawnSeeker();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpawnMiner();
        }
    }

    private void SpawnMiner()
    {
        if (minerCount < maxMiners)
        {
            GameObject result = Instantiate(minerPrefab, minerSpawnPoint.position, Quaternion.identity);
            result.GetComponent<Entity>().pathfinder = pathfinder;
            result.GetComponent<Miner>().parentBase = this;
            minerCount++;
        }
    }

    private void SpawnSeeker()
    {
        if (seekerCount < maxSeekers)
        {
            Instantiate(seekerPrefab, seekerSpawnPoint.position, Quaternion.identity).GetComponent<Entity>().pathfinder = pathfinder;
            seekerCount++;
        }
    }

    public void DepositGold(int goldAmmount)
    {

    }
}
