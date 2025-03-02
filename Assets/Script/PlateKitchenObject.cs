using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectsSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectsSO> validKitchenObjectSOList;

    private List<KitchenObjectsSO> kitchenObjectsSOList;

    private void Awake()
    {
        kitchenObjectsSOList = new List<KitchenObjectsSO>();
    }
    public bool TryAddIngredient(KitchenObjectsSO kitchenObjectsSO)
    {
        if (!validKitchenObjectSOList.Contains(kitchenObjectsSO))
        {
            //Not a valid ingredient
            return false;
        }

        if (kitchenObjectsSOList.Contains(kitchenObjectsSO))
        {
            // Already has this type
            return false;
        }
        else
        {
            kitchenObjectsSOList.Add(kitchenObjectsSO);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectsSO
            });
            return true;
        }
    }

    public List<KitchenObjectsSO> GetKitchenObjectSOList()
    {
        return kitchenObjectsSOList;
    }
}
