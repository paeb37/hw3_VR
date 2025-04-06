using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportAreaManager : MonoBehaviour
{
    [Tooltip("The interaction layer for teleportation")]
    public InteractionLayerMask teleportLayer;

    [Tooltip("The transform containing all scene objects")]
    public Transform mainTransform;

    // void Start()
    // {
    //     SetupFloorAreas();
    // }

    public void SetupFloorAreas()
    {
        if (mainTransform == null)
        {
            Debug.LogError("Main Transform is not assigned in TeleportAreaManager");
            return;
        }

        // Iterate through direct children of mainTransform
        foreach (Transform child in mainTransform)
        {
            // Check if this child is tagged as "Floor"
            if (child.CompareTag("Floor"))
            {
                // Add TeleportArea component if it doesn't have one
                if (!child.GetComponent<TeleportationArea>())
                {
                    TeleportationArea teleportArea = child.gameObject.AddComponent<TeleportationArea>();
                    
                    // Configure the teleport area
                    teleportArea.interactionLayers = teleportLayer;
                    teleportArea.matchOrientation = MatchOrientation.None;
                    teleportArea.matchDirectionalInput = false;
                    teleportArea.filterSelectionByHitNormal = true;
                    teleportArea.upNormalToleranceDegrees = 30f;
                }
            }
        }
    }

    // public void MakeAreaTeleportable(GameObject area)
    // {
    //     if (!area.GetComponent<TeleportationArea>())
    //     {
    //         TeleportationArea teleportArea = area.AddComponent<TeleportationArea>();
    //         teleportArea.interactionLayers = teleportLayer;
    //     }
    // }

    // public void MakeAreaNonTeleportable(GameObject area)
    // {
    //     TeleportationArea teleportArea = area.GetComponent<TeleportationArea>();
    //     if (teleportArea)
    //     {
    //         Destroy(teleportArea);
    //     }
    // }
} 