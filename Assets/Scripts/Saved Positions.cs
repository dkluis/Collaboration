using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedPositions : MonoBehaviour
{
    Transform[] boardLocInSeq;
    GameObject[] boardObjects;
    public List<Transform> posInSeqList;
    int horGridElementMax = 4;
    public Dictionary<int, GameObject> boardRowGrid;
    public Dictionary<int, Dictionary<int, GameObject>> boardGrid;

    private void Start()
    {
        boardGrid = new Dictionary<int, Dictionary<int, GameObject>>();
        boardRowGrid = new Dictionary<int, GameObject>();
        Debug.Log($"Doing FillNodesByGrid");
        FillNodesByGrid();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        FillNodeInSeq();
        for (int i = 0; i < posInSeqList.Count; i++)
        {
            Vector2 currentNode = posInSeqList[i].position;
            if (i > 0)
            {
                Vector2 previousNode = posInSeqList[i - 1].position;
                Gizmos.DrawLine(previousNode, currentNode);
            }
        }
    }

    void FillNodeInSeq()
    {
        posInSeqList.Clear();
        boardLocInSeq = GetComponentsInChildren<Transform>();
        foreach (Transform location in boardLocInSeq)
        {
            if (location != this.transform)
            {
                posInSeqList.Add(location);
            }
        }
    }

    void FillNodesByGrid()
    {
        //boardRowGrid.Clear();
        //boardGrid.Clear();
        boardObjects = GetComponentsInChildren<GameObject>();
        Debug.Log($"FillNodesByGrid found {boardObjects.Length} board locations");
        int horIdx = 0;
        int verIdx = 0;
        int processed = -1;
        foreach (GameObject GOInGrid in boardObjects)
        {
            processed++;
            if (GOInGrid != this.transform)
            {
                Debug.Log($"Processing {processed}");
                if (horIdx <= horGridElementMax)
                {
                    Debug.Log($"Working on Row {verIdx} with Pos {horIdx}");
                    boardRowGrid.Add(horIdx, GOInGrid);
                    horIdx++;
                }
                else
                {
                    Debug.Log($"Adding a whole row with ID {verIdx}");
                    boardGrid.Add(verIdx, boardRowGrid);
                    boardRowGrid.Clear();
                    horIdx = 0;
                    verIdx++;
                }
            }
        }
        Debug.Log($"Have {boardGrid.Count} rows in Vertical Grid");
    }
}
