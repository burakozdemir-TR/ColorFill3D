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
    private List<Vector2> pos;
    private Point point;
    private Vector3 lastMovement;
    private int middleX;
    private int middleY;
    private void Start()
    {
        Application.targetFrameRate = 60;
        ground = GetComponentInParent<Grid>();
        pos = new List<Vector2>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            FloodFill(new Vector2(middleX, middleY), Color.red);
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
    private void DetectPosition()
    {
        var ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            point = hit.transform.gameObject.GetComponent<Point>();
            FillShape(point, Color.white);
        }
    }
    private void FillShape(Point pt, Color oldColor)
    {
        if (oldColor == ground.grid[pt.y, pt.x].GetComponent<SpriteRenderer>().color)
        {
            pos.Add(pt.xy);
            FindMiddlePoint(pos);
            ground.grid[pt.y, pt.x].GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    public void FindMiddlePoint(List<Vector2> pos)
    {
        middleX = 0;
        middleY = 0;
        middleX = (int)pos.Average(x => x.x);
        middleY = (int)pos.Average(y => y.y);
        //find middle point in closed shape
        Debug.Log(new Vector2(middleX , middleY));
    }
    public void FloodFill(Vector2 middle , Color newColor)
    {
        if (middle.x + 1 > ground.rows || middle.y + 1 > ground.cols || middle.x < 0 || middle.y < 0)
            return;
        if (newColor == ground.grid[(int)middle.y, (int)middle.x].GetComponent<SpriteRenderer>().color)
            return;

        ground.grid[(int)middle.y, (int)middle.x].GetComponent<SpriteRenderer>().color = newColor;

        FloodFill(new Vector2(middle.x - 1, middle.y), newColor);
        FloodFill(new Vector2(middle.x, middle.y + 1), newColor);
        FloodFill(new Vector2(middle.x + 1, middle.y), newColor);
        FloodFill(new Vector2(middle.x, middle.y - 1), newColor);
        pos.Clear();
    }
}
