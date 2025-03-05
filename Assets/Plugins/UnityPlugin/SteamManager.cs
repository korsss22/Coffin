using UnityEngine;

public class SteamManager : MonoBehaviour
{
    public uint appId;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        try
        {
            Steamworks.SteamClient.Init(appId, true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Steam 초기화 실패: " + e.Message);
        }
    }

    private void OnApplicationQuit()
    {
        try {
            Steamworks.SteamClient.Shutdown();
        } catch(System.Exception e) {
            Debug.Log(e.Message);
        }
    }
}
