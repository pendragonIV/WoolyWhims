using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Line
    private LineRenderer lineRenderer;
    [SerializeField]
    private GameObject linePrefab;
    [SerializeField]
    private Transform lineContainer;
    [SerializeField]
    private List<GameObject> initializedLine;
    [SerializeField]
    private GameObject currentLine;

    #endregion

    #region Sprite 
    [SerializeField]
    private Sprite[] playerSprites;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    #endregion

    #region Movement variables
    [SerializeField]
    private bool isMoving;
    private Vector2 movementDirection;

    #endregion

    private void OnMove(InputValue value)
    {
        
        movementDirection = value.Get<Vector2>();
    }

    private void Start()
    {

        SetPlayerCenterCell();
        InitNewLine();
        GridCellManager.instance.AddMovedCell(GridCellManager.instance.GetObjCell(transform.position));

    }

    private void Update()
    {
        if (movementDirection != Vector2.zero && !isMoving)
        { 
            isMoving = true;
            MoveObj();
            movementDirection = Vector2.zero;
        }
        else
        {
            movementDirection = Vector2.zero;
        }

    }

    private void FixedUpdate()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    private void MoveObj()
    {
        Vector3Int currentPos = GridCellManager.instance.GetObjCell(transform.position);
        Vector3Int nextPos = currentPos + new Vector3Int((int)movementDirection.x, (int)movementDirection.y, 0);
        if (GridCellManager.instance.IsPlaceableArea(nextPos))
        {
            if (OverlapLine(GridCellManager.instance.PositonToMove(nextPos)) != -1)
            {
                StartCoroutine(MoveBack(OverlapLine(GridCellManager.instance.PositonToMove(nextPos))));
                return;
            }
            InitNewLine();
            GridCellManager.instance.AddMovedCell(nextPos);
            CheckWin();
            transform.DOMove(GridCellManager.instance.PositonToMove(nextPos), .1f).OnComplete(() => isMoving = false);
        }
        else
        {
            isMoving = false;
        }
    }

    private IEnumerator MoveBack(int backToIndex)
    {
        for(int i = initializedLine.Count - 1; i > backToIndex; i--)
        {
            this.transform.DOMove(initializedLine[i].GetComponent<LineRenderer>().GetPosition(0), .1f);
            GridCellManager.instance.RemoveMovedCell(i);
            Destroy(initializedLine[i]);
            initializedLine.RemoveAt(i);
            yield return new WaitForSeconds(.1f);
        }
        isMoving = false;
    }

    private int OverlapLine(Vector3 position)
    {
        for(int i = 0; i < initializedLine.Count; i++)
        {
            if (initializedLine[i].GetComponent<LineRenderer>().GetPosition(1) == position)
            {
                return i;
            }
        }
        return -1;
    }

    #region Checking

    private void CheckWin()
    {
        if (GridCellManager.instance.IsCompleted())
        {
            GameManager.instance.Win();
        }
    }

    #endregion

    #region Line

    private void InitNewLine()
    {
        currentLine = Instantiate(linePrefab, lineContainer);
        currentLine.name = "Line " + initializedLine.Count;
        lineRenderer = currentLine.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);

        initializedLine.Add(currentLine);
    }

    #endregion 


    #region Default Setup
    private void SetPlayerCenterCell()
    {
        Vector3Int cellPos = GridCellManager.instance.GetObjCell(transform.position);
        this.transform.position = GridCellManager.instance.PositonToMove(cellPos);
    }
    #endregion
}
