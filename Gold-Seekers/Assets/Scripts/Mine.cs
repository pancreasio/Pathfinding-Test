using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public GameObject flag;

    public bool claimed;
    // Start is called before the first frame update
    void Start()
    {
        flag.SetActive(false);
        claimed = false;
    }

    public void Claim()
    {
        flag.SetActive(true);
        claimed = true;
        tag = "Claimed Mine";
    }
}
