 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route : MonoBehaviour
{
    Transform[] childBojects;
    public List<Transform> childNodeList;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        FillNodes();
        for (int i = 0; i < childNodeList.Count; i++)
        {
            Vector2 currentNode = childNodeList[i].position;
            if (i>0)
            {
                Vector2 previousNode = childNodeList[i - 1].position;
                Gizmos.DrawLine(previousNode, currentNode);
            }
        }
    }

    void FillNodes()
    {
        childNodeList.Clear();
        childBojects = GetComponentsInChildren<Transform>();
        foreach (Transform child in childBojects)
        {
            if (child != this.transform)
            {
                childNodeList.Add(child);
            }
        }
    }
}
