using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerFocusScript : MonoBehaviour
{
    public int focus = 100;
    public int maxFocus = 100;
    public float focusLossRate = 3f;
    public int focusGainOnSuccess = 10;
    public int focusLossOnFail = 20;
    public int minimumtime = 5;
    public int maximumtime = 15;
    public KeyCode[] inputRequestKeys = { KeyCode.Space, KeyCode.H, KeyCode.K, KeyCode.V, KeyCode.M };
    public float inputRequestTime = 2f;
    public UIDocument uiDocument;
    private bool isSneaking = false;
    private bool inputRequestActive = false;
    private float inputRequestTimer = 0f;
    private KeyCode currentInputRequestKey;
    private PlayerMovementScript playerMovementScript;
    private Label focusLabel;
    private Label inputRequestLabel;
    private ProgressBar timingBar;
    private VisualElement targetRangeIndicator;
    private VisualElement timingBarContainer;

    private float targetStart;
    private float targetEnd;
    private float nextInputRequestTime;

    void Start()
    {
        playerMovementScript = GetComponent<PlayerMovementScript>();

        if (uiDocument == null)
        {
            Debug.LogError("UIDocument is not assigned.");
            return;
        }

        var root = uiDocument.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("Root Visual Element not found in UIDocument.");
            return;
        }

        focusLabel = root.Q<Label>("FocusLabel");
        inputRequestLabel = root.Q<Label>("InputRequestLabel");
        timingBar = root.Q<ProgressBar>("TimingBar");
        targetRangeIndicator = root.Q<VisualElement>("TargetRangeIndicator");
        timingBarContainer = root.Q<VisualElement>("TimingBarContainer");

        if (focusLabel == null)
        {
            Debug.LogError("FocusLabel not found in the UI Document.");
            return;
        }

        if (inputRequestLabel == null)
        {
            Debug.LogError("InputRequestLabel not found in the UI Document.");
            return;
        }

        if (timingBar == null)
        {
            Debug.LogError("TimingBar not found in the UI Document.");
            return;
        }

        if (targetRangeIndicator == null)
        {
            Debug.LogError("TargetRangeIndicator not found in the UI Document.");
            return;
        }

        if (timingBarContainer == null)
        {
            Debug.LogError("TimingBarContainer not found in the UI Document.");
            return;
        }

        UpdateFocusUI();

        timingBarContainer.style.display = DisplayStyle.None;
        inputRequestLabel.style.display = DisplayStyle.None;
        timingBar.style.display = DisplayStyle.None;
        targetRangeIndicator.style.display = DisplayStyle.None;

        SetNextInputRequestTime();
    }

    void Update()
    {
        if (playerMovementScript != null)
        {
            if (playerMovementScript.moveInput != Vector2.zero && !isSneaking)
            {
                float focusLoss = focusLossRate * playerMovementScript.currentSpeed * Time.deltaTime;
                focus -= Mathf.RoundToInt(focusLoss);
                focus = Mathf.Clamp(focus, 0, maxFocus);
                //Debug.Log($"Focus decreased by {focusLoss}, new focus: {focus}");
                UpdateFocusUI();
            }

            if (isSneaking)
            {
                if (!inputRequestActive && Time.time >= nextInputRequestTime)
                {
                    StartInputRequest();
                }
                else if (inputRequestActive)
                {
                    CheckInputRequest();
                }
            }
        }
    }

    public void OnSneak(InputValue context)
    {
        isSneaking = context.isPressed;
        if (!isSneaking)
        {
            if (inputRequestActive)
            {
                focus -= focusLossOnFail;
                focus = Mathf.Clamp(focus, 0, maxFocus);
                inputRequestActive = false;
                UpdateFocusUI();
            }

            timingBarContainer.style.display = DisplayStyle.None;
            inputRequestLabel.style.display = DisplayStyle.None;
            timingBar.style.display = DisplayStyle.None;
            targetRangeIndicator.style.display = DisplayStyle.None;
        }
    }

    private void StartInputRequest()
    {
        inputRequestActive = true;
        inputRequestTimer = 0f;
        timingBarContainer.style.display = DisplayStyle.Flex;
        currentInputRequestKey = inputRequestKeys[Random.Range(0, inputRequestKeys.Length)];
        inputRequestLabel.text = $"Press {currentInputRequestKey}";
        inputRequestLabel.style.display = DisplayStyle.Flex;
        timingBar.style.display = DisplayStyle.Flex;
        timingBar.value = 0f;
        targetRangeIndicator.style.display = DisplayStyle.Flex;

        float targetRangeWidth = Random.Range(5f, 30f);
        targetStart = Random.Range(10f, 100f - targetRangeWidth);
        targetEnd = targetStart + targetRangeWidth;

        StartCoroutine(UpdateTargetRangeIndicator());
    }

    private void CheckInputRequest()
    {
        inputRequestTimer += Time.deltaTime;
        timingBar.value = (inputRequestTimer / inputRequestTime) * 100f;

        if (Input.GetKeyDown(currentInputRequestKey))
        {
            if (timingBar.value >= targetStart && timingBar.value <= targetEnd)
            {
                focus += focusGainOnSuccess;
            }
            else
            {
                focus -= focusLossOnFail;
            }
            focus = Mathf.Clamp(focus, 0, maxFocus);
            inputRequestActive = false;
            timingBarContainer.style.display = DisplayStyle.None;
            inputRequestLabel.style.display = DisplayStyle.None;
            timingBar.style.display = DisplayStyle.None;
            targetRangeIndicator.style.display = DisplayStyle.None;
            UpdateFocusUI();
            SetNextInputRequestTime();
        }
        else if (inputRequestTimer >= inputRequestTime)
        {
            focus -= focusLossOnFail;
            focus = Mathf.Clamp(focus, 0, maxFocus);
            inputRequestActive = false;
            timingBarContainer.style.display = DisplayStyle.None;
            inputRequestLabel.style.display = DisplayStyle.None;
            timingBar.style.display = DisplayStyle.None;
            targetRangeIndicator.style.display = DisplayStyle.None;
            UpdateFocusUI();
            SetNextInputRequestTime();
        }
    }

    private IEnumerator UpdateTargetRangeIndicator()
    {
        yield return new WaitForEndOfFrame();

        float barWidth = timingBar.resolvedStyle.width;
        float indicatorWidth = (targetEnd - targetStart) / 100f * barWidth;

        Debug.Log($"Target range start: {targetStart}%, end: {targetEnd}%, bar width: {barWidth}, indicator width: {indicatorWidth}");

        targetRangeIndicator.style.left = new StyleLength(new Length(targetStart, LengthUnit.Percent));
        targetRangeIndicator.style.width = indicatorWidth;
    }

    private void UpdateFocusUI()
    {
        focusLabel.text = $"Focus: {focus}";
    }

    private void SetNextInputRequestTime()
    {
        
        nextInputRequestTime = Time.time + Random.Range(minimumtime, maximumtime); 
        Debug.Log($"Next input request time: {nextInputRequestTime - Time.time}");
    }
}