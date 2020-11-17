using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    public int maxSeekers;
    public int maxMiners;

    public int seekerPrice;
    public int minerPrice;

    public Text goldText;

    public GameObject seekerPrefab;
    public GameObject minerPrefab;
    public Transform seekerSpawnPoint;
    public Transform minerSpawnPoint;

    public Pathfinding pathfinder;

    public int startingGold;
    private int gold;

    private int seekerCount;

    private int minerCount;

    private void Start()
    {
        gold = startingGold;
        seekerCount = 0;
        minerCount = 0;
        UpdateGoldText();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && gold >= seekerPrice)
        {
            gold -= seekerPrice;
            SpawnSeeker();
            UpdateGoldText();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && gold >= minerPrice)
        {
            gold -= minerPrice;
            SpawnMiner();
            UpdateGoldText();
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

    void UpdateGoldText()
    {
        goldText.text = "current gold: " + gold;
    }

    public void DepositGold(int goldAmmount)
    {
        gold += goldAmmount;
        UpdateGoldText();
    }
}
