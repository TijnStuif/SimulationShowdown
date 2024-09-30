using System.Collections.Generic;
using System.Threading.Tasks;
using Pickups;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class Inventory : MonoBehaviour
    {
        // I wanted to avoid making singletons, but a static class can't inherit so singleton it is
        public static Inventory Instance;
        // this is a temporary "solution" that should not be in dev and especially not main
        public List<Item> m_items = new List<Item>();
    
        // all labels in the guide were static
        private Label m_description;
    
        private bool m_frameToggle;
    
        private bool m_isReady;
    
        // all labels in the guide were static
        private Label m_itemName;
    
        private VisualElement m_root;

        private Information m_selectedInformation;
    
        private VisualElement m_tileArray;

        public void Add(Information info)
        {
            // create new item based on info
            var item = new Item
            {
                information = info
            };
            // add to List<Item>
            Add(item);
        }

        private void Add(Item item) => m_items.Add(item);
    
        public void Remove(Item item)
        {
            m_items.Remove(item);
        }

        // in the guide this was static
        // they put a static method in a singleton
        // I don't think that makes sense
        public void UpdateUiInfo(Information info)
        {
            // toggle system (if info already selected, deselect)
            if (info == m_selectedInformation)
            {
                m_itemName.text = string.Empty;
                m_description.text = string.Empty;
                m_selectedInformation = null;
            }
            else
            {
                m_itemName.text = info.itemName;
                m_description.text = info.description;
                m_selectedInformation = info;
            }

        }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                ReadyUp();
            }
            if (Instance != this)
            {
                Destroy(this); 
            }

        }
    
        private void Start() => LoadInventory();
    
        private void LateUpdate()
        {
            // super scuffed
            m_frameToggle = !m_frameToggle;
        }

        
        private async void LoadInventory()
        {
            Debug.Log("loading inventory");
            await Readiness();
        
            // supposed to be a lambda expression, but those are slow
            void InitItem(Item i, Visual v) { i.visual = v; v.RemoveFromClassList("no-display"); }

            for (int i = 0; i < m_items.Count; i++)
            {
                var visual = new Visual(m_items[i].information); 
                InitItem(m_items[i], visual);
                // to tile, add visual element
                m_tileArray.ElementAt(i).Add(visual);
            }
        }

        private async void ReadyUp()
        {
            // get root visual element of UIDocument in the scene
            m_root = GetComponentInChildren<UIDocument>().rootVisualElement;
        
            m_tileArray = m_root.Q<VisualElement>("TileArray");
            m_description = m_root.Q<Label>("Description");
            m_itemName = m_root.Q<Label>("ItemName");
        
            // I don't feel like installing UniTask
            // My alternative to UniTask.WaitForEndOfFrame:
            // (I don't know if this actually works like it should)
            await EndOfLateUpdate();

            m_isReady = true;
        }

        private Task EndOfLateUpdate()
        {
            var currentFrameState = m_frameToggle;
            while (currentFrameState != m_frameToggle)
            {
                // do nothing
            }
            return Task.CompletedTask;
        }
        
        private Task Readiness()
        {
            while (!m_isReady)
            {
                // do nothing
            }
            return Task.CompletedTask;
        }
    }
}
