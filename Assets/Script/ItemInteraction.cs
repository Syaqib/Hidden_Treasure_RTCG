using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class ItemInteraction : MonoBehaviour
{
    public string itemName; // Name of the item to display in the notification

    private bool isFound = false; // Flag to track if the item has been found

    private void OnMouseDown()
    {
        if (!isFound)
        {
            // Trigger notification when clicked if not already found
            string notificationText = itemName + " has been found";
            StartCoroutine(ShowNotification(notificationText, 3f));

            isFound = true; // Mark the item as found
        }
    }

    IEnumerator ShowNotification(string text, float duration)
    {
        // Show notification UI (replace with your actual UI display logic)
        Debug.Log(text);

        // Wait for duration
        yield return new WaitForSeconds(duration);

        // Hide or remove notification UI (replace with your actual UI removal logic)
        Debug.Log("Notification hidden");
    }
}
