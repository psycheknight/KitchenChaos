using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter platesCounter;
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform platePrefabVisual;

    private List<GameObject> plateVisualGameObjectList;

    private void Awake()
    {
        plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpwaned += PlatesCounter_OnPlateSpwaned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, System.EventArgs e)
    {
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        plateVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void PlatesCounter_OnPlateSpwaned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(platePrefabVisual, counterTopPoint);

        float plateoffset = .1f;
        plateVisualTransform.localPosition = new Vector3 (0, plateoffset * plateVisualGameObjectList.Count , 0);

        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);

    }

}