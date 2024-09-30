using System.Collections.Generic;
using System.Linq;
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
        public List<Item> m_items = new List<Item>();
    
        private Label m_description;
    
        private bool m_frameToggle;
    
        private bool m_isReady;
    
        private Label m_itemName;
    
        private VisualElement m_root;
    
        private VisualElement m_tileArray;
    
        public int TileSize { get; set; }

        public void Add(Information info)
        {
            var item = new Item
            {
                information = info
            };
            Add(item);
        }

        private void Add(Item item) => m_items.Add(item);
    
        public void Remove(Item item)
        {
            m_items.Remove(item);
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


        private void InitTileSize()
        {
            // my ass is not using a Linq method to get the first element of an array
            VisualElement firstTile = m_tileArray.Children().ElementAt(0);
            TileSize = Mathf.RoundToInt(firstTile.worldBound.width);
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
        
            InitTileSize();

            m_isReady = true;
        }
    
        private async void LoadInventory()
        {
            Debug.Log("loading inventory");
            await Readyness();
        
            // supposed to be a lambda expression, but those are slow
            void InitItem(Item i, Visual v) { i.visual = v; v.RemoveFromClassList("no-display"); }

            for (int i = 0; i < m_items.Count; i++)
            {
                Debug.Log("adding to tile array");
                var visual = new Visual(m_items[i].information); 
                InitItem(m_items[i], visual);
                // to tile, add visual element
                m_tileArray.ElementAt(i).Add(visual);
            }
        }
    
        private Task Readyness()
        {
            while (!m_isReady)
            {
                // do nothing
            }
            return Task.CompletedTask;
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
    }
}
