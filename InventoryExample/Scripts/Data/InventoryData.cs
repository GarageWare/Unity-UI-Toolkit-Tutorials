using System;
using System.Collections.Generic;
using UnityEngine;

namespace GWG.Tutorials.Inventory
{
    [CreateAssetMenu(fileName = "InventoryDataExample", menuName = "Garage-Ware Games/Tutorials/New Inventory Data Example File", order = 1)]
    public class InventoryData : ScriptableObject
    {
        [SerializeField]
        public int coins;
        public List<InventoryItem> items;

        public void AddItem(InventoryItem item)
        {
            items.Add(item);
        }
        public void RemoveItem(InventoryItem item)
        {
            items.Remove(item);
        }
        public void Clear()
        {
            items.Clear();
        }
        public void AddQuantity(InventoryItem item, int quantity)
        {
            item.itemCount += quantity;
        }
        public void RemoveQuantity(InventoryItem item, int quantity)
        {
            item.itemCount -= quantity;
        }
        public void SetQuantity(InventoryItem item, int quantity)
        {
            item.itemCount = quantity;
        }
        public int GetQuantity(InventoryItem item)
        {
            return item.itemCount;
        }
        public bool HasItem(InventoryItem item)
        {
            return item.itemCount > 0;
        }
    }

    [Serializable]
    public class InventoryItem
    {
        public string itemName;
        public Sprite itemIcon;
        public int itemCount;
        public string itemDescription;
        public int itemValue;
    }
}