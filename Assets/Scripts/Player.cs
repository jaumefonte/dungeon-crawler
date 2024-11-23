using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    private MazeCell currentCell;
    bool currentlyMoving;
    [SerializeField] private float translationSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float mouseLookSpeed;
    private MazeDirection currentDirection;
    private Vector3 currentlook = Vector3.zero;
    public void SetLocation(MazeCell cell)
    {
        if (currentCell != null)
        {
            currentCell.OnPlayerExited();
        }
        currentCell = cell;
        transform.position = cell.transform.position;
        
        currentCell.OnPlayerEntered();
    }

    private IEnumerator Move(MazeDirection direction)
    {
        currentlyMoving = true;
        Vector3 targetPosition = transform.position;
        MazeCellEdge edge = currentCell.GetEdge(direction);
        if (edge is MazePassage)
        {
            targetPosition = edge.otherCell.transform.position;
            float sqrRemainingDistance = (transform.position - targetPosition).sqrMagnitude;

            while (sqrRemainingDistance > float.Epsilon)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * translationSpeed);
                sqrRemainingDistance = (transform.position - targetPosition).sqrMagnitude;
                yield return null;
            }

            SetLocation(edge.otherCell);
        }
        GameController.instance.PlayerMoves();
        currentlyMoving = false;
    }


    private IEnumerator Look(MazeDirection direction)
    {
        currentlyMoving = true;
        while (transform.rotation != direction.ToRotation()) 
        { 
            transform.rotation = Quaternion.RotateTowards(transform.rotation, direction.ToRotation(), Time.deltaTime * rotationSpeed);
            yield return null;
        }
        currentDirection = direction;
        currentlook = transform.eulerAngles;
        currentlyMoving = false;
    }
    private void MouseLook()
    {        
        currentlook.y += Input.GetAxis("Mouse X")* mouseLookSpeed;
        transform.rotation = Quaternion.Euler(currentlook);
    }
    private void SetDirection()
    {
        StartCoroutine(Look(MazeDirections.FromEuler(currentlook)));
    }
    private void Update()
    {
        if (!currentlyMoving && !GameController.instance.inBattle) 
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                MouseLook();
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                SetDirection();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                StartCoroutine(Move(currentDirection));
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(Move(currentDirection.GetNextClockwise()));
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                StartCoroutine(Move(currentDirection.GetOpposite()));
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(Move(currentDirection.GetNextCounterclockwise()));
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                StartCoroutine(Look(currentDirection.GetNextCounterclockwise()));
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(Look(currentDirection.GetNextClockwise()));
            }
        }
        
    }
}
