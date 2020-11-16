using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Pathfinding pathfinder;
    public Transform target;
    public float moveSpeed;
    public float objectiveDistanceOffset;

    private bool moving;
    private int currentPathIndex;
    protected Pathfinding.TerrainPath currentPath;
    // Start is called before the first frame update
    void Start()
    {
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving && objectiveDistanceOffset < Vector3.Distance(transform.position, target.position))
        {
            currentPath = pathfinder.FindPath(transform.position, target.position);
            currentPathIndex = 0;
            moving = true;
        }

        if (moving)
        {
            MoveToNextPoint(Time.deltaTime);
        }

    }

    void MoveToNextPoint(float TimeDelta)
    {
        if (currentPath.waypoints.Count < currentPathIndex + 2)
        {
            moving = false;
            currentPathIndex = 0;
            return;
        }


        Vector3 targetPosition = currentPath.waypoints[currentPathIndex + 1].worldPosition;
        Vector3 result = (targetPosition - transform.position).normalized * TimeDelta * moveSpeed;
        if ((targetPosition - transform.position + result).magnitude < objectiveDistanceOffset)
        {
            transform.position = targetPosition;
            currentPathIndex++;
            if (currentPath.waypoints.Count < currentPathIndex)
            {
                moving = false;
                currentPathIndex = 0;
            }
        }
        else
        {
            transform.Translate(result);
        }
    }

    void OnDrawGizmos()
    {
        if (currentPath != null)
            pathfinder.grid.DrawDebugPath(currentPath);
    }
}
