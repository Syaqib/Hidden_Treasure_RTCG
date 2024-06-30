using UnityEngine;
using TMPro;
using System.Collections;
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

    [Tooltip("The lose panel that will pop up if the player runs out of time.")]
    [SerializeField]
    private GameObject losePanel;

    [Tooltip("The TextMeshPro component to display the countdown timer.")]
    [SerializeField]
    private TextMeshProUGUI countdownText;

    [Tooltip("The starting time for the countdown in seconds.")]
    [SerializeField]
    private float startTime = 60f;

    [Tooltip("Distance from the camera to place the win/lose panel.")]
    [SerializeField]
    private float panelDistance = 2.0f;

    [Tooltip("Vertical offset for the win/lose panel.")]
    [SerializeField]
    private float verticalOffset = 0.5f;

    [Tooltip("Offset for the countdown text relative to the player's view.")]
    [SerializeField]
    private Vector3 countdownOffset = new Vector3(0, -0.5f, 2f);

    private float currentTime;
    private bool gameActive = true;

    private void Start()
    {
        winPanel.SetActive(false); // Ensure the win panel is initially hidden
        losePanel.SetActive(false); // Ensure the lose panel is initially hidden
        currentTime = startTime;
        StartCoroutine(CountdownTimer());
    }

    private void Update()
    {
        if (gameActive)
        {
            PositionCountdownText();
        }
    }

    private IEnumerator CountdownTimer()
    {
        while (gameActive && currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            countdownText.text = Mathf.Ceil(currentTime).ToString();
            yield return null;
        }

        if (currentTime <= 0 && gameActive)
        {
            ShowLosePanel();
        }
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
        gameActive = false; // Stop the timer
        winPanel.SetActive(true);

        // Position the win panel in front of the camera with offset
        PositionPanel(winPanel);
    }

    private void ShowLosePanel()
    {
        gameActive = false; // Stop the timer
        losePanel.SetActive(true);

        // Position the lose panel in front of the camera with offset
        PositionPanel(losePanel);
    }

    private void PositionPanel(GameObject panel)
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 panelPosition = cameraPosition + cameraForward * panelDistance;
        panelPosition.y += verticalOffset;
        panel.transform.position = panelPosition;
        panel.transform.rotation = Camera.main.transform.rotation;
    }

    private void PositionCountdownText()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 textPosition = cameraPosition + cameraForward * countdownOffset.z;
        textPosition.y += countdownOffset.y;
        textPosition.x += countdownOffset.x;
        countdownText.transform.position = textPosition;
        countdownText.transform.rotation = Camera.main.transform.rotation;
    }
}
