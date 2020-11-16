using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject flag;
    public static GameManager.GameplayEvent OnMineClaimed;
    public static GameManager.GameplayEvent OnMineDrained;
    public bool claimed;
    // Start is called before the first frame update
    void Start()
    {
        flag.SetActive(false);
        claimed = false;
    }

    public void Claim()
    {
        if(OnMineClaimed!= null)
            OnMineClaimed.Invoke();
        flag.SetActive(true);
        claimed = true;
        tag = "Claimed Mine";
    }
}
