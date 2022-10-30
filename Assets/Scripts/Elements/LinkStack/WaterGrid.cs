using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaterGrid : MonoBehaviour
{
    [SerializeField]
    public int row, col;
    [SerializeField]
    Vector2 gridSize;
    [SerializeField]
    Vector2 maxBorderSize;
    [SerializeField]
    GameObject gridPrefab;
    GameObject[,] grids;
    public AnimationBuffer animationBuffer;
    [SerializeField]
    List<Color> colors = new List<Color>();
    TMP_Text resultText;
    void Awake()
    {
        resultText = GetComponentInChildren<TMP_Text>();
        gameObject.AddComponent<AnimationBuffer>();
        gameObject.AddComponent<ChangeTextAnimator>();
        gameObject.AddComponent<WaitAnimator>();
        gameObject.AddComponent<PopAnimator>();
        gameObject.AddComponent<SelfDestroyAnimator>();
        resultText.text = "Total Water Volume : 0";
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void CreateGrid()
    {
        grids = new GameObject[row, col];
        if (gridSize.x * col > maxBorderSize.x) gridSize.x = maxBorderSize.x / col;
        if (gridSize.y * row > maxBorderSize.y) gridSize.y = maxBorderSize.y / row;
        
        Vector2 corner = new Vector2(transform.position.x - gridSize.x * col / 2, transform.position.y - gridSize.y * row / 2);
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
            {
                GameObject newGrid = Instantiate(gridPrefab);
                newGrid.transform.localScale = gridSize;
                newGrid.AddComponent<AnimationBuffer>();
                newGrid.AddComponent<PopAnimator>();
                newGrid.AddComponent<SelfDestroyAnimator>();
                SpriteRenderer sprite = newGrid.GetComponent<SpriteRenderer>();
                sprite.color = new Color(0,0,0,0);
                newGrid.transform.position = corner + new Vector2((j + 0.5f) * gridSize.x, (i + 0.5f) * gridSize.y);
                grids[i, j] = newGrid;
            }
    }
    public void SetColor(int x, int h, int colorType) {
        Color newColor = colors[colorType];
        //Debug.Log("x : " + x + "  h : " + h + "    row : " + row + "  col : " + col);
        animationBuffer.Add(new ChangeColorAnimatorInfo(grids[h, x], newColor));
    }
    public void SetWallColor(int pos, int height, int colorType)
    {
        Color newColor = colors[colorType];
        //Debug.Log("x : " + x + "  h : " + h + "    row : " + row + "  col : " + col);
        for (int i = 0; i < height; i++)
            animationBuffer.Add(new ChangeColorAnimatorInfo(grids[i, pos], newColor));
    }
    public void SetWaterColor(int pos, int minHeight, int maxHeight, int colorType)
    {
        Color newColor = colors[colorType];
        //Debug.Log("x : " + x + "  h : " + h + "    row : " + row + "  col : " + col);
        for (int i = minHeight; i < maxHeight; i++)
            animationBuffer.Add(new ChangeColorAnimatorInfo(grids[i, pos], newColor));       
    }
    IEnumerator _Destroy()
    {
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
            {
                animationBuffer.Add(new SelfDestroyAnimatorInfo(grids[i, j]));
                yield return new WaitForSeconds(0.005f);
                //animationBuffer.Add(new WaitAnimatorInfo(gameObject, 0.1f, false));
            }
        animationBuffer.Add(new SelfDestroyAnimatorInfo(gameObject));
    }
    public void Destroy()
    {
        StartCoroutine(_Destroy());
        /*
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
            {
                animationBuffer.Add(new SelfDestroyAnimatorInfo(grids[i, j]));
                animationBuffer.Add(new WaitAnimatorInfo(gameObject, 0.1f, false));
            }
        animationBuffer.Add(new SelfDestroyAnimatorInfo(gameObject));
        */
    }
    public void UpdateResult(string newText) {
        animationBuffer.Add(new ChangeTextAnimatorInfo(gameObject, newText));
        //resultText.text = newText;
    }
}
