using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject flag;
    public static GameManager.GameplayEvent OnMineClaimed;
    public static GameManager.GameplayEvent OnMineDrained;
    public bool isBeingDrained;

    public int maxGold;
    private int remainingGold;
    // Start is called before the first frame update
    void Start()
    {
        flag.SetActive(false);
        isBeingDrained = false;
        remainingGold = maxGold;
    }

    public void Claim()
    {
        if(OnMineClaimed!= null)
            OnMineClaimed.Invoke();
        flag.SetActive(true);
        tag = "Claimed Mine";
    }

    public int MineGold(int requestedGold)
    {
        isBeingDrained = true;
        if (requestedGold > remainingGold)
        {
            remainingGold = 0;
            return requestedGold - remainingGold;
        }
        else
        {
            remainingGold -= requestedGold;
            return requestedGold;
        }

    }

    public void StopMiningGold()
    {
        isBeingDrained = false;
    }

    public bool IsDrained()
    {
        return (remainingGold > 0);
    }
}
