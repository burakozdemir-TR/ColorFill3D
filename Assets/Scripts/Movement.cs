using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private bool isMoving;
    private Vector3 origPos, targetPos;
    private float timeToMove = .2f;
    private Grid ground;
    public Point point;
    private void Start()
    {
        Application.targetFrameRate = 60;
        ground = GetComponentInParent<Grid>();
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
        if (isMoving)
            Debug.Log("immovin");
        else
            Debug.Log("NNNNNNNNNNNNNNNNNOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");   
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
   






    private void MyTestFunction(Point pt, Color oldColor, Color newColor)
    {
        if (newColor == ground.grid[pt.y, pt.x].GetComponent<SpriteRenderer>().color)
            return;
        else
            ground.grid[pt.y, pt.x].GetComponent<SpriteRenderer>().color = newColor;
        //return;
        Stack<Point> pixels = new Stack<Point>();
        pixels.Push(pt);
        while (pixels.Count != 0)
        {
            Point temp = pixels.Pop();
            int y1 = temp.y;
            while (y1 >= 0 && ground.grid[y1, temp.x].GetComponent<SpriteRenderer>().color == oldColor)
            {
                y1--;
            }
            y1++;
            bool spanLeft = false;
            bool spanRight = false;
            while (y1 < ground.cols && ground.grid[y1, temp.x].GetComponent<SpriteRenderer>().color == oldColor)
            {
                if (!spanLeft && temp.x > 0 && temp.x - 1 > 0 ? ground.grid[y1, temp.x - 1].GetComponent<SpriteRenderer>().color == oldColor : false)
                {
                    pixels.Push(new Point(y1, temp.x - 1));
                    ground.grid[y1, temp.x - 1].GetComponent<SpriteRenderer>().color = newColor;
                    spanLeft = true;
                }
                else if (spanLeft && temp.x - 1 == 0 && temp.x - 1 > 0 ? ground.grid[y1, temp.x - 1].GetComponent<SpriteRenderer>().color != oldColor : false)
                {
                    spanLeft = false;
                }
                if (!spanRight && temp.x < ground.rows - 1 && temp.x + 1 > ground.rows ? ground.grid[y1, temp.x + 1].GetComponent<SpriteRenderer>().color == oldColor : false)
                {
                    pixels.Push(new Point(y1, temp.x + 1));
                    ground.grid[y1, temp.x + 1].GetComponent<SpriteRenderer>().color = newColor;
                    spanRight = true;
                }
                else if (spanRight && temp.x < ground.rows - 1 && temp.x + 1 > ground.rows ? ground.grid[y1, temp.x + 1].GetComponent<SpriteRenderer>().color != oldColor : false)
                {
                    spanRight = false;
                }
                y1++;
            }
        }
    }
}
