using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Leak : MonoBehaviour
{
    [SerializeField] private GameObject raindropPrefab;
    [SerializeField] private List<Transform> instantiationPoints;

    public static Leak Instance { get; private set; }
    private void Awake() 
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void Start()
    {
        foreach (var i in instantiationPoints)
        {
            StartCoroutine(Instantiate(false, i.position));    
        }
    }

    public void DestroyAndNew(GameObject destroyDrop, Vector3 position)
    {
        Destroy(destroyDrop);
        StartCoroutine(Instantiate(true, position));
    }

    private IEnumerator Instantiate(bool staticTime, Vector3 instantiationPosition)
    {
        yield return new WaitForSeconds(staticTime ? 3f : Random.Range(1f, 3f));
        Instantiate(raindropPrefab, instantiationPosition, Quaternion.identity);
    }
}
