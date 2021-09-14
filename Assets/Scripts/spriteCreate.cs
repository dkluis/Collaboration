// Create a Sprite at start-up.
// Assign a texture to the sprite when the button is pressed.

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class spriteCreate : MonoBehaviour
{
    public List<GameObject> BoardSquares;
    public GameObject squareTemplate;
    private GameObject board;
    private Transform newsquare;
    private GameObject BTG;
    private GameObject BTB;
    private GameObject BTActive;

    bool clicked = false;
    float birdX;
    float birdY;
    bool isBirdMoving = false;
    bool isMouseAllowed = false;

    int boardDimX = 20;  // 11
    int boardDimY = 20;   // 9

    int minX;
    int maxX;
    int minY;
    int maxY;

    private void Awake()
    {
        board = GameObject.Find("Board");
        Color color = Color.white;
        transform.position = new Vector2(-2f, -2f);
        squareTemplate = GameObject.Find("SquareTemplate");
        BTG = GameObject.Find("Bird Token Grey");
        BTB = GameObject.Find("Bird Token Blue");
        BTActive = BTG;

        SetBTActive(BTG);

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
        //int bx = Mathf.RoundToInt((maxX + 1) / 2);
        //int by= Mathf.RoundToInt((maxY + 1) / 2);
        //StartCoroutine(MoveTo(BTActive, new Vector2(bx, by)));
        //BTActive.GetComponent<birdTokenInfo>().locationSquare = $"Square {bx}-{by}";

        SetBTActive(BTB);
    }

    void Update()
    {

    }

    private bool isEven(int i)
    {
        if (i % 2 == 0) { return true; } else { return false; }
    }

    private void GetAllSquares()
    {
        BoardSquares.Clear();
        Transform[] allsquares = GetComponentsInChildren<Transform>();
        foreach (Transform square in allsquares)
        {
            if (square != this.transform) { BoardSquares.Add(square.gameObject); }
        }
    }

    private GameObject FindSquare(string current, string direction)
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

    private void OnGUI()
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
            GameObject sqtr = GameObject.Find("Square 8-7");
            birdTokenInfo bti = BTActive.GetComponent<birdTokenInfo>();
            bti.locationSquare = "Square 8-7";
            StartCoroutine(MoveTo(BTActive, sqtr.transform.position));
            SwitchBTActive();
        }

        if (Input.GetMouseButtonDown(0) && isMouseAllowed)
        {
            BTActive.GetComponent<Renderer>().material.color = Color.green;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            Debug.Log($"Mouse Button Down detected with Coordinate {mousePos.x} {mousePos.y}");
            int x = Mathf.RoundToInt(mousePos.x);
            int y = Mathf.RoundToInt(mousePos.y);
            if (x > maxX || x < minX || y > maxY || y < minY) { Debug.Log($"Clicking outside of the board!!!!"); return; }
            string clickedSquare = $"Square {x}-{y}";
            GameObject GO = GameObject.Find(clickedSquare);
            //GO.GetComponent<Renderer>().material.color = Color.clear;
            if (BTActive.GetComponent<birdTokenInfo>().locationSquare == GO.name)
            {
                Debug.Log($"Click the square occupied by the bird token");
                BTActive.GetComponent<Renderer>().material.color = Color.yellow;
            }
            StartCoroutine(MoveTo(BTActive, new Vector2(GO.transform.position.x, GO.transform.position.y)));
            BTActive.GetComponent<birdTokenInfo>().locationSquare = GO.name;
        }


        if (GUI.Button(new Rect(10, 90, 500, 30), "Allow the Mouse to Move the Bird"))
        {
            if (isMouseAllowed) { isMouseAllowed = false; } else { isMouseAllowed = true; }
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

    private void ExecuteMove(string direction)
    {
        //GameObject BT = GameObject.Find("Bird Token Grey");
        BTActive.GetComponent<Renderer>().material.color = Color.green;
        GameObject NextSquare = FindSquare(BTActive.GetComponent<birdTokenInfo>().locationSquare, direction);
        if (NextSquare is null) { Debug.Log($"Hit the Upper Boundary"); BTActive.GetComponent<Renderer>().material.color = Color.red; return; }
        StartCoroutine(MoveTo(BTActive, new Vector2(NextSquare.transform.position.x, NextSquare.transform.position.y)));
        BTActive.GetComponent<birdTokenInfo>().locationSquare = NextSquare.name;
    }

    private IEnumerator MoveTo(GameObject GO, Vector2 topos)
    {
        if (isBirdMoving) { yield break; }
        isBirdMoving = true;
        while (!MoveToPos(GO, topos)) { yield return null; };
        yield return new WaitForSeconds(0.05f);
        isBirdMoving = false;
    }

    private bool MoveToPos(GameObject GO, Vector2 goalNode)
    {
        GO.transform.position = Vector2.MoveTowards(GO.transform.position, goalNode, 5f * Time.deltaTime);
        if (goalNode.x == GO.transform.position.x && goalNode.y == GO.transform.position.y) { SwitchBTActive(); return true; } else { return false; }
    }

    void SetBTActive(GameObject act)
    {
        BTActive.GetComponent<birdTokenInfo>().isActive = "No";
        BTActive = act;
        act.GetComponent<birdTokenInfo>().isActive = "Yes";
        Debug.Log("Setting DB Active");
    }

    void SwitchBTActive()
    {
        if (BTG.GetComponent<birdTokenInfo>().isActive == "No") { SetBTActive(BTG); } else { SetBTActive(BTB); }
        Debug.Log("Switching DB Active");
    }

    void Misc()
    {
        if (Input.GetKey(KeyCode.Joystick1Button0) ||
          Input.GetKey(KeyCode.Joystick1Button1) ||
          Input.GetKey(KeyCode.Joystick1Button2) ||
          Input.GetKey(KeyCode.Joystick1Button3) ||
          Input.GetKey(KeyCode.Joystick1Button4) ||
          Input.GetKey(KeyCode.Joystick1Button5) ||
          Input.GetKey(KeyCode.Joystick1Button6) ||
          Input.GetKey(KeyCode.Joystick1Button7) ||
          Input.GetKey(KeyCode.Joystick1Button8) ||
          Input.GetKey(KeyCode.Joystick1Button9) ||
          Input.GetKey(KeyCode.Joystick1Button10) ||
          Input.GetKey(KeyCode.Joystick1Button11) ||
          Input.GetKey(KeyCode.Joystick1Button12) ||
          Input.GetKey(KeyCode.Joystick1Button13) ||
          Input.GetKey(KeyCode.Joystick1Button14) ||
          Input.GetKey(KeyCode.Joystick1Button15) ||
          Input.GetKey(KeyCode.Joystick1Button16) ||
          Input.GetKey(KeyCode.Joystick1Button17) ||
          Input.GetKey(KeyCode.Joystick1Button18) ||
          Input.GetKey(KeyCode.Joystick1Button19))
        {
            Debug.Log("JoyStick Button is Used");
        }

        // joystick axis
        if (Input.GetAxis("XC Left Stick X") != 0.0f ||
           Input.GetAxis("XC Left Stick Y") != 0.0f ||
           Input.GetAxis("XC Triggers") != 0.0f ||
           Input.GetAxis("XC Right Stick X") != 0.0f ||
           Input.GetAxis("XC Right Stick Y") != 0.0f)
        {
            Debug.Log("JoyStick Stick is Used");
        }
    }
}

