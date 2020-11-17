using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : Worker
{
    public GameManager.GameplayEvent OnMineClaimed;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public void BeginMark()
    {
        entity.OnTargetReached = null;
        entity.OnTargetReached = EndMark;
        entity.GoToTarget(targetMine.transform.position);
    }

    public void EndMark()
    {
        targetMine.Claim();
        closeMines.Remove(targetMine.gameObject);
        targetMine = null;
        if(OnMineClaimed!= null)
            OnMineClaimed.Invoke();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Unclaimed Mine")
        {
            closeMines.Add(collision.gameObject);
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.transform.tag == "Unclaimed Mine" && closeMines.Contains(collision.gameObject))
        {
            closeMines.Remove(collision.gameObject);
        }
    }
}
