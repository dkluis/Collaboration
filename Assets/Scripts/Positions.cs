using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positions : MonoBehaviour
{
    Transform[] boardLocation;
    public List<Transform> positionsList;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        FillNodes();
        for (int i = 0; i < positionsList.Count; i++)
        {
            Vector2 currentNode = positionsList[i].position;
            if (i > 0)
            {
                Vector2 previousNode = positionsList[i - 1].position;
                Gizmos.DrawLine(previousNode, currentNode);
            }
        }
    }

    void FillNodes()
    {
        positionsList.Clear();
        boardLocation = GetComponentsInChildren<Transform>();
        foreach (Transform location in boardLocation)
        {
            if (location != this.transform)
            {
                positionsList.Add(location);
            }
        }
    }
}
