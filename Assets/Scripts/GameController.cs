using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public Maze mazePrefab;

    private Maze mazeInstance;

    private Coroutine generationCoroutine;

    public Player playerPrefab;

    private Player playerInstance;

    private Camera playerCamera;

    private bool mapIsActive;
    [Header("MAP")]
    [SerializeField] Camera mapCamera;
    [SerializeField] GameObject mapBackground;



    private void Start()
    {
        StartCoroutine(BeginGame());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
        if (Input.GetKeyUp(KeyCode.M)) 
        {
            mapIsActive = !mapIsActive;
            ToggleMap(mapIsActive);
        }
    }

    private IEnumerator BeginGame()
    {
        Camera.main.clearFlags = CameraClearFlags.Skybox;
        Camera.main.rect = new Rect(0f, 0f, 1f, 1f);
        mazeInstance = Instantiate(mazePrefab) as Maze;
        yield return StartCoroutine(mazeInstance.Generate());
        playerInstance = Instantiate(playerPrefab) as Player;
        playerCamera = playerInstance.transform.GetComponentInChildren<Camera>();
        playerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));
        Camera.main.clearFlags = CameraClearFlags.Depth;
        Camera.main.rect = new Rect(0f, 0f, 0.5f, 0.5f);
    }


    private void RestartGame()
    {
        StopCoroutine(generationCoroutine);
        Destroy(mazeInstance.gameObject);
        if (playerInstance != null)
        {
            Destroy(playerInstance.gameObject);
        }
        StartCoroutine(BeginGame());
    }
    private void ToggleMap(bool activate)
    {
        if (activate)
        {
            playerCamera.gameObject.SetActive(false);
            mapBackground.SetActive(true);
            mapCamera.rect = new Rect(0, 0, 1, 1);
            mapCamera.orthographicSize = 10f;
        }
        else 
        {
            playerCamera.gameObject.SetActive(true);
            mapBackground.SetActive(false);
            mapCamera.rect = new Rect(0, 0, 0.25f, 0.5f);
            mapCamera.orthographicSize = 15f;
        }
    }
}
