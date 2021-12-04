using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public GameObject bombPrefab;
    public Transform spawnBulletPosition;
    public GameObject bullet;
    public LayerMask shootLayerMask;

    public RandomMazeGenerator.Cell[,] cells;
    private RandomMazeGenerator randomMazeGenerator;
    private GameManager gameManager;

    private int[] currentLocation;
    private List<int[]> path;
    private List<int[]> visitedCells;
    private Stack<int[]> exitPath;
    private bool exitFound = false;
    private bool keyFound = false;
    private bool reachedDestination = true;
    private int[] exitLocation;
    private int[] keyLocation1;
    private int[] keyLocation2;
    private bool isWalking = false;
    private bool playerInRange = false;
    private bool enemyInRange = false;
    private Transform aimDestination = null;

    private const float runMoveDuration = 1.5f;
    private const float walkMoveDuration = 3.75f;
    private const float runSpeedAnimation = 5.335f;
    private const float walkSpeedAnimation = 2f;
    [SerializeField]
    private float walkTime = 10f;

    [SerializeField]
    private float bombCooldownTime = 10f;
    private float bombTimeStamp = 0f;
    [SerializeField]
    private float shootCooldownTime = 10f;
    private float shootTimeStamp = 0f;
    [SerializeField]
    private float shootRange = 10f;

    private Animator animator;


    private void Awake()
    {
        randomMazeGenerator = RandomMazeGenerator.instance;
        gameManager = GameManager.instance;
        animator = GetComponent<Animator>();
        animator.SetFloat("MotionSpeed", 1f);
    }

    void Start()
    {
        cells = randomMazeGenerator.cells;
        currentLocation = GameManager.instance.initialLocation;
        exitLocation = GameManager.instance.exitLocation;
        keyLocation1 = GameManager.instance.keyLocation1;
        keyLocation2 = GameManager.instance.keyLocation2;
        path = new List<int[]>();
        visitedCells = new List<int[]>();
        exitPath = new Stack<int[]>();
        exitPath.Push(exitLocation);
        path.Add(currentLocation);
        visitedCells.Add(currentLocation);
        shootTimeStamp = Time.time + shootCooldownTime;
        bombTimeStamp = Time.time + bombCooldownTime;
    }

    void Update()
    {
        if (reachedDestination)
        {
            ChooseAction();
        }

        if(aimDestination != null)
        {
            transform.LookAt(aimDestination);
        }
    }

    public void ChoosePath(int[] location)
    {
        reachedDestination = false;
        List<int> availablePaths = new List<int>();
        List<int[]> availableCells = new List<int[]>();
        List<int[]> unvisitedAvailableCells = new List<int[]>();

        for (int i = 0; i < 4; i++)
        {
            if (!cells[location[0], location[1]].hasWalls[i])
            {
                availablePaths.Add(i);
            }
        }

        foreach (int path in availablePaths)
        {
            if (path == 0)
            {
                availableCells.Add(new int[] { location[0] - 1, location[1] });
            }
            else if (path == 1)
            {
                availableCells.Add(new int[] { location[0], location[1] - 1 });
            }
            else if (path == 2)
            {
                availableCells.Add(new int[] { location[0] + 1, location[1] });
            }
            else
            {
                availableCells.Add(new int[] { location[0], location[1] + 1 });
            }
        }

        foreach (int[] cell in availableCells)
        {
            if (!visitedCells.Any(p => p.SequenceEqual(cell)))
            {
                unvisitedAvailableCells.Add(cell);
            }
        }

        if (unvisitedAvailableCells.Count == 0)
        {
            MoveBackward(path[path.Count - 2]);
        }
        else
        {
            int selectedCellIndex = Random.Range(0, unvisitedAvailableCells.Count);
            MoveForward(unvisitedAvailableCells[selectedCellIndex]);
        }
    }

    public void ChoosePathToExit()
    {
        reachedDestination = false;
        currentLocation = exitPath.Pop();
        float moveDuration = isWalking ? walkMoveDuration : runMoveDuration;
        StartCoroutine(MoveToLocation(gameManager.ConvertCellToLocation(currentLocation, transform.position.y),
            moveDuration));
    }

    private void MoveForward(int[] cell)
    {
        visitedCells.Add(cell);
        path.Add(cell);
        float moveDuration = isWalking ? walkMoveDuration : runMoveDuration;
        StartCoroutine(MoveToLocation(gameManager.ConvertCellToLocation(cell, transform.position.y),
            moveDuration));
        currentLocation = cell;
    }

    private void MoveBackward(int[] cell)
    {
        path.Remove(path[path.Count - 1]);
        float moveDuration = isWalking ? walkMoveDuration : runMoveDuration;
        StartCoroutine(MoveToLocation(gameManager.ConvertCellToLocation(cell, transform.position.y),
            moveDuration));
        currentLocation = cell;
    }

    private IEnumerator MoveToLocation(Vector3 location, float duration)
    {
        float time = 0f;
        Vector3 startPosition = transform.position;

        transform.LookAt(location);
        float speedAnimation = isWalking ? walkSpeedAnimation : runSpeedAnimation;
        animator.SetFloat("Speed", speedAnimation);

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, location, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = location;
        reachedDestination = true;
        animator.SetFloat("Speed", 0f);
    }

    private void Move()
    {
        if (!currentLocation.SequenceEqual(keyLocation1) && !currentLocation.SequenceEqual(keyLocation2) && !keyFound)
        {
            if (currentLocation.SequenceEqual(exitLocation))
            {
                exitFound = true;
            }

            ChoosePath(currentLocation);
        }
        else
        {
            keyFound = true;
            if (!currentLocation.SequenceEqual(exitLocation) && !exitFound)
            {
                ChoosePath(currentLocation);
            }
            else
            {
                exitFound = true;
            }
        }

        if (exitFound && !keyFound)
        {
            if (!exitPath.Contains(currentLocation))
            {
                exitPath.Push(currentLocation);
            }
            else
            {
                //Pop the wrong paths
                exitPath.Pop();
            }
        }

        if (exitFound && keyFound)
        {
            //Pop the current keyLocation
            if (currentLocation.SequenceEqual(keyLocation1) || currentLocation.SequenceEqual(keyLocation2))
            {
                exitPath.Pop();
            }

            if (!currentLocation.SequenceEqual(exitLocation))
            {
                ChoosePathToExit();
            }
        }

    }

    public void ChooseAction()
    {
        SetPlayerInRange();

        int rnd = Random.Range(0, 4);
        if (rnd == 0)
        {
            Move();
        }
        else if (rnd == 1 && bombTimeStamp <= Time.time)
        {
            DropBomb();
            bombTimeStamp = Time.time + bombCooldownTime;
        }
        else if (rnd == 2 && shootTimeStamp <= Time.time && playerInRange)
        {
            ShootPlayer();
            shootTimeStamp = Time.time + shootCooldownTime;
        }
        else if (rnd == 3 && shootTimeStamp <= Time.time && enemyInRange)
        {
            ShootEnemy();
            shootTimeStamp = Time.time + shootCooldownTime;
        }
    }

    public void SetIsWalking()
    {
        StartCoroutine(StartWalkProcess());
    }

    private IEnumerator StartWalkProcess()
    {
        if (!isWalking)
        {
            isWalking = true;
            yield return new WaitForSeconds(walkTime);
            isWalking = false;
        }
    }

    private void DropBomb()
    {
        Instantiate(bombPrefab, transform.position + transform.forward, Quaternion.identity);
    }

    private void ShootPlayer()
    {
        reachedDestination = false;
        StartCoroutine(Shoot(GameManager.instance.player.transform));
    }

    private void ShootEnemy()
    {

    }

    private IEnumerator Shoot(Transform destination)
    {
        aimDestination = destination;
        animator.SetBool("Aim", true);
        yield return new WaitForSeconds(2f);
        Vector3 aimDir = ((destination.position + Vector3.up) - spawnBulletPosition.position).normalized;
        Instantiate(bullet, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
        animator.SetBool("Aim", false);
        reachedDestination = true;
        aimDestination = null;
    }

    private void SetPlayerInRange()
    {
        Vector3 playerPosition = GameManager.instance.player.transform.position;
        playerInRange = Vector3.Distance(playerPosition,
            transform.position) <= shootRange;

        if (playerInRange)
        {
            if (Physics.Raycast(spawnBulletPosition.position, (playerPosition + Vector3.up) - spawnBulletPosition.position,
            out RaycastHit hit, 999f, shootLayerMask))
            {
                playerInRange = true;
            }
            else
                playerInRange = false;
        }
    }

}
