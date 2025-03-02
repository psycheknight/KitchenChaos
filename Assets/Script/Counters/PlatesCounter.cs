using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpwaned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectsSO plateKitchenObjectSO;

    private float spwanPlateTimer;
    private float spwanPlateTimerMax = 4f;

    private int platesSpwanedAmount;
    private int platesSpwanedAmountMax = 4;

    private void Update()
    {
        spwanPlateTimer += Time.deltaTime;
        if (spwanPlateTimer > spwanPlateTimerMax)
        {
            spwanPlateTimer = 0f;

            if (KitchenGameManager.Instance.IsGamePlaying() && platesSpwanedAmount < platesSpwanedAmountMax)
            {
                platesSpwanedAmount++;

                OnPlateSpwaned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //Player is emppty handed 
            if (platesSpwanedAmount > 0)
            {
                // There's atleast one plate here
                platesSpwanedAmount--;
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);

                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}