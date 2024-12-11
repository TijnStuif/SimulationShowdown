using Player.V2;
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
    private Player.V2.Movement playerMovement;
    private Player.V1.Movement m_compatMovementV1;

    private void Awake()
    {
        var uiDocumentObj = Instantiate(uiDocumentPrefab);
        uiDocument = uiDocumentObj.GetComponent<UIDocument>();
    }

    void Start()
    {
        #if DEBUG
        if (Compatibility.IsV1)
        {
            m_compatMovementV1 = FindObjectOfType<Player.V1.Movement>();
        }
        #endif
        if (playerMovement == null)
            playerMovement = FindObjectOfType<Movement>();
        if (playerMovement == null && m_compatMovementV1 == null)
            throw new StateController.ScriptNotFoundException(nameof(playerMovement));
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
            return;
        }

        if (isActive)
        {
            keyContainer.style.backgroundColor = new StyleColor(Color.green);
        }
        else
        {
            #if DEBUG
            if (Compatibility.IsV1)
            {
                if (m_compatMovementV1.areControlsInverted)
                    keyContainer.style.backgroundColor = new StyleColor(Color.magenta);
                else
                    keyContainer.style.backgroundColor = new StyleColor(Color.white);
                return;
            }
            #endif
            if (playerMovement.AreControlsInverted)
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