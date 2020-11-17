using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Pathfinding pathfinder;
    public float moveSpeed;
    public float objectiveDistanceOffset;
    public Vector3 forwardVector;

    public delegate void TargetReachedTask();
    public TargetReachedTask OnTargetReached;
    private bool interruptMovement;
    private bool moving;

    // Start is called before the first frame update
    void Start()
    {
        forwardVector = Vector3.zero;
        interruptMovement = false;
        moving = false;
    }

    public void GoToTarget(Vector3 movementTarget)
    {
        moving = true;
        StartCoroutine(GoToTargetAndInvokeTask(movementTarget));
    }

    private IEnumerator GoToTargetAndInvokeTask(Vector3 targetPosition)
    {
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
    }

    public void InterruptMovement()
    {
        if(moving)
            interruptMovement = true;
    }

    bool MoveToNextPoint(float TimeDelta, Pathfinding.TerrainPath targetPath, ref int currentPathIndex)
    {
        if (targetPath == null  || targetPath.waypoints.Count < currentPathIndex + 2)
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
