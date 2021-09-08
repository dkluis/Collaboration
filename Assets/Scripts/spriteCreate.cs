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

    int boardDimX = 9;
    int boardDimY = 9;

    int minX;
    int maxX;
    int minY;
    int maxY;

    void Awake()
    {
        board = GameObject.Find("Board");
        Color color = Color.white;
        transform.position = new Vector2(-2f, -2f);
        squareTemplate = GameObject.Find("SquareTemplate");

        for (int x = 1; x <= boardDimX; x++)
        {
            for (int y = 1; y <= boardDimY; y++)
            {
                color = Color.white;
                if (isEven(x) && isEven(y)) { color = Color.black; }
                if (!isEven(x) && !isEven(y)) { color = Color.black; }

                newsquare = Instantiate(squareTemplate.transform, new Vector2(x, y), Quaternion.identity);
                newsquare.name = $"Square {x}-{y}";
                newsquare.parent = board.transform;
                newsquare.transform.position = new Vector2(x, y);
                GameObject col = GameObject.Find(newsquare.name);
                col.GetComponent<Renderer>().material.color = color;
            }
        }
        GetAllSquares();
        minX = int.Parse(BoardSquares[0].transform.position.x.ToString());
        minY = int.Parse(BoardSquares[0].transform.position.y.ToString());
        maxX = int.Parse(BoardSquares[BoardSquares.Count - 1].transform.position.x.ToString());
        maxY = int.Parse(BoardSquares[BoardSquares.Count - 1].transform.position.y.ToString());
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

    string GetUpSquare(string square)
    {
        string upsquare = "";
        GameObject sq = GameObject.Find(square);
        

        return upsquare;
    }

    GameObject FindSquare(string current, string direction)
    {
        
        string next = "";
        string tmp = current.Replace("Square ", "");
        string[] x_y = tmp.Split('-');
        int x = int.Parse(x_y[0]);
        int y = int.Parse(x_y[1]);
        switch (direction)
        {
            case "Up":
                y++;
                break;
            case "Down":
                y--;
                break;
            case "Left":
                x--;
                break;
            case "Right":
                x++;
                break;
            case "UpRight":
                x++;
                y++;
                break;
            case "DownRight":
                x++;
                y--;
                break;
            case "UpLeft":
                x--;
                y++;
                break;
            case "DownLeft":
                x--;
                y--;
                break;
            default:
                x = -9999;
                y = -9999;
                break;
        }
        next = $"/Board/Square {x}-{y}";
        GameObject newSquare = GameObject.Find(next);
        if (newSquare is null) { Debug.Log($"Did not find square {next}"); }
        Debug.Log($"Going to: {next}");
        return newSquare;
    }


    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 500, 30), "Color Square 5-5 Cyan"))
        {
            GameObject thissquare = GameObject.Find("Square 5-5");
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
            GameObject sqtr = GameObject.Find("Square 5-5");
            birdTokenInfo bti = BT.GetComponent<birdTokenInfo>();
            bti.locationSquare = "Square 5-5";
            StartCoroutine(MoveTo(BT, sqtr.transform.position));
        }

        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Alpha8) || Input.GetKey(KeyCode.Keypad8)) && !isBirdMoving)
        {
            ExecuteMove("Up");
        }

        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2)) && !isBirdMoving)
        {
            ExecuteMove("Down");
        }

        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Keypad4)) && !isBirdMoving)
        {
            ExecuteMove("Left");
        }

        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.Alpha6) || Input.GetKey(KeyCode.Keypad6)) && !isBirdMoving)
        {
            ExecuteMove("Right");
        }

        if ((Input.GetKey(KeyCode.Keypad9) || Input.GetKey(KeyCode.Alpha9)) && !isBirdMoving)
        {
            ExecuteMove("UpRight");
        }

        if ((Input.GetKey(KeyCode.Keypad7) || Input.GetKey(KeyCode.Alpha7)) && !isBirdMoving)
        {
            ExecuteMove("UpLeft");
        }

        if ((Input.GetKey(KeyCode.Keypad1) || Input.GetKey(KeyCode.Alpha1)) && !isBirdMoving)
        {
            ExecuteMove("DownLeft");
        }

        if ((Input.GetKey(KeyCode.Keypad3) || Input.GetKey(KeyCode.Alpha3)) && !isBirdMoving)
        {
            ExecuteMove("DownRight");
        }

    }

    void ExecuteMove(string direction)
    {
        GameObject BT = GameObject.Find("Bird Token Grey");
        BT.GetComponent<Renderer>().material.color = Color.green;
        GameObject NextSquare = FindSquare(BT.GetComponent<birdTokenInfo>().locationSquare, direction);
        if (NextSquare is null) { Debug.Log($"Hit the Upper Boundary"); BT.GetComponent<Renderer>().material.color = Color.red; ; return; }
        StartCoroutine(MoveTo(BT, new Vector2(NextSquare.transform.position.x, NextSquare.transform.position.y)));
        BT.GetComponent<birdTokenInfo>().locationSquare = NextSquare.name;
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
        GO.transform.position = Vector2.MoveTowards(GO.transform.position, goalNode, 3f * Time.deltaTime);
        //Debug.Log($"Goal {goalNode.x}, {goalNode.y} and pos is {GO.transform.position.x}, {GO.transform.position.y}");
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
