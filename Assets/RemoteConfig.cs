using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Core.Environments;
using UnityEditor;
using Sirenix.OdinInspector;

public class RemoteConfig : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    [BoxGroup("Environment Info")]
    [EnumToggleButtons]
    public EnvironmentState environment;
    
    private string badgeString;
    private string bucketIdString;
    private string environmentString;
    
    async Task InitializeRemoteConfigAsync()
    {
        var options = new InitializationOptions();
        
        // set environment
        SetEnvironment(options);

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

        badgeString = RemoteConfigService.Instance.appConfig.GetString("badge");
        bucketIdString = RemoteConfigService.Instance.appConfig.GetString("bucketId");

        CcdManager.BucketId = bucketIdString;
        CcdManager.Badge = badgeString;
        CcdManager.EnvironmentName = environmentString;
    }
    
    private void SetEnvironment(InitializationOptions options)
    {
        switch (environment)
        {
            case EnvironmentState.development:
                environmentString = "development";
                options.SetEnvironmentName(environment.ToString());
                break;
            case EnvironmentState.production:
                environmentString = "production";
                options.SetEnvironmentName(environment.ToString());
                break;
            default:
                options.SetEnvironmentName("development");
                environmentString = "development";
                break;
        }
    }
}


public enum EnvironmentState
{
    development,
    production
}
