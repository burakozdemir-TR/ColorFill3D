using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Movement : MonoBehaviour
{
    private bool isMoving;
    private Vector3 origPos, targetPos;
    private float timeToMove = .2f;
    private Grid ground;
    List<Vector2> pos;
    public Point point;
    private void Start()
    {
        Application.targetFrameRate = 60;
        ground = GetComponentInParent<Grid>();
        pos = new List<Vector2>();
    }
    private void DetectPosition()
    {
        var ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            point = hit.transform.gameObject.GetComponent<Point>();
            MyTestFunction(point, Color.white, Color.red);
        }
    }
    void Update()
    {
        MoveSequance();
        //if (isMoving)
        //    Debug.Log("immovin");
        //else
        //    Debug.Log("NNNNNNNNNNNNNNNNNOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");   
    }
    private void MoveSequance()
    {
        if (Input.GetKey(KeyCode.W) && !isMoving)
            StartCoroutine(MoveCharacter(Vector3.up));
        if (Input.GetKey(KeyCode.A) && !isMoving)
            StartCoroutine(MoveCharacter(Vector3.left));
        if (Input.GetKey(KeyCode.S) && !isMoving)
            StartCoroutine(MoveCharacter(Vector3.down));
        if (Input.GetKey(KeyCode.D) && !isMoving)
            StartCoroutine(MoveCharacter(Vector3.right));
    }
    private IEnumerator MoveCharacter(Vector3 direction)
    {
        DetectPosition();
        isMoving = true;
        float elapsedTime = 0;
        origPos = transform.position;
        targetPos = origPos + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
        
    }
    void FloodFill(int x, int y, Color newColor)
    {
        pos.Clear();
        if (x + 1 > ground.rows || y + 1 > ground.cols || x < 0 || y < 0)
            return;
        if (newColor == ground.grid[y, x].GetComponent<SpriteRenderer>().color)
            return;

        ground.grid[y, x].GetComponent<SpriteRenderer>().color = newColor;

        FloodFill(y - 1, x , newColor);
        FloodFill(y , x + 1 ,  newColor);
        FloodFill(y + 1 , x , newColor);
        FloodFill(y , x - 1 ,  newColor);

        /*For 8-connected pixels,additional calls
        boundary_fill(x+1,y-1,fill_color,boundary_color);
        boundary_fill(x+1,y+1,fill_color,boundary_color);
        boundary_fill(x-1,y+1,fill_color,boundary_color);
        boundary_fill(x-1,y-1,fill_color,boundary_color);
        */
    }
    private void MyTestFunction(Point pt, Color oldColor, Color newColor)
    {
        if (oldColor == ground.grid[pt.y, pt.x].GetComponent<SpriteRenderer>().color)
        {
            //Stack<Vector2> pos = new Stack<Vector2>();
            //pos.Push(pt.xy);
            pos.Add(pt.xy);
            //foreach (var i in pos)
            //{
            //    Debug.Log(i.ToString());
            //}
            //foreach (var i in pos)
            //{
            //    Debug.Log(pos.Contains(new Vector2(pos.Average(x => x.x), pos.Average(y => y.y))));
            //}
            ground.grid[pt.y, pt.x].GetComponent<SpriteRenderer>().color = newColor;
            CheckNeighbors(pos, newColor);
            //Debug.Log(CheckNeighbors(pos, newColor));
            //if(CheckNeighbors(pos, newColor))
            //{
            //}
        }
        //if (newColor == ground.grid[pt.y, pt.x].GetComponent<SpriteRenderer>().color)
        //    return;
        //else
        //    ground.grid[pt.y, pt.x].GetComponent<SpriteRenderer>().color = newColor;
        //return;
        //Stack<Point> pixels = new Stack<Point>();
        //pixels.Push(pt);
        //while (pixels.Count != 0)
        //{
        //    Point temp = pixels.Pop();
        //    int y1 = temp.y;
        //    while (y1 >= 0 && ground.grid[temp.x, y1].GetComponent<SpriteRenderer>().color == oldColor)
        //    {
        //        y1--;
        //    }
        //    y1++;
        //    bool spanLeft = false;
        //    bool spanRight = false;
        //    while (y1 < ground.cols && ground.grid[y1,temp.x].GetComponent<SpriteRenderer>().color == oldColor)
        //    {
        //        ground.grid[y1,temp.x].GetComponent<SpriteRenderer>().color = newColor;
        //        if (!spanLeft && temp.x > 0 && temp.x - 1 > 0 ? ground.grid[y1, temp.x - 1].GetComponent<SpriteRenderer>().color == oldColor : false)
        //        {
        //            pixels.Push(new Point(y1, temp.x - 1));
        //            spanLeft = true;
        //            //ground.grid[y1,temp.x - 1].GetComponent<SpriteRenderer>().color = newColor;
        //        }
        //        else if (spanLeft && temp.x - 1 == 0 && temp.x - 1 > 0 ? ground.grid[y1, temp.x - 1].GetComponent<SpriteRenderer>().color != oldColor : false)
        //        {
        //            spanLeft = false;
        //        }
        //        if (!spanRight && temp.x < ground.rows - 1 && temp.x + 1 < ground.rows ? ground.grid[y1, temp.x + 1].GetComponent<SpriteRenderer>().color == oldColor : false)
        //        {
        //            pixels.Push(new Point(y1, temp.x + 1));
        //            spanRight = true;
        //            //ground.grid[y1, temp.x + 1].GetComponent<SpriteRenderer>().color = newColor;
        //        }
        //        else if (spanRight && temp.x < ground.rows - 1 && temp.x + 1 < ground.rows ? ground.grid[y1, temp.x + 1].GetComponent<SpriteRenderer>().color != oldColor : false)
        //        {
        //            spanRight = false;
        //        }
        //        y1++;
        //    }
        //}
    }
    public void CheckNeighbors(List<Vector2> pos, Color newColor)
    {
        var counter = 0;
        var x = 0;
        var y = 0;
        foreach(var i in pos)
        {
            if (i.x + 1 < ground.rows && ImageCheck(i.x + 1, i.y, newColor))
                counter++;
            if (i.x - 1 > 0 && ImageCheck(i.x - 1, i.y, newColor))
                counter++;
            if (i.y + 1 < ground.cols && ImageCheck(i.x , i.y + 1, newColor))
                counter++;
            if (i.y - 1 > 0 && ImageCheck(i.x , i.y - 1, newColor))
                counter++;
        }
        x = (int)pos.Average(x => x.x);
        y = (int)pos.Average(y => y.y);
        //Debug.Log(new Vector2(x,y));
        if (counter >= pos.Count)
        {
            pos.Clear();
            FloodFill(x, y, newColor);
        }

    }
    bool ImageCheck(float x,float y,Color newColor)
    {
        if (ground.grid[(int)x, (int)y].GetComponent<SpriteRenderer>().color == newColor)
            return true;
        return false;
    }
}
