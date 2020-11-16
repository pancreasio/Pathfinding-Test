using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Pathfinding pathfinder;
    public Transform target;

    protected Pathfinding.TerrainPath currentPath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentPath = pathfinder.FindPath(transform.position, target.position);
    }

    void OnDrawGizmos()
    {
        if (currentPath != null)
            pathfinder.grid.DrawDebugPath(currentPath);
    }
}
