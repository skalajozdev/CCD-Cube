using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Core.Environments;
using UnityEditor;

public class RemoteConfig : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    private string badge;
    private string environment;
    private string bucketId;

    public bool devMode;

    async Task InitializeRemoteConfigAsync()
    {
        var options = new InitializationOptions();

        if (devMode)
            options.SetEnvironmentName("development");
        else
            options.SetEnvironmentName("production");

        // initialize handlers for unity game services
        await UnityServices.InitializeAsync(options);

        // remote config requires authentication for managing environment information
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    async Task Start()
    {
        // initialize Unity's authentication and core services, however check for internet connection
        // in order to fail gracefully without throwing exception if connection does not exist
        if (Utilities.CheckForInternetConnection())
        {
            await InitializeRemoteConfigAsync();
        }

        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        Debug.Log("RemoteConfigService.Instance.appConfig fetched: " + RemoteConfigService.Instance.appConfig.config.ToString());

        badge = RemoteConfigService.Instance.appConfig.GetString("badge");
        environment = RemoteConfigService.Instance.appConfig.GetString("environment");
        bucketId = RemoteConfigService.Instance.appConfig.GetString("bucketId");

        CcdManager.Badge = badge;
        CcdManager.EnvironmentName = environment;
        CcdManager.BucketId = bucketId;

        //Debug.Log("Init of CCD-M bucketId: " + CcdManager.BucketId + " env: " + CcdManager.EnvironmentName + " badge: " + CcdManager.Badge);
    }
}
