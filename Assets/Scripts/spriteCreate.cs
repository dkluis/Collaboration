// Create a Sprite at start-up.
// Assign a texture to the sprite when the button is pressed.

using UnityEngine;

public class spriteCreate : MonoBehaviour
{
    public Texture2D tex;
    private Sprite mySprite;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        //sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);

        transform.position = new Vector2(1.5f, 1.5f);
    }

    void Start()
    {
        float scale = .5f;
        mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Add sprite"))
        {
            sr.sprite = mySprite;
        }
        if (GUI.Button(new Rect(10, 50, 100, 30), "Delete sprite"))
        {
            Destroy(sr);
        }
    }
}
