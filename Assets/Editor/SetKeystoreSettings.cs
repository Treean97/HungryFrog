using UnityEditor;

public static class SetKeystoreSettings
{
    [MenuItem("CI/ApplyKeystoreSettings")]
    public static void Apply()
    {
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = "user.keystore";
        PlayerSettings.Android.keystorePass = System.Environment.GetEnvironmentVariable("KEYSTORE_PASSWORD");
        PlayerSettings.Android.keyaliasName = System.Environment.GetEnvironmentVariable("KEY_ALIAS");
        PlayerSettings.Android.keyaliasPass = System.Environment.GetEnvironmentVariable("KEY_PASSWORD");
    }
}