using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MageAI : MonoBehaviour
{
    public float radius = 3;
    AIPath ai;
    Seeker seeker;
    public int[] roomRect;

    void Start()
    {
        ai = GetComponent<AIPath>();
    }
    Vector3 PickRandomPoint()
    {
        Vector3 point;
        while (true)
        {
            point = Random.insideUnitSphere * radius;
            point += ai.position;
            if (point.x >= roomRect[0] + 4 && point.x <= roomRect[0] + roomRect[2] - 4 && point.y >= roomRect[1] + 4 && point.y <= roomRect[1] + roomRect[3] - 4)
            {
                break;
            }
        }
        return point;
    }
    void Update()
    {
        // Update the destination of the AI if
        // the AI is not already calculating a path and
        // the ai has reached the end of the path or it has no path at all
        if (!ai.pathPending && (ai.TargetReached || !ai.hasPath))
        {
            ai.destination = PickRandomPoint();
            ai.SearchPath();
        }
    }
}
