    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int rows;
    public int cols;
    public float tileSize;
    public GameObject[,] grid;
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid(rows,cols);
        AdjustCamera();
    }
    private void GenerateGrid(int width,int height)
    {
        grid = new GameObject[width, height];
        GameObject referenceTile = (GameObject)Instantiate(Resources.Load("testgrid"));
        for (int row = 0; row < rows; row++)
        {
            for(int col = 0; col < cols; col++)
            {
                GameObject tile = Instantiate(referenceTile, transform);
                float posX = col * tileSize;
                float posY = row * tileSize;
                tile.transform.position = new Vector2(posX,posY);
                grid[row, col] = tile;
            }
        }
        Destroy(referenceTile);
    }
    private void AdjustCamera()
    {
        Camera.main.transform.position = new Vector3(rows/2,cols/2,-rows);
    }
}