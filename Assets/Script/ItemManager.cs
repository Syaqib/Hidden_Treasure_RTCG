using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ItemManager : MonoBehaviour
{
    [Header("Items to Find UI")]
    [Tooltip("The TextMeshPro components to display the names of items to find.")]
    [SerializeField]
    private List<TextMeshProUGUI> itemTexts;

    [Tooltip("The win panel that will pop up when all items are found.")]
    [SerializeField]
    private GameObject winPanel;

    [Tooltip("Distance from the camera to place the win panel.")]
    [SerializeField]
    private float winPanelDistance = 1.0f;

    [Tooltip("Vertical offset for the win panel.")]
    [SerializeField]
    private float verticalOffset = 0.5f;

    private void Start()
    {
        winPanel.SetActive(false); // Ensure the win panel is initially hidden
    }

    public void ItemFound(TextMeshProUGUI itemText)
    {
        if (itemTexts.Contains(itemText))
        {
            itemTexts.Remove(itemText);
            itemText.gameObject.SetActive(false);

            if (itemTexts.Count == 0)
            {
                DisplayWin();
            }
        }
    }

    private void DisplayWin()
    {
        winPanel.SetActive(true);

        // Position the win panel in front of the camera with offset
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 panelPosition = cameraPosition + cameraForward * winPanelDistance;
        panelPosition.y += verticalOffset;
        winPanel.transform.position = panelPosition;
        winPanel.transform.rotation = Camera.main.transform.rotation;
    }
}
