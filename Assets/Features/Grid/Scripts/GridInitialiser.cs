using UnityEngine;

public class GridInitialiser : MonoBehaviour
{
    [Header("Grid Settings")]
    public PrefabsSO prefabsSO;
    GameObject vertextprefab; //assigned via prefabSO
    public int columnsCount;
    [HideInInspector] public int rowsCount;

    private Camera mainCamera;
    private Vector3 safeBottomLeft;
    private float spacing;


    void Awake()
    {

    }
    void Start()
    {
        CacheReferences();
        if (!ValidateInputs()) return;
        CalculateSafeAreaBounds();
        SpawnGrid();
    }

    // Validate input values to prevent runtime errors
    bool ValidateInputs()
    {
        if (vertextprefab == null)
        {
            Debug.LogError("Sphere prefab not assigned.");
            return false;
        }

        if (columnsCount < 2 || rowsCount < 2)
        {
            Debug.LogError("columnsCount and rowsCount must be at least 2.");
            return false;
        }

        return true;
    }

    // Cache camera reference and calculate bottom-left position in world space
    void CacheReferences()
    {
        mainCamera = Camera.main;
        vertextprefab = prefabsSO.vertexPrefab;
        rowsCount = columnsCount; // Assuming a square grid for simplicity
    }

    // Converts the safe area rectangle to world space coordinates
    void CalculateSafeAreaBounds()
    {
        Rect safeArea = Screen.safeArea;

        Vector3 bottomLeftScreen = new Vector3(safeArea.xMin, safeArea.yMin, mainCamera.nearClipPlane + 10);
        Vector3 topRightScreen = new Vector3(safeArea.xMax, safeArea.yMax, mainCamera.nearClipPlane + 10);

        safeBottomLeft = mainCamera.ScreenToWorldPoint(bottomLeftScreen);
        Vector3 safeTopRight = mainCamera.ScreenToWorldPoint(topRightScreen);

        spacing = (safeTopRight.x - safeBottomLeft.x) / (columnsCount - 1);

    }

    // Spawn the grid of spheres using calculated spacing
    void SpawnGrid()
    {
        for (int row = 0; row < rowsCount; row++)
        {
            for (int col = 0; col < columnsCount; col++)
            {
                Vector3 spawnPos = CalculateWorldPosition(col, row);
                Instantiate(vertextprefab, spawnPos, Quaternion.identity);
            }
        }
    }

    Vector3 CalculateWorldPosition(int col, int row)
    {
        float x = safeBottomLeft.x + col * spacing;
        float y = safeBottomLeft.y + row * spacing;
        return new Vector3(x, y, 0f);
    }
}
