using UnityEngine;
using UnityEngine.UIElements;

public abstract class AbstractUiController : MonoBehaviour
{
        protected void PlayButtonSound() => FindObjectOfType<AudioManager>().PlaySFX(FindObjectOfType<AudioManager>().buttonSFX);
        
        /// <summary>
        /// Required to be set to use Hide and Set methods
        /// </summary>
        protected VisualElement Root { get; set; }

        /// <summary>
        /// Requires Root property to be set!
        /// </summary>
        public void Show() => Root.RemoveFromClassList("hidden");   

        /// <summary>
        /// Requires Root property to be set!
        /// </summary>
        public void Hide() => Root.AddToClassList("hidden");
}
