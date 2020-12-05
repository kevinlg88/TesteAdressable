using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetRefObjectData : MonoBehaviour
{
    public Image image;
    public Text text;

    public Text textID;
    public AssetReference assetref;

    private AsyncOperationHandle handle;
    private void Start () 
    {
        //Addressables.UpdateCatalogs(null, true);

        //Debug.Log("Asset GUID: " + assetref.AssetGUID);
        AsyncOperationHandle<List<string>> list = Addressables.CheckForCatalogUpdates(true);
        StartCoroutine(CheckUpdates(list));

    }

    IEnumerator CheckUpdates(AsyncOperationHandle<List<string>> upHandle)
    {

        while(!upHandle.IsDone)
        {
            textID.text = "Checking updates..." + upHandle.PercentComplete;
            image.color = Color.yellow;
            yield return null;
        }

        image.color = Color.green;
        foreach (var item in upHandle.Result)
        {
            textID.text +=", " + item;   
        }

        handle = Addressables.DownloadDependenciesAsync(assetref);
        StartCoroutine(DownloadAsset(handle));

    }

    IEnumerator DownloadAsset (AsyncOperationHandle handle)
    {
        while(!handle.IsDone)
        {
            text.text = handle.Status.ToString() + " - " + handle.PercentComplete;
            image.color = Color.yellow;
            yield return null;
        }
        image.color = Color.blue;
        handle =  Addressables.InstantiateAsync(assetref,new Vector3(0,0,0), Quaternion.identity);
        StartCoroutine(InstAsync(handle));
    }

    IEnumerator InstAsync (AsyncOperationHandle handle)
    {
        while(!handle.IsDone)
        {
            text.text = handle.Status.ToString() + " - " + handle.PercentComplete;
            image.color = Color.yellow;
            yield return null;
        }
        GameObject result = handle.Result as GameObject;
        text.text = "Result: " + result.name;
        if(result != null)
        {
            image.color = Color.green;
            text.text = "Name: " + result.name;
        }
        
        Caching.ClearCache();
        assetref.ReleaseAsset();
    }

}
