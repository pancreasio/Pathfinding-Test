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
    public GameObject targetMine;
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
        if (closeMines != null && closeMines.Count > 0)
        {
            foreach (GameObject mine in closeMines)
            {
                if (Vector3.Angle(entity.forwardVector, mine.transform.position - transform.position) < visionAngle)
                {
                    entity.InterruptMovement();
                    targetMine = mine;
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
                Debug.Log("fond target: " + foundTarget);
                entity.OnTargetReached += FindPatrolTarget;
                entity.GoToTarget(possibleTarget);
                break;
            }
        }
        if(iterations >= 1000)
            Debug.Log("no viable path");
    }

    public void BeginPatrol()
    {
        Debug.Log("begun patrol");
        sphereCollider.enabled = true;
        FindPatrolTarget();
    }

    public void StopPatrol()
    {
        closeMines.Clear();
        sphereCollider.enabled = false;
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
        if (collision.transform.tag == "Unclaimed Mine" && closeMines.Contains( collision.gameObject))
        {
            closeMines.Remove(collision.gameObject);
        }
    }
}
