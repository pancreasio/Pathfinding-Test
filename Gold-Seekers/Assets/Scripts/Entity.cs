using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Pathfinding pathfinder;
    public float moveSpeed;
    public float objectiveDistanceOffset;
    public Transform target;
    public Vector3 forwardVector;

    public delegate void TargetReachedTask();
    public TargetReachedTask OnTargetReached;
    private bool interruptMovement;
    private bool moving;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(GoToTargetAndInvokeTask(target));
        //OnTargetReached += ItWorks;
        forwardVector = Vector3.zero;
        interruptMovement = false;
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ItWorks()
    {
        Debug.Log("it works!");
    }

    public void GoToTarget(Vector3 movementTarget)
    {
        StartCoroutine(GoToTargetAndInvokeTask(movementTarget));
    }

    private IEnumerator GoToTargetAndInvokeTask(Vector3 targetPosition)
    {
        moving = true;
        Pathfinding.TerrainPath pathToTarget = pathfinder.FindPath(transform.position, targetPosition);
        int pathIndex = 0;
        while (!MoveToNextPoint(Time.deltaTime, pathToTarget, ref pathIndex))
        {
            if (interruptMovement)
            {
                interruptMovement = false;
                yield break;
            }
            yield return null;
        }

        if (OnTargetReached != null)
        {
            OnTargetReached.Invoke();
        }

        moving = false;
    }

    public void InterruptMovement()
    {
        if(moving)
            interruptMovement = true;
    }

    bool MoveToNextPoint(float TimeDelta, Pathfinding.TerrainPath targetPath, ref int currentPathIndex)
    {
        if (targetPath.waypoints.Count < currentPathIndex + 2)
        {
            currentPathIndex = 0;
            return true;
        }


        Vector3 targetPosition = targetPath.waypoints[currentPathIndex + 1].worldPosition;
        Vector3 result = (targetPosition - transform.position).normalized * TimeDelta * moveSpeed;
        forwardVector = result.normalized;

        if ((targetPosition - transform.position + result).magnitude < objectiveDistanceOffset)
        {
            transform.position = new Vector3(targetPosition.x,transform.position.y, targetPosition.z);
            currentPathIndex++;
            if (targetPath.waypoints.Count < currentPathIndex)
            {
                currentPathIndex = 0;
                return true;
            }

            return false;
        }
        else
        {
            transform.Translate(result);
            return false;
        }
    }
}
