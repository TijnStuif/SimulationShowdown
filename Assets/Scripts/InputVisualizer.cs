using UnityEngine;
using UnityEngine.UIElements;

public class InputVisualizer : MonoBehaviour
{
    public UIDocument uiDocument;

    private VisualElement WKeyContainer;
    private VisualElement AKeyContainer;
    private VisualElement SKeyContainer;
    private VisualElement DKeyContainer;

    void Start()
    {
        // Load the UXML file
        var visualTree = uiDocument.rootVisualElement;

        // Find the key containers in the UXML file using their names
        WKeyContainer = visualTree.Q<VisualElement>("WKeyContainer");
        AKeyContainer = visualTree.Q<VisualElement>("AKeyContainer");
        SKeyContainer = visualTree.Q<VisualElement>("SKeyContainer");
        DKeyContainer = visualTree.Q<VisualElement>("DKeyContainer");

        // Debug logs to verify that the elements are found
        Debug.Log($"WKeyContainer: {WKeyContainer != null}");
        Debug.Log($"AKeyContainer: {AKeyContainer != null}");
        Debug.Log($"SKeyContainer: {SKeyContainer != null}");
        Debug.Log($"DKeyContainer: {DKeyContainer != null}");
    }

    void Update()
    {
        // Get the input values from the Input Manager
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Update the background color of each key based on the input values
        UpdateKey(WKeyContainer, vertical > 0);
        UpdateKey(AKeyContainer, horizontal < 0);
        UpdateKey(SKeyContainer, vertical < 0);
        UpdateKey(DKeyContainer, horizontal > 0);
    }

    private void UpdateKey(VisualElement keyContainer, bool isActive)
    {
        if (keyContainer == null)
        {
            Debug.LogWarning("Key container is null");
            return;
        }

        if (isActive)
        {
            keyContainer.style.backgroundColor = new StyleColor(Color.green);
        }
        else
        {
            keyContainer.style.backgroundColor = new StyleColor(Color.white);
        }
    }
}