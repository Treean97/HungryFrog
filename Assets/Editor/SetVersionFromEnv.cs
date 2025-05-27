using UnityEditor;

[InitializeOnLoad]
static class SetVersionFromEnv
{
    static SetVersionFromEnv()
    {
        // CI 가 설정해 줄 ENV
        var version = System.Environment.GetEnvironmentVariable("BUILD_VERSION");
        if (string.IsNullOrEmpty(version)) return;

        // Android
        PlayerSettings.bundleVersion = version;
        PlayerSettings.Android.bundleVersionCode = int.Parse(version.Replace(".", ""));

        // iOS
        PlayerSettings.iOS.buildNumber = version;
    }
}
