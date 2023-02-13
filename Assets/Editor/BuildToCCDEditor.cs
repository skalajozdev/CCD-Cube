using System.Collections;
using System.Collections.Generic;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public class BuildToCCDEditor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AddressableAssetSettings.BuildPlayerContent(out AddressablesPlayerBuildResult result);
        AddressableAssetSettings.BuildAndReleasePlayerContent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
