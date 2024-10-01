using System.Collections.Generic;
using UnityEditor;
using UnityEditor.iOS;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : MonoBehaviour
{
    private bool m_resourcesLoaded;
    // singleton pattern
    public static ItemManager Instance;

    private readonly List<string> m_assetNames =
        new ()
        {
            "Pickups/BattleMusic",
            "Pickups/PeanutAllergy",
            "Pickups/GamblingAddict"
        };

    private readonly List<Pickups.Information> m_pickupInformation = new List<Pickups.Information>();

    private GameObject m_itemPrefab;
    

    private void Awake()
    {
        // if there's no instance set, set it to the one which just got created
        if (Instance == null)
            Instance = this;

        // if there's an instance made which is not the same as the original one, destroy it, so no other instance can
        // exist
        // On one hand, Destroy() does not destroy the object immediately, on the other hand, it's called in Awake()
        // so it can probably destroy the rogue instance before anything is happening
        if (Instance != this)
            Destroy(this);
        
        if (!m_resourcesLoaded)
            LoadResources();
    }

    // return reference to a random asset
    public Pickups.Information GetRandomPickupInformation()
    {
        int iRandom = Random.Range(0, m_assetNames.Count);
        string assetName = m_assetNames[iRandom];
        
        // create instance of the pickup info
        // Pickups.Information asset = ScriptableObject.CreateInstance<Pickups.Information>();
        // set info the an actual (random) asset)
        return m_pickupInformation[iRandom];
    }
    
    /// <summary>
    /// return inactive ItemDrop object which random pickup information (via ItemDrop.DataHolder.Information)
    /// </summary>
    /// <returns></returns>
    public GameObject GetRandomItemDrop()
    {
        return GetItemDropFrom(GetRandomPickupInformation());
    }
    /// <summary>
    /// return inactive ItemDrop object with specified pickup information (via ItemDrop.DataHolder.Information)
    /// </summary>
    /// <returns></returns>
    public GameObject GetItemDropFrom(Pickups.Information info)
    {
        if (!m_resourcesLoaded)
            LoadResources();
        GameObject drop = Instantiate(m_itemPrefab);
        drop.SetActive(false);
        Pickups.ItemDrop.DataHolder dataHolder = drop.GetComponent<Pickups.ItemDrop.DataHolder>();
        dataHolder.Information = info;
        return drop;
    }

    private void LoadResources()
    {
        m_itemPrefab = Resources.Load("Pickups/ItemDrop", typeof(GameObject)) as GameObject;
        foreach (var assetName in m_assetNames)
        {
            var i = Resources.Load(assetName);
            var inf = Resources.Load<Pickups.Information>(assetName);
            var info = Instantiate(Resources.Load<Pickups.Information>(assetName));
            m_pickupInformation.Add(info); 
        }

        m_resourcesLoaded = true;
    }
}
