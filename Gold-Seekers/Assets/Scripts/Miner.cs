using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Miner : Worker
{
    public int goldCarryCapacity;
    public int goldDrainedPerSecond;

    private int currentGold;

    public PlayerBase parentBase;

    public GameManager.GameplayEvent OnFinishedMining;
    public GameManager.GameplayEvent OnFinishedDepositing;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currentGold = 0;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void GoToMine()
    {
        closeMines.Remove(targetMine.gameObject);
        entity.OnTargetReached = null;
        entity.OnTargetReached = BeginMining;
        entity.GoToTarget(targetMine.transform.position);
    }

    public void BeginMining()
    {
        entity.OnTargetReached = null;
        entity.OnTargetReached += StartMiningRoutine;
        entity.GoToTarget(targetMine.transform.position);
    }

    private void StartMiningRoutine()
    {
        StartCoroutine(MineLoop(targetMine));
    }

    private IEnumerator MineLoop(Mine mine)
    {
        float timer = 0f;
        while (currentGold < goldCarryCapacity && !mine.IsDrained())
        {
            timer += Time.deltaTime;
            if (timer > 1f)
            {
                currentGold += mine.MineGold(goldDrainedPerSecond);
                timer = 0f;
            }

            yield return null;
        }
        StopMining();
    }

    public void StopMining()
    {
        entity.OnTargetReached = null;
        targetMine.StopMiningGold();
        targetMine = null;
        if (OnFinishedMining != null)
            OnFinishedMining.Invoke();
    }

    public void StartReturning()
    {
        entity.OnTargetReached = null;
        entity.OnTargetReached += StopReturning;
        entity.GoToTarget(parentBase.transform.position);
    }

    public void StopReturning()
    {
        entity.OnTargetReached = null;
        parentBase.DepositGold(currentGold);
        if (OnFinishedDepositing!= null)
            OnFinishedDepositing.Invoke();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Claimed Mine")
        {
            closeMines.Add(collision.gameObject);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.transform.tag == "Claimed Mine" && closeMines.Contains(collision.gameObject))
        {
            closeMines.Remove(collision.gameObject);
        }
    }
}
