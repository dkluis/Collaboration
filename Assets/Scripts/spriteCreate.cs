// Create a Sprite at start-up.
// Assign a texture to the sprite when the button is pressed.

using System.Collections.Generic;
using UnityEngine;

public class spriteCreate : MonoBehaviour
{
    public Texture2D tex;
    private SpriteRenderer sr;
    public BoardSquare BrSq; //Accessing the BoardSquare Class

    private bool createClicked = false;
    //private bool HideClicked = false;

    private int size = 32;

    public List<GameObject> BoardSquares;

    void Awake()
    {
        Color color = Color.white;
        transform.position = new Vector2(-2f, -2f);
        int boardDimRows = 5;
        int boardDimColumns = 10;
        for (int r = 0; r < boardDimRows; r++)
        {
            for (int c = 0; c < boardDimColumns; c++)
            {
                color = Color.white;
                if (isEven(r) && isEven(c)) { color = Color.black; }
                if (!isEven(r) && !isEven(c)) { color = Color.black; }
                BrSq = new BoardSquare();
                BrSq.Init(new Vector2(c, r), color, c, r, size);
                BrSq.Create();
                createClicked = true;
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
        if (GUI.Button(new Rect(10, 10, 100, 30), "Add Square"))
        {
            if (createClicked) { Debug.Log("Change Color for Row 1, Column 3"); return; }
            GameObject thissquare = BoardSquares[2];
            Debug.Log($"Name is {thissquare.name}");
            thissquare.GetComponent<MeshRenderer>().material.color = Color.cyan;
        }

        /*
        if (GUI.Button(new Rect(10, 50, 100, 30), "Hide Square"))
        {
            if (!createClicked) { Debug.Log("Not Created yet"); return; }
            if (HideClicked) { Debug.Log("Already Clicked Hide"); return; }
            Debug.Log($"Hide {BrSq.NameInfo()}");
            BrSq.Hide();
            HideClicked = true;
        }
        if (GUI.Button(new Rect(10, 90, 100, 30), "Show Square"))
        {
            if (!createClicked) { Debug.Log("Not Created yet"); return; }
            if (HideClicked)
            {
                BrSq.Show();
                HideClicked = false;
                return;
            }
            else
            {
                Debug.Log($"Show Not Hidden");
            }
        }
        */
    }
}
