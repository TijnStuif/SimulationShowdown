using UnityEngine;
using UnityEngine.UIElements;

public abstract class AbstractUiController : MonoBehaviour
{
        protected void PlayButtonSound() => AudioManager.Instance.PlaySFX(FindObjectOfType<AudioManager>().buttonSFX);
        protected VisualElement Root { get; set; }

        public void Show() => Root.RemoveFromClassList("hidden");   

        public void Hide() => Root.AddToClassList("hidden");
}
