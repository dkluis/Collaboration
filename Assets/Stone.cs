using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    public Route currentRoute;

    private int routePosition = 1;
    public int steps = 0;
    private bool isMoving = false;
    private int totalAvailableSteps = 0;
    private int currentAvailableSteps = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        {
            totalAvailableSteps = currentRoute.childNodeList.Count - 1;
            currentAvailableSteps = totalAvailableSteps - routePosition;
            Debug.Log($"Total is {totalAvailableSteps} and Current {currentAvailableSteps}");
            steps = Random.Range(1, 7);
            if (routePosition + steps < totalAvailableSteps)
            {
                Debug.Log($"Starting CoRoutine Move Standard {steps}"); 
                StartCoroutine(Move());
            }
            else
            {
                if (routePosition + steps > currentRoute.childNodeList.Count) { steps = currentRoute.childNodeList.Count - 1 - routePosition; }
                Debug.Log($"Starting CoRoutine Move To End {steps}");
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
        Debug.Log($"Finished All Moves");
        isMoving = false; 
    }

    IEnumerator MoveToStart()
    {
        if (isMoving) { yield break; }
        isMoving = true;

        Vector2 nextNode = currentRoute.childNodeList[0].position;
        while (!MoveToNextNode(nextNode)) { yield return null; };
        Debug.Log($"Move To Start Pos ##################################################");
        yield return new WaitForSeconds(0.1f);
        steps = 0;
        routePosition = 0;

        Debug.Log($"Finish Moving to Start");
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
