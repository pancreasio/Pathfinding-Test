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

    private bool drained;
    // Start is called before the first frame update
    void Start()
    {
        flag.SetActive(false);
        isBeingDrained = false;
        remainingGold = maxGold;
        drained = false;
    }

    void Update()
    {
        if (remainingGold <= 0)
        {
            DestroyMine();
        }

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
            drained = true;
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
        if(remainingGold<0)
            DestroyMine();
    }

    void DestroyMine()
    {
        if (OnMineDrained != null)
            OnMineDrained.Invoke();

        Destroy(this.gameObject);
    }

    public bool IsDrained()
    {
        return (remainingGold <= 0);
    }
}
