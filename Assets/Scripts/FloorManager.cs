using UnityEngine;

public static class FloorManager
{
    // Current floor number (1, 2, or 3)
    private static int currentFloorNumber = 1;

    // Property to access the current floor number
    public static int CurrentFloorNumber
    {
        get { return currentFloorNumber; }
    }

    // Method to update the current floor number
    public static void SetCurrentFloor(int floorNumber)
    {
        if (floorNumber >= 1 && floorNumber <= 3)
        {
            currentFloorNumber = floorNumber;
            Debug.Log($"Player moved to Floor {floorNumber}");
        }
        else
        {
            Debug.LogError($"Invalid floor number: {floorNumber}. Must be 1, 2, or 3.");
        }
    }

    // Helper method to get floor number from a floor object's name
    // public static int GetFloorNumberFromName(string floorName)
    // {
    //     // Extract the number from names like "Floor1", "Floor2", "Floor3"
    //     if (floorName.StartsWith("Floor"))
    //     {
    //         string numberStr = floorName.Substring(5); // Skip "Floor"
    //         if (int.TryParse(numberStr, out int floorNumber))
    //         {
    //             return floorNumber;
    //         }
    //     }
    //     return -1; // Invalid floor name
    // }
} 