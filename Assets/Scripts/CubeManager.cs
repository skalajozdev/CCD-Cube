using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEditor;
using System.Linq;
using Sirenix.OdinInspector;

public class CubeManager : MonoBehaviour
{
    [Required]
    public GameObject button;

    private AsyncOperationHandle m_CubeLoadingHandle;

    bool clicked;

    public void OnCubeClick()
    {
        if (!button.activeSelf)
            return;

        clicked = true;
        
        m_CubeLoadingHandle = Addressables.InstantiateAsync("Cube", transform, false);
        Debug.Log("Env: " + CcdManager.EnvironmentName + " BucketID: " + CcdManager.BucketId + " Badge: " + CcdManager.Badge);

        m_CubeLoadingHandle.Completed += OnCubeInstantiated;
        button.SetActive(false);
    }

    private void OnCubeInstantiated(AsyncOperationHandle obj)
    {
        // We can check for the status of the InstantiationAsync operation: Failed, Succeeded or None
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Asset instantiated successfully");
            var opHandle = Addressables.LoadResourceLocationsAsync("Cube");
        }
    }

    private void OnDisable()
    {
        if (!clicked)
            return;

        m_CubeLoadingHandle.Completed -= OnCubeInstantiated;
        Addressables.Release(m_CubeLoadingHandle);
        Debug.Log("Asset released");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
