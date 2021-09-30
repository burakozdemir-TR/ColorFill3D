using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public SpriteRenderer parent;
    public int x, y;
    public Vector2 xy;
    void Start()
    {
        parent = GetComponent<SpriteRenderer>();
        x = (int)transform.position.x;
        y = (int)transform.position.y;
        xy = new Vector2(x, y);
    }
    public Point(int x ,int y)
    {
        this.x = x;
        this.y = y;
    }
    public Vector2 CharacterPosition()
    {
        if (transform.position == GameObject.Find("Character").GetComponent<Transform>().position)
        {
            return xy;
        }
        return Vector2.zero;
    }

}
