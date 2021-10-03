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
    Vector3 lastMovement;
    int x;
    int y;
    private void Start()
    {
        Application.targetFrameRate = 60;
        ground = GetComponentInParent<Grid>();
        pos = new List<Vector2>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("TTTTTTTTEEEEEEEEESSSSSSSSTTTTTTTTTTTT");
            CheckMiddle(new Vector2(x, y), Color.red);
        }
        MoveSequance();
    }
    private void MoveSequance()
    {
        if (Input.GetKey(KeyCode.W) && !isMoving)
        {
            StartCoroutine(MoveCharacter(Vector3.up));
            lastMovement = Vector3.up;
        }
        if (Input.GetKey(KeyCode.A) && !isMoving)
        {
            StartCoroutine(MoveCharacter(Vector3.left));
            lastMovement = Vector3.left;
        }
        if (Input.GetKey(KeyCode.S) && !isMoving)
        {
            StartCoroutine(MoveCharacter(Vector3.down));
            lastMovement = Vector3.down;
        }
        if (Input.GetKey(KeyCode.D) && !isMoving)
        {
            StartCoroutine(MoveCharacter(Vector3.right));
            lastMovement = Vector3.right;
        }
    }
    private IEnumerator MoveCharacter(Vector3 direction)
    {
        DetectPosition(direction);
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
    private void DetectPosition(Vector3 direction)
    {
        var ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            point = hit.transform.gameObject.GetComponent<Point>();

            MyTestFunction(point, Color.white, Color.red, direction);
        }
    }
    private void MyTestFunction(Point pt, Color oldColor, Color newColor, Vector3 direction)
    {
        if (oldColor == ground.grid[pt.y, pt.x].GetComponent<SpriteRenderer>().color)
        {
            // if (
            //(direction == Vector3.left && lastMovement == Vector3.right)
            //|| (direction == Vector3.right && lastMovement == Vector3.left)
            //|| (direction == Vector3.down && lastMovement == Vector3.up)
            //|| (direction == Vector3.up && lastMovement == Vector3.down)
            //)return;
            // if (direction != lastMovement)
            // {
            //     pos.Add(pt.xy);
            //     CheckNeighbors(pos, newColor);
            // }
            //foreach (var i in pos)
            //{
            //    Debug.Log(i.ToString());
            //} 
            pos.Add(pt.xy);
            CheckNeighbors(pos, newColor);
            ground.grid[pt.y, pt.x].GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    //void FloodFill(int x, int y, Color newColor)
    //{
    //    if (x + 1 > ground.rows || y + 1 > ground.cols || x < 0 || y < 0)
    //        return;
    //    if (newColor == ground.grid[y, x].GetComponent<SpriteRenderer>().color)
    //        return;

    //    ground.grid[x , y].GetComponent<SpriteRenderer>().color = newColor;

    //    FloodFill(x - 1 , y , newColor);
    //    FloodFill(x , y + 1 ,  newColor);
    //    FloodFill(x + 1 , y , newColor);
    //    FloodFill(x , y - 1 ,  newColor);
    //    FloodFill(x + 1 , y - 1 , newColor);
    //    FloodFill(x + 1 , y + 1 , newColor);
    //    FloodFill(x - 1 , y + 1 , newColor);
    //    FloodFill(x - 1 , y - 1 , newColor);
    //    pos.Clear();
    //}
    
    public void CheckNeighbors(List<Vector2> pos, Color newColor)
    {
        var counter = 0;
        x = 0;
        y = 0;
        //foreach(var i in pos)
        //{
        //    //wall
        //    if (i.x + 1 > ground.rows || i.x - 1 < 0 || i.y + 1 > ground.cols || i.y - 1 < 0)
        //        counter++;
        //    if (i.x + 1 < ground.rows && ImageCheck(i.x + 1, i.y, newColor))
        //        counter++;
        //    if (i.x - 1 > 0 && ImageCheck(i.x - 1, i.y, newColor))
        //        counter++;
        //    if (i.y + 1 < ground.cols && ImageCheck(i.x , i.y + 1, newColor))
        //        counter++;
        //    if (i.y - 1 > 0 && ImageCheck(i.x , i.y - 1, newColor))
        //        counter++;
        //}
        x = (int)pos.Average(x => x.x);
        y = (int)pos.Average(y => y.y);
        Debug.Log(new Vector2(x,y));
        //Debug.Log(counter);
        //Debug.Log(pos.Count);
        //foreach(var i in pos)
        //{
        //    Debug.Log(i.sqrMagnitude);
        //}
        
        //if (!isMoving)
            //CheckMiddle(new Vector2(x,y),newColor);
            //FloodFill(y, x, newColor);
        //return new Vector2(x, y);
        //if (counter >= pos.Count * 2 || counter + 1 >= pos.Count * 2)
        //{
        //    //pos.Clear();
        //    FloodFill(x, y, newColor);
        //}
    }
    public void CheckMiddle(Vector2 middle , Color newColor)
    {
        if (middle.x + 1 > ground.rows || middle.y + 1 > ground.cols || middle.x < 0 || middle.y < 0)
            return;
        if (newColor == ground.grid[(int)middle.y, (int)middle.x].GetComponent<SpriteRenderer>().color)
            return;

        ground.grid[(int)middle.y, (int)middle.x].GetComponent<SpriteRenderer>().color = newColor;

        CheckMiddle(new Vector2(middle.x - 1, middle.y), newColor);
        CheckMiddle(new Vector2(middle.x, middle.y + 1), newColor);
        CheckMiddle(new Vector2(middle.x + 1, middle.y), newColor);
        CheckMiddle(new Vector2(middle.x, middle.y - 1), newColor);
        pos.Clear();
    }
    bool ImageCheck(float x,float y,Color newColor)
    {
        if (ground.grid[(int)x, (int)y].GetComponent<SpriteRenderer>().color == newColor)
            return true;
        return false;
    }
}
