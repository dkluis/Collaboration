// Create a Sprite at start-up.
// Assign a texture to the sprite when the button is pressed.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteCreate : MonoBehaviour
{
    public List<GameObject> BoardSquares;
    public GameObject squareTemplate;
    private GameObject board;
    private Transform newsquare;

    bool clicked = false;
    float birdX;
    float birdY;
    bool isBirdMoving = false;

    void Awake()
    {
        board = GameObject.Find("Board");
        Color color = Color.white;
        transform.position = new Vector2(-2f, -2f);
        squareTemplate = GameObject.Find("SquareTemplate");
        int boardDimX = 6;
        int boardDimY = 3;
        for (int x = 0; x < boardDimX; x++)
        {
            for (int y = 0; y < boardDimY; y++)
            {
                color = Color.white;
                if (isEven(x) && isEven(y)) { color = Color.black; }
                if (!isEven(x) && !isEven(y)) { color = Color.black; }

                newsquare = Instantiate(squareTemplate.transform, new Vector2(x - 2, y - 2), Quaternion.identity);
                newsquare.name = $"Square {x + 1}-{y + 1}";
                newsquare.parent = board.transform;
                GameObject col = GameObject.Find(newsquare.name);
                col.GetComponent<Renderer>().material.color = color;
            }
        }
        GetAllSquares();
    }

    bool isEven(int i)
    {
        if (i % 2 == 0) { return true; } else { return false; }
    }

    void GetAllSquares()
    {
        BoardSquares.Clear();
        Transform[] allsquares = GetComponentsInChildren<Transform>();
        foreach (Transform square in allsquares)
        {
            if (square != this.transform) { BoardSquares.Add(square.gameObject); }
        }
    }

    void Start()
    {
        //mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 500, 30), "Color Square 2-2 Cyan"))
        {
            GameObject thissquare = GameObject.Find("Square 2-2");
            if (!clicked)
            {
                thissquare.GetComponent<Renderer>().material.color = Color.cyan;
                clicked = true;
                Debug.Log($"Name is {thissquare.name} and clicked = {clicked}");
            }
            else
            {
                thissquare.GetComponent<Renderer>().material.color = Color.black;
                clicked = false;
                Debug.Log($"Name is {thissquare.name} and clicked = {clicked}");
            }
        }

        if (GUI.Button(new Rect(10, 50, 500, 30), "Fetch Bird Token to Board Middle"))
        {
            GameObject BT = GameObject.Find("Bird Token Grey");
            GameObject sqtr = GameObject.Find("Square 4-2");
            BT.transform.position = sqtr.transform.position;
            //Need the while loop, etc to move animated
            //BT.transform.position = Vector2.MoveTowards(BT.transform.position, sqtr.transform.position, 1f * Time.deltaTime);
        }

        if (GUI.Button(new Rect(10, 90, 500, 30), "Move Bird Token to Up"))
        {
            GameObject BT = GameObject.Find("Bird Token Grey");
            StartCoroutine(MoveTo(BT, new Vector2(BT.transform.position.x, BT.transform.position.y + 1)));
        }

        if (Input.GetKey(KeyCode.UpArrow) && !isBirdMoving)
        {
            GameObject BT = GameObject.Find("Bird Token Grey");
            StartCoroutine(MoveTo(BT, new Vector2(BT.transform.position.x, BT.transform.position.y + 1)));
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            GameObject BT = GameObject.Find("Bird Token Grey");
            StartCoroutine(MoveTo(BT, new Vector2(BT.transform.position.x, BT.transform.position.y - 1)));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            GameObject BT = GameObject.Find("Bird Token Grey");
            birdX = BT.transform.position.x + 1;
            StartCoroutine(MoveTo(BT, new Vector2(BT.transform.position.x - 1, BT.transform.position.y)));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            GameObject BT = GameObject.Find("Bird Token Grey");
            birdX = BT.transform.position.x - 1;
            StartCoroutine(MoveTo(BT, new Vector2(BT.transform.position.x + 1, BT.transform.position.y)));
        }
    }

    IEnumerator MoveTo(GameObject GO, Vector2 topos)
    {
        if (isBirdMoving) { yield break; }
        isBirdMoving = true;
        while (!MoveToPos(GO, topos)) { yield return null; };
        yield return new WaitForSeconds(0.1f);
        isBirdMoving = false;
    }

    bool MoveToPos(GameObject GO, Vector2 goalNode)
    {
        GO.transform.position = Vector2.MoveTowards(GO.transform.position, goalNode, 2f * Time.deltaTime);
        Debug.Log($"Goal {goalNode.x}, {goalNode.y} and pos is {GO.transform.position.x}, {GO.transform.position.y}");
        if (goalNode.x == GO.transform.position.x && goalNode.y == GO.transform.position.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
