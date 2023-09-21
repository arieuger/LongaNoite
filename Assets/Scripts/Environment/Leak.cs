using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Leak : MonoBehaviour
{
    [SerializeField] private GameObject raindropPrefab;
    [SerializeField] private Transform instantiationPoint;

    public static Leak Instance { get; private set; }
    private void Awake() 
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void Start()
    {
        Instantiate();
    }

    public void DestroyAndNew(GameObject destroyDrop)
    {
        Destroy(destroyDrop);
        Invoke(nameof(Instantiate), 3);
    }

    private void Instantiate()
    {
        Instantiate(raindropPrefab, instantiationPoint.position, Quaternion.identity);
    }
}
