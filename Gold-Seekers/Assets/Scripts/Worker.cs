using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{
    public float visionAngle;
    public float patrolRadius;

    protected Entity entity;
    protected SphereCollider sphereCollider;
    protected List<GameObject> closeMines;
    public Mine targetMine;

    public GameManager.GameplayEvent OnMineFound;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        entity = transform.GetComponent<Entity>();
        targetMine = null;
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
        closeMines = new List<GameObject>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (closeMines.Count > 0)
        {
            foreach (GameObject mine in closeMines)
            {
                if (Vector3.Angle(entity.forwardVector, mine.transform.position - transform.position) < visionAngle && !mine.GetComponent<Mine>().isBeingDrained)
                {
                    targetMine = mine.GetComponent<Mine>();
                    OnMineFound.Invoke();
                    break;
                }
            }
        }
    }

    protected void FindPatrolTarget()
    {
        entity.OnTargetReached = null;
        bool foundTarget = false;
        int iterations = 0;
        Node targetNode = null;
        while (!foundTarget && iterations < 1000)
        {
            iterations++;
            Vector2 circleVector = Random.insideUnitCircle * patrolRadius;
            Vector3 possibleTarget = new Vector3(transform.position.x + circleVector.x, transform.position.y, transform.position.z + circleVector.y);
            targetNode = entity.pathfinder.grid.NodeFromWorldPoint(possibleTarget);
            if (targetNode != null/* && Physics.CheckSphere(possibleTarget, entity.pathfinder.grid.nodeRadius, entity.pathfinder.grid.unwalkableMask)*/)
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
}
