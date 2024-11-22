using UnityEngine;
using UnityEngine.UIElements;

public abstract class AbstractUiController : MonoBehaviour
{
    protected VisualElement Root { get; set; }

    public void Show() => Root.RemoveFromClassList("hidden");

    public void Hide() => Root.AddToClassList("hidden");
}
