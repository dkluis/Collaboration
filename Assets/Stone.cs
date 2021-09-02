using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public Route currentRoute;

    private int routePosition = 0;
    public int steps = 0;
    private bool isMoving = false;

    private void Update()
    {      
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            steps = Random.Range(1, 6);
            Debug.Log($"Number of Steps: {steps}, available steps = {currentRoute.childNodeList.Count}");
            if (routePosition + steps < currentRoute.childNodeList.Count)
            {
                Debug.Log($"Starting CoRoutine Move Standard {steps}");
                StartCoroutine(Move());
            }
            else
            {
                steps = steps - routePosition;
                Debug.Log($"Starting CoRoutine Move Limited {steps}");
                StartCoroutine(Move());
            }
        }
    }

    IEnumerator Move()
    {
        if (isMoving) { yield break; }
        isMoving = true;

        while(steps > 0)
        {
            Debug.Log($"Perform Step {steps} ---> Current Pos {routePosition} for: {currentRoute.childNodeList[routePosition].position.x}, {currentRoute.childNodeList[routePosition].position.y} <> Next Pos {routePosition + 1} is: {currentRoute.childNodeList[routePosition + 1].position.x}, {currentRoute.childNodeList[routePosition + 1].position.y}");
            Vector2 nextNode = currentRoute.childNodeList[routePosition + 1].position;
            while (!MoveToNextNode(nextNode)) { yield return null; };
            Debug.Log($"Finish A Move to Route Pos {routePosition} via Step {steps}");
            yield return new WaitForSeconds(1f);
            steps--;
            routePosition++;
        }
        Debug.Log($"Finish All Moves");
        isMoving = false; 
    }

    bool MoveToNextNode(Vector2 goalNode)
    {
        transform.position = Vector2.MoveTowards(transform.position, goalNode, 2f * Time.deltaTime);
        if (goalNode.x == transform.position.x && goalNode.y == transform.position.y)
        {
            Debug.Log($"Returning True from MoveToNextNode");
            return true;
        }
        else
        {
            
            return false;
        }
    }

}
