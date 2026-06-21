using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class GradientTextureCreator : EditorWindow
    {

    public float minTemp = -50f;     
    public float maxTemp = 50f;       
    
    public float freezeThreshold = -20f; 
    public float burnThreshold = 30f;   


    private Color _coldColor = new Color(0, 0.5f, 1, 1); 
    private Color _safeColor = new Color(0.5f, 0.5f, 0.5f, 1);
    private Color _hotColor = new Color(1, 0.2f, 0, 1);

    private readonly int _width = 256;
    private readonly int _height = 32;

    [MenuItem("Tools/Generate Temperature Texture")]
    public static void ShowWindow()
    {
        GetWindow<GradientTextureCreator>("Temp Gradient Gen");
    }

    private void OnGUI()
    {
        GUILayout.Label("Slider range", EditorStyles.boldLabel);
        minTemp = EditorGUILayout.FloatField("Min Slider Temp", minTemp);
        maxTemp = EditorGUILayout.FloatField("Max Slider Temp", maxTemp);

        GUILayout.Space(10);
        GUILayout.Label("Player threshold", EditorStyles.boldLabel);
        freezeThreshold = EditorGUILayout.FloatField("Freeze Threshold", freezeThreshold);
        burnThreshold = EditorGUILayout.FloatField("Burn Threshold", burnThreshold);

        GUILayout.Space(10);
        GUILayout.Label("Colors", EditorStyles.boldLabel);
        _coldColor = EditorGUILayout.ColorField("Cold", _coldColor);
        _safeColor = EditorGUILayout.ColorField("Safe", _safeColor);
        _hotColor = EditorGUILayout.ColorField("Hot", _hotColor);
        
        float pFreeze = Mathf.InverseLerp(minTemp, maxTemp, freezeThreshold);
        float pBurn = Mathf.InverseLerp(minTemp, maxTemp, burnThreshold);
        
        GUILayout.Space(10);

        GUILayout.Space(10);
        if (GUILayout.Button("Save Texture to Assets", GUILayout.Height(40)))
        {
            CreateAndSaveTexture(pFreeze, pBurn);
        }
    }

    private void CreateAndSaveTexture(float freezePct, float burnPct)
    {
        Texture2D texture = new Texture2D(_width, _height);
        
        freezePct = Mathf.Clamp01(freezePct);
        burnPct = Mathf.Clamp(burnPct, freezePct, 1f);

        for (int x = 0; x < _width; x++)
        {
            float t = (float)x / (_width - 1);
            Color col;

            if (t < freezePct)
            {
                float localT = t / freezePct;
                col = Color.Lerp(_coldColor, _safeColor, localT);
            }
            else if (t > burnPct)
            {
                float localT = (t - burnPct) / (1f - burnPct);
                col = Color.Lerp(_safeColor, _hotColor, localT);
            }
            else
            {
                col = _safeColor;
            }

            for (int y = 0; y < _height; y++)
            {
                texture.SetPixel(x, y, col);
            }
        }

        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        string path = Application.dataPath + "/TemperatureGradient.png";
        File.WriteAllBytes(path, bytes);

        AssetDatabase.Refresh();

        string assetPath = "Assets/TemperatureGradient.png";
        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
        if (importer)
        {
            importer.textureType = TextureImporterType.Sprite; 
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.filterMode = FilterMode.Bilinear;
            importer.SaveAndReimport();
        }

        Debug.Log($"Texture Done! Freeze: {freezeThreshold}, Burn: {burnThreshold}");
    }
}
}