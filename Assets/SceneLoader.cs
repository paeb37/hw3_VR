using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene JSON File (from StreamingAssets)")]
    public string jsonFileName = "scene_data.json";

    [Header("Parent for All Loaded Objects")]
    public Transform sceneRoot;

    private string FullPath => Path.Combine(Application.streamingAssetsPath, jsonFileName);

    [System.Serializable]
    public class ObjectTransformData
    {
        public string name;
        public Vector3 position;
        public Quaternion rotation;
    }

    [System.Serializable]
    public class SceneData
    {
        public ObjectTransformData[] objects;
    }

    public void LoadScene()
    {
        if (!File.Exists(FullPath))
        {
            Debug.LogError("❌ Scene JSON file not found: " + FullPath);
            return;
        }

        string json = File.ReadAllText(FullPath);
        SceneData sceneData = JsonUtility.FromJson<SceneData>(json);

        if (sceneData == null || sceneData.objects == null)
        {
            Debug.LogError("❌ Invalid scene JSON format.");
            return;
        }

        int loadedCount = 0;

        foreach (var objData in sceneData.objects)
        {
            string objName = objData.name.Trim();

            if (!objName.ToLower().Contains("parent"))
                continue;

            string cleanName = objName.Replace("(Parent)", "").Replace("(parent)", "").Trim();
            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + cleanName);

            if (prefab == null)
            {
                Debug.LogWarning($"⚠️ Prefab not found: {cleanName}");
                continue;
            }

            GameObject instance = Instantiate(prefab, objData.position, objData.rotation, sceneRoot);
            instance.AddComponent<SceneObjectTag>().originalName = cleanName;

            Debug.Log($"✅ Loaded prefab: {cleanName} at {objData.position}");
            loadedCount++;
        }

        Debug.Log($"✅ Scene load complete. Total parent prefabs loaded: {loadedCount}");
    }

    void Start()
    {
        Debug.Log("🟡 SceneLoader Start() called");
        LoadScene();
    }
}




