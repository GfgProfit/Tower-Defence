using UnityEngine;
using UnityEditor;
using System.IO;

public class ScreenshotTaker : EditorWindow
{
    private int _resolutionMultiplier = 1;
    private string _folderPath = "Screenshots";
    private string _fileName = "screenshot";

    [MenuItem("Tools/Screenshot Taker")]
    private static void ShowWindow()
    {
        GetWindow<ScreenshotTaker>("Screenshot Taker");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Screenshot Settings", EditorStyles.boldLabel);
        _resolutionMultiplier = EditorGUILayout.IntSlider("Resolution Multiplier", _resolutionMultiplier, 1, 5);
        _folderPath = EditorGUILayout.TextField("Save Folder", _folderPath);
        _fileName = EditorGUILayout.TextField("File Name", _fileName);

        if (GUILayout.Button("Take Screenshot (GameView)"))
        {
            TakeGameViewScreenshot();
        }

        if (GUILayout.Button("Take Screenshot (SceneView)"))
        {
            TakeSceneViewScreenshot();
        }
    }

    private void TakeGameViewScreenshot()
    {
        if (!Directory.Exists(_folderPath))
        {
            Directory.CreateDirectory(_folderPath);
        }

        string fullPath = Path.Combine(_folderPath, $"{_fileName}_{System.DateTime.Now:yyyyMMdd_HHmmss}.png");
        ScreenCapture.CaptureScreenshot(fullPath, _resolutionMultiplier);

        Debug.Log($"[ScreenshotTaker] GameView Screenshot saved to: {fullPath}");
    }

    private void TakeSceneViewScreenshot()
    {
        if (SceneView.lastActiveSceneView == null)
        {
            Debug.LogWarning("[ScreenshotTaker] No active SceneView found.");
            return;
        }

        if (!Directory.Exists(_folderPath))
        {
            Directory.CreateDirectory(_folderPath);
        }

        int width = (int)(SceneView.lastActiveSceneView.position.width * _resolutionMultiplier);
        int height = (int)(SceneView.lastActiveSceneView.position.height * _resolutionMultiplier);

        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        SceneView.lastActiveSceneView.camera.targetTexture = renderTexture;
        SceneView.lastActiveSceneView.camera.Render();

        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();

        SceneView.lastActiveSceneView.camera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(renderTexture);

        byte[] bytes = texture.EncodeToPNG();
        string fullPath = Path.Combine(_folderPath, $"{_fileName}_scene_{System.DateTime.Now:yyyyMMdd_HHmmss}.png");
        File.WriteAllBytes(fullPath, bytes);

        DestroyImmediate(texture);

        Debug.Log($"[ScreenshotTaker] SceneView Screenshot saved to: {fullPath}");
    }
}