using UnityEngine;

public class GridInitialiser : MonoBehaviour
{
    [Header("Grid Settings")]
    public PrefabsSO prefabsSO;
    GameObject vertextprefab; // assigned via prefabSO
    public int columnsCount;
    [HideInInspector] public int rowsCount;

    private Camera mainCamera;
    private Vector3 safeBottomLeft;
    public float spacing;

    void Start()
    {
        CacheReferences();
        if (!ValidateInputs()) return;
        CalculateSafeAreaBounds();
        SpawnGrid();
    }

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

    void CacheReferences()
    {
        mainCamera = Camera.main;
        vertextprefab = prefabsSO.vertexPrefab;
        rowsCount = columnsCount; // square grid
    }

    void CalculateSafeAreaBounds()
    {
        Rect safeArea = Screen.safeArea;

        Vector3 bottomLeftScreen = new Vector3(safeArea.xMin, safeArea.yMin, mainCamera.nearClipPlane + 10);
        Vector3 topRightScreen = new Vector3(safeArea.xMax, safeArea.yMax, mainCamera.nearClipPlane + 10);

        safeBottomLeft = mainCamera.ScreenToWorldPoint(bottomLeftScreen);
        Vector3 safeTopRight = mainCamera.ScreenToWorldPoint(topRightScreen);

        spacing = (safeTopRight.x - safeBottomLeft.x) / (columnsCount - 1);
    }

    void SpawnGrid()
    {
        // Calculate total grid size
        float gridWidth = (columnsCount - 1) * spacing;
        float gridHeight = (rowsCount - 1) * spacing;

        // Center position in world space (screen center in safe area)
        Vector3 safeCenterScreen = new Vector3(
            Screen.safeArea.center.x,
            Screen.safeArea.center.y,
            mainCamera.nearClipPlane + 10
        );
        Vector3 safeCenterWorld = mainCamera.ScreenToWorldPoint(safeCenterScreen);

        // Adjust start position so grid is centered
        Vector3 startPos = new Vector3(
            safeCenterWorld.x - gridWidth / 2f,
            safeCenterWorld.y - gridHeight / 2f,
            0f
        );

        for (int row = 0; row < rowsCount; row++)
        {
            for (int col = 0; col < columnsCount; col++)
            {
                Vector3 spawnPos = startPos + new Vector3(col * spacing, row * spacing, 0f);
                Instantiate(vertextprefab, spawnPos, Quaternion.identity);
            }
        }
    }
}
