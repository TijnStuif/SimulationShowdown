using UnityEngine;
using UnityEngine.UIElements;

public class InputVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject uiDocumentPrefab;
    private UIDocument uiDocument;
    private VisualElement WKeyContainer;
    private VisualElement AKeyContainer;
    private VisualElement SKeyContainer;
    private VisualElement DKeyContainer;
    private Player.Movement playerMovement;

    private void Awake()
    {
        var uiDocumentObj = Instantiate(uiDocumentPrefab);
        uiDocument = uiDocumentObj.GetComponent<UIDocument>();
    }

    void Start()
    {
        playerMovement = FindObjectOfType<Player.Movement>();
        var visualTree = uiDocument.rootVisualElement;

        WKeyContainer = visualTree.Q<VisualElement>("WKeyContainer");
        AKeyContainer = visualTree.Q<VisualElement>("AKeyContainer");
        SKeyContainer = visualTree.Q<VisualElement>("SKeyContainer");
        DKeyContainer = visualTree.Q<VisualElement>("DKeyContainer");
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

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
            if (playerMovement.areControlsInverted)
            {
                keyContainer.style.backgroundColor = new StyleColor(Color.magenta);
            }
            else
            {
            keyContainer.style.backgroundColor = new StyleColor(Color.white);
            }
        }
    }
}