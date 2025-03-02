using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{


    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;
    public static DeliveryManager Instance { get; private set; }



    [SerializeField] private RecipeListSO recipeListSO;
    private List<RecipeSO> waitingRecipeSOList;

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipeMax = 4;
    private int successfulRecipesAmount;

    private void Awake()
    {
        Instance = this;
        waitingRecipeSOList = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            if (KitchenGameManager.Instance.IsGamePlaying() && waitingRecipeSOList.Count < waitingRecipeMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];

                waitingRecipeSOList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DevilerRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
            if (waitingRecipeSO.kitchenObjectsSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // Has the same number of ingredients

                bool plateContentMatchesRecipe = true;
                foreach (KitchenObjectsSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectsSOList)
                {
                    //Cycling through all the ingredients in the Recipe 
                    bool ingredientFound = false;
                    foreach (KitchenObjectsSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        //Cycling through all the ingredients in the Plate 

                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            // Ingredient Match
                            ingredientFound = true;
                            break;
                        }
                    }
                    // This Cycling process ends here another code start compling through here
                    if (!ingredientFound)
                    {
                        // This Recipe Ingredient was not found on the Plate
                        plateContentMatchesRecipe = true;
                    }
                }

                if (plateContentMatchesRecipe)
                {
                    // Player Deliverd the correct Recipe
                    //Debug.Log("Player delivered the correct recipe!");

                    successfulRecipesAmount++;

                    waitingRecipeSOList.RemoveAt(i);

                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }

        // No Matches Found
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        // Player did not deiver a correct recipe
        Debug.Log("Player did not deliver a correct recipe");
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
