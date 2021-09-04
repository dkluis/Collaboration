using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquare
{
    private Sprite sprite;
    private GameObject GaOb;
    private SpriteRenderer spre ;
    private Color col;

    protected string Name;
    protected Vector2 Pos;
    protected int Row;
    protected int Column;

    public BoardSquare()
    {
        Debug.Log($"Creating a BoardSquare");
    }

    public void Init(Vector2 positions, Color color, int row, int column, int size)
    {
        GaOb = new GameObject($"Square {column + 1}-{row + 1}");
        col = color;
        GameObject board = GameObject.Find("Board");
        spre = GaOb.AddComponent<SpriteRenderer>();
        
        spre.color = col;
        Name = spre.name;
        Pos = new Vector2(row, column);
        sprite = Sprite.Create(new Texture2D(size, size), new Rect(0, 0, size, size), Pos, 100.0f);
        Row = row;
        Column = column;
        GaOb.transform.position = new Vector2(row, column);

    }

    public void Create()
    {
        Debug.Log($"Create {Name}");
        spre.sprite = sprite;
        //GaOb.transform.position = Vector2.MoveTowards(transform.position, Pos, 0);
    }

    public void Hide()
    {
        GaOb.SetActive(false);
    }

    public void Show()
    {
        GaOb.SetActive(true);
    }

    public int RowInfo()
    {
        return Row;
    }

    public int ColumnInfo()
    {
        return Column;
    }

    public Vector2 Position()
    {
        return Pos;
    }

    public string NameInfo()
    {
        return Name;
    }

    public void SetColor(Color color)
    {
        //TODO set Color
    }
}