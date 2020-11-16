using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Pathfinding pathfinder;
    public float moveSpeed;
    public float objectiveDistanceOffset;
    public Transform target;


    public delegate void TargetReachedTask();
    public TargetReachedTask OnTargetReached;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GoToTargetAndInvokeTask(target));
        OnTargetReached += ItWorks;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ItWorks()
    {
        Debug.Log("it works!");
    }

    private IEnumerator GoToTargetAndInvokeTask(Transform targetPosition)
    {
        Pathfinding.TerrainPath pathToTarget = pathfinder.FindPath(transform.position, targetPosition.position);
        int pathIndex = 0;
        while (!MoveToNextPoint(Time.deltaTime, pathToTarget, ref pathIndex))
        {
            yield return null;
        }

        if (OnTargetReached != null)
        {
            OnTargetReached.Invoke();
        }
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
