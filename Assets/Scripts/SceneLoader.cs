using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;


// Add SceneObjectTag class definition
public class SceneObjectTag : MonoBehaviour
{
   public string originalName;
}


public class SceneLoader : MonoBehaviour
{
   [Header("Scene JSON File (from StreamingAssets)")]
   public string jsonFileName = ""; // set in inspector




   [Header("Main Transform for Scene Hierarchy")]
   public Transform mainTransform; // assign in inspector




   private string FullPath => Path.Combine(Application.streamingAssetsPath, jsonFileName);




   [System.Serializable]
   public class ObjectTransformData
   {
       [SerializeField]
       public string name;
       [SerializeField]
       public Vector3 position;
       [SerializeField]
       public Vector3 rotation;
       [SerializeField]
       public Vector3 scale;
   }




   [System.Serializable]
   public class SceneData
   {
       [SerializeField]
       public List<ObjectTransformData> objects;  // Changed from array to List
   }




   public void LoadScene()
   {
       if (mainTransform == null)
       {
           Debug.LogError("❌ Main Transform is not assigned. Please assign a Transform in the inspector.");
           return;
       }

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
       Dictionary<string, GameObject> roomParents = new Dictionary<string, GameObject>();
       Dictionary<string, GameObject> floorParents = new Dictionary<string, GameObject>();

       // First pass: Create floor parents as children of MainTransform
       foreach (var objData in sceneData.objects)
       {
           string objName = objData.name.Trim();
           
           // Check if this is a floor object with room label (e.g., "Floor1 (Room1)")
           if (objName.StartsWith("Floor") && objName.Contains("(Room"))
           {
               // Extract floor number and room label
               string[] parts = objName.Split('(');
               string floorName = parts[0].Trim();
               string roomLabel = "(" + parts[1].Trim();
               
               // Create a unique key for this floor
               string floorKey = floorName + roomLabel;
               
               if (!floorParents.ContainsKey(floorKey))
               {
                   // Load the Floor prefab
                   Debug.Log($"🔍 Attempting to load floor prefab: Resources/Prefabs/Floor");
                   GameObject floorPrefab = Resources.Load<GameObject>("Prefabs/Floor");
                   if (floorPrefab == null)
                   {
                       Debug.LogWarning($"⚠️ Floor prefab not found. Make sure 'Floor' exists in Resources/Prefabs/ folder.");
                       continue; // Skip this floor if prefab not found
                   }
                   
                   // Instantiate the Floor prefab as a child of MainTransform
                   GameObject floorParent = Instantiate(floorPrefab, mainTransform);
                   floorParent.name = floorName;
                   
                   // Set local position, rotation, and scale
                   floorParent.transform.localPosition = objData.position;
                   floorParent.transform.localRotation = Quaternion.Euler(objData.rotation);
                   floorParent.transform.localScale = objData.scale;
                   
                   floorParents[floorKey] = floorParent;
                   floorParent.AddComponent<SceneObjectTag>().originalName = "Floor";
                   Debug.Log($"🏢 Created floor from prefab: {floorParent.name} for {roomLabel} at local position {objData.position}");
               }
           }
       }

       // Second pass: instantiate AttachTransform objects as children of their respective floors
       foreach (var objData in sceneData.objects)
       {
           string objName = objData.name.Trim();

           // Extract room info using regex
           Match roomMatch = Regex.Match(objName, @"\(Room\d+\)");
           string roomLabel = roomMatch.Success ? roomMatch.Value : null;

           // Extract base name
           string baseName = objName.Split('(')[0].Trim();

           // Remove "(Clone)" suffix if present
           baseName = baseName.Replace("(Clone)", "").Trim();

           // Skip non-room AttachTransforms
           if (baseName == "AttachTransform" && (roomLabel == null || !objName.Contains(roomLabel)))
           {
               Debug.Log($"⚠️ Skipping AttachTransform variant: {objName}");
               continue;
           }

           // Load only AttachTransform first
           if (baseName == "AttachTransform" && roomLabel != null)
           {
               string cleanName = Regex.Replace(objName, @"\(Room\d+\)", "").Trim();
               // Remove "(Clone)" suffix if present
               cleanName = cleanName.Replace("(Clone)", "").Trim();
               
               Debug.Log($"🔍 Attempting to load prefab: Resources/Prefabs/{cleanName}");
               GameObject prefab = Resources.Load<GameObject>("Prefabs/" + cleanName);
               if (prefab == null)
               {
                   Debug.LogWarning($"⚠️ Prefab not found: {cleanName}. Make sure it exists in Resources/Prefabs/ folder.");
                   continue;
               }
               
               // Find the corresponding floor for this room
               string floorKey = null;
               foreach (var key in floorParents.Keys)
               {
                   if (key.Contains(roomLabel))
                   {
                       floorKey = key;
                       break;
                   }
               }
               
               // Use the floor as parent if available, otherwise use MainTransform
               Transform parentTransform = floorKey != null && floorParents.ContainsKey(floorKey) ? 
                   floorParents[floorKey].transform : mainTransform;
               
               // Create the room parent as a child of the floor or MainTransform
               GameObject roomParent = Instantiate(prefab, parentTransform);
               roomParent.name = baseName + " " + roomLabel;
               
               // Set local position, rotation, and scale
               roomParent.transform.localPosition = objData.position;
               roomParent.transform.localRotation = Quaternion.Euler(objData.rotation);
               roomParent.transform.localScale = objData.scale;
               
               roomParents[roomLabel] = roomParent;
               roomParent.AddComponent<SceneObjectTag>().originalName = cleanName;
               Debug.Log($"🏠 Created room parent: {roomParent.name} on {floorKey ?? "MainTransform"} at local position {objData.position}");
           }
       }

       // Third pass: instantiate all other objects under their respective AttachTransform
       foreach (var objData in sceneData.objects)
       {
           string objName = objData.name.Trim();
           Match roomMatch = Regex.Match(objName, @"\(Room\d+\)");
           string roomLabel = roomMatch.Success ? roomMatch.Value : null;
           string baseName = objName.Split('(')[0].Trim();

           // Remove "(Clone)" suffix if present
           baseName = baseName.Replace("(Clone)", "").Trim();

           // Skip AttachTransform again in second pass
           if (baseName == "AttachTransform")
               continue;

           // Skip floor objects as they're already created from prefab
           if (baseName.StartsWith("Floor") && objName.Contains("(Room"))
               continue;

           string cleanName = Regex.Replace(objName, @"\(Room\d+\)", "").Trim();
           // Remove "(Clone)" suffix if present
           cleanName = cleanName.Replace("(Clone)", "").Trim();
           
           Debug.Log($"🔍 Attempting to load prefab: Resources/Prefabs/{cleanName}");
           GameObject prefab = Resources.Load<GameObject>("Prefabs/" + cleanName);
           if (prefab == null)
           {
               Debug.LogWarning($"⚠️ Prefab not found: {cleanName}. Make sure it exists in Resources/Prefabs/ folder.");
               continue;
           }

           // Find the corresponding floor for this room
           string floorKey = null;
           if (roomLabel != null)
           {
               foreach (var key in floorParents.Keys)
               {
                   if (key.Contains(roomLabel))
                   {
                       floorKey = key;
                       break;
                   }
               }
           }

           // Parent to room or floor
           Transform parent = roomLabel != null && roomParents.ContainsKey(roomLabel) ?
               roomParents[roomLabel].transform : 
               (floorKey != null && floorParents.ContainsKey(floorKey) ? floorParents[floorKey].transform : mainTransform);

           // Create the instance as a child of the parent
           GameObject instance = Instantiate(prefab, parent);
           
           // Set local position, rotation, and scale
           instance.transform.localPosition = objData.position;
           instance.transform.localRotation = Quaternion.Euler(objData.rotation);
           instance.transform.localScale = objData.scale;
           
           instance.AddComponent<SceneObjectTag>().originalName = cleanName;
           Debug.Log($"✅ Loaded prefab: {cleanName} in {roomLabel} at local position {objData.position} on {floorKey ?? "MainTransform"}");
           loadedCount++;
       }

       // Clean-up pass: remove any AttachTransform with no children
       foreach (Transform child in mainTransform)
       {
           if (child.name.StartsWith("AttachTransform") && child.childCount == 0)
           {
               Debug.Log($"🗑️ Destroying empty AttachTransform: {child.name}");
               Destroy(child.gameObject);
           }
       }

       Debug.Log($"✅ Scene load complete. Total objects loaded: {loadedCount}");
   }




   void Start()
   {
       Debug.Log("🟡 SceneLoader Start() called");
       LoadScene();
   }
}




