using Player.V2;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class InputVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject uiDocumentPrefab;
    private UIDocument uiDocument;
    private VisualElement WKeyContainer;
    private VisualElement AKeyContainer;
    private VisualElement SKeyContainer;
    private VisualElement DKeyContainer;
    private VisualElement DashkeyContainer;
    private VisualElement DashKeyOverlay;
    private Player.V2.Movement playerMovement;
    private Player.V2.Teleport playerTeleport;

    private void Awake()
    {
        var uiDocumentObj = Instantiate(uiDocumentPrefab);
        uiDocument = uiDocumentObj.GetComponent<UIDocument>();
    }

    void Start()
    {
        playerMovement = FindObjectOfType<Movement>();
        if (playerMovement == null)
            throw new StateController.ScriptNotFoundException(nameof(playerMovement));
        playerTeleport = FindObjectOfType<Player.V2.Teleport>();
        var visualTree = uiDocument.rootVisualElement;

        WKeyContainer = visualTree.Q<VisualElement>("WKeyContainer");
        AKeyContainer = visualTree.Q<VisualElement>("AKeyContainer");
        SKeyContainer = visualTree.Q<VisualElement>("SKeyContainer");
        DKeyContainer = visualTree.Q<VisualElement>("DKeyContainer");
        DashkeyContainer = visualTree.Q<VisualElement>("DashKeyContainer");
        DashKeyOverlay = visualTree.Q<VisualElement>("DashKeyOverlay");

        playerTeleport.Teleported += HandleDash;
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

    private void HandleDash()
    {
        StartCoroutine(FadeDashKeyContainerColor());
    }

    private IEnumerator FadeDashKeyContainerColor()
    {
        float duration = playerTeleport.Cooldown;
        float elapsedTime = 0f;

        DashKeyOverlay.style.width = new Length(100, LengthUnit.Percent);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            DashKeyOverlay.style.width = new Length((1 - t) * 100, LengthUnit.Percent);
            yield return null;
        }

        DashKeyOverlay.style.width = new Length(0, LengthUnit.Percent);
    }
}