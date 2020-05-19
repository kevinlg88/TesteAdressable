using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AssetRefObjectData : MonoBehaviour
{
    [SerializeField] private AssetReference _sqrAref;
    [SerializeField] private List<AssetReference> _references = new List<AssetReference>();


    [SerializeField] private List<GameObject> _completeObj = new List<GameObject>();

    private void Awake () 
    {
        _references.Add(_sqrAref);    

        StartCoroutine(LoadAndWaitUntilComplete());
    }

    private IEnumerator LoadAndWaitUntilComplete()
    {
       yield return AssetRefLoader.CreateAssetsAddToList(_references, _completeObj);
    }

}
