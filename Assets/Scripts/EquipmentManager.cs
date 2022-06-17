using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton

    public static EquipmentManager instance;

    void Awake () 
    {
        instance = this;
    }

    #endregion
    
    public Equipment[] defaultItems;
    public SkinnedMeshRenderer targetMesh;
    Equipment[] currentEquipment;
    SkinnedMeshRenderer[] currentMeshes;

    public delegate void OnEquipmentChanged (Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;
    Inventory inventory;

    void Start ()
    {
        inventory = Inventory.instance;

        //Creates a variable referencing the EquipmentSlot array.
        int numSlots = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        //Referncing the Equipment array of our character.
        currentEquipment = new Equipment[numSlots];
        currentMeshes = new SkinnedMeshRenderer[numSlots];

        EquipDefaultItems();
    }

    public void Equip (Equipment newItem)
    {
        //slotIndex is referencing the index of the newItem being equipped in the equipSlot array.
        int slotIndex = (int)newItem.equipSlot;
        Unequip(slotIndex);
        Equipment oldItem = Unequip(slotIndex);

        // An Item has been equipped so we trigger a callback
        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }

        SetEquipmentBlendShapes(newItem, 100);

        // Insert the item into the slot
        currentEquipment[slotIndex] = newItem;
        SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
        newMesh.transform.parent = targetMesh.transform;

        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;
        currentMeshes[slotIndex] = newMesh;
    }

    // Unequip an item with a particular index
    public Equipment Unequip (int slotIndex)
    {
        // Only do this if an item is there
        if (currentEquipment[slotIndex] != null)
        {
            if (currentMeshes[slotIndex] != null)
            {
                Destroy(currentMeshes[slotIndex].gameObject);
            }
            // Add the item to the inventory
            Equipment oldItem = currentEquipment[slotIndex];
            SetEquipmentBlendShapes(oldItem, 0);
            inventory.Add(oldItem);


            // Remove the item from the equipment array.
            currentEquipment[slotIndex] = null;

            // Equipment has been removed so we trigger a callback
            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
                
            }

            return oldItem;
        }
        return null;
    }

    public void UnequipAll () 
    {

        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
        // When you unequip all items, re-equip default items
        EquipDefaultItems();
    }

    void SetEquipmentBlendShapes(Equipment item, int weight)
    {
        foreach (EquipmentMeshRegion blendShape in item.coveredMeshRegions)
        {
            int shapeIndex = (int)blendShape;
            targetMesh.SetBlendShapeWeight(shapeIndex, weight);
        }
    }

    void EquipDefaultItems()
    {
        foreach (Equipment item in defaultItems)
        {
            Equip(item);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            UnequipAll();
        }
    }
}
