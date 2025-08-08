using UnityEngine;

public class FrameRateDebugger : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private Rect safeAreaRect;

    void Awake()
    {
        Application.targetFrameRate = 120;

        // Get the safe area in screen space
        Rect safeArea = Screen.safeArea;

        // Shift it a bit so text isn't glued to the edge
        safeAreaRect = new Rect(
            safeArea.x + 10f,
            safeArea.y + 10f,
            safeArea.width - 20f,
            safeArea.height - 20f
        );
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int fps = Mathf.CeilToInt(1.0f / deltaTime);

        GUIStyle style = new GUIStyle
        {
            fontSize = 24,
            normal = { textColor = Color.white }
        };

        // Draw inside safe area
        GUI.Label(new Rect(safeAreaRect.x, safeAreaRect.y, 200, 40), $"FPS: {fps}", style);
    }
}
