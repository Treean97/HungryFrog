using UnityEditor;
using UnityEditor.Build.Reporting;
using System.Linq;

public class BuildScript
{
    public static void BuildAndroid()
    {
        // Keystore 설정 먼저 적용
        SetKeystoreSettings.Apply();

        // 씬 목록 가져오기
        string[] scenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();

        EditorUserBuildSettings.buildAppBundle = true;

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = "build/app.aab",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        var report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new System.Exception("Build failed: " + report.summary.result);
        }
    }   
}
