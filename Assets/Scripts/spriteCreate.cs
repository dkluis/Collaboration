// Create a Sprite at start-up.
// Assign a texture to the sprite when the button is pressed.

using System.Collections.Generic;
using UnityEngine;

public class spriteCreate : MonoBehaviour
{
    public Texture2D tex;
    private SpriteRenderer sr;
    public BoardSquare BrSq; //Accessing the BoardSquare Class

    //private bool createClicked = false;
    //private bool HideClicked = false;

    private int size = 32;

    public List<GameObject> BoardSquares;
    public GameObject squareTemplate;
    private GameObject board;
    private Transform newsquare;

    bool clicked = false;
    float birdRow;
    float birdColumn;

    void Awake()
    {
        board = GameObject.Find("Board");
        Color color = Color.white;
        transform.position = new Vector2(-2f, -2f);
        squareTemplate = GameObject.Find("SquareTemplate");
        int boardDimRows = 6;
        int boardDimColumns = 6;
        for (int r = 0; r < boardDimRows; r++)
        {
            for (int c = 0; c < boardDimColumns; c++)
            {
                color = Color.white;
                if (isEven(r) && isEven(c)) { color = Color.black; }
                if (!isEven(r) && !isEven(c)) { color = Color.black; }

                newsquare = Instantiate(squareTemplate.transform, new Vector2(c - 2, r - 2), Quaternion.identity);
                newsquare.name = $"Square {c + 1}-{r +1}";
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
        if (GUI.Button(new Rect(10, 10, 500, 30), "Color Square 0-2 Cyan"))
        {
            GameObject thissquare = BoardSquares[2];
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
            GameObject sqtr = GameObject.Find("Square 3-3");
            BT.transform.position = sqtr.transform.position;
            birdRow = BT.transform.position.x - 1;
            birdColumn = BT.transform.position.y - 1;
            //Need the while loop, etc to move animated
            //BT.transform.position = Vector2.MoveTowards(BT.transform.position, sqtr.transform.position, 1f * Time.deltaTime);
        }

        if (GUI.Button(new Rect(10, 90, 500, 30), "Move Bird Token to Up"))
        {
            GameObject BT = GameObject.Find("Bird Token Grey");
            birdColumn++;
            BT.transform.position = new Vector2(birdColumn, birdRow);
            //Need the while loop, etc to move animated
            //BT.transform.position = Vector2.MoveTowards(BT.transform.position, sqtr.transform.position, 1f * Time.deltaTime);
            Debug.Log($"Should have moved");
        }
    }
}
