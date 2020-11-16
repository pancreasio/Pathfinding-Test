using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    public float visionAngle;
    public float patrolRadius;

    private Entity entity;
    private SphereCollider sphereCollider;
    private List<GameObject> closeMines;
    public Mine targetMine;
    public GameManager.GameplayEvent OnMineFound;
    public GameManager.GameplayEvent OnMineClaimed;


    // Start is called before the first frame update
    void Start()
    {
        entity = transform.GetComponent<Entity>();
        targetMine = null;
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
        closeMines = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + entity.forwardVector, Color.red);
        if (closeMines.Count > 0)
        {
            foreach (GameObject mine in closeMines)
            {
                Debug.DrawLine(transform.position, mine.transform.position, Color.cyan);
                if (Vector3.Angle(entity.forwardVector, mine.transform.position - transform.position) < visionAngle)
                {
                    targetMine = mine.GetComponent<Mine>();
                    OnMineFound.Invoke();
                    break;
                }
            }
        }
    }

    void FindPatrolTarget()
    {
        entity.OnTargetReached = null;
        bool foundTarget = false;
        int iterations = 0;
        Node targetNode = null;
        while (!foundTarget && iterations <1000)
        {
            iterations++;
            Vector2 circleVector= Random.insideUnitCircle * patrolRadius;
            Vector3 possibleTarget = new Vector3(transform.position.x + circleVector.x, transform.position.y, transform.position.z + circleVector.y);
            targetNode = entity.pathfinder.grid.NodeFromWorldPoint(possibleTarget);
            if (targetNode != null && Physics.CheckSphere(possibleTarget, entity.pathfinder.grid.nodeRadius, entity.pathfinder.grid.unwalkableMask))
            {
                foundTarget = true;
                entity.OnTargetReached += FindPatrolTarget;
                entity.GoToTarget(possibleTarget);
                break;
            }
        }
    }

    public void BeginPatrol()
    {
        sphereCollider.enabled = true;
        FindPatrolTarget();
    }

    public void StopPatrol()
    {
        entity.InterruptMovement();
        closeMines.Clear();
        sphereCollider.enabled = false;
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
