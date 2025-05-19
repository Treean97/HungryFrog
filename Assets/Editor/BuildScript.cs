using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;

public class BuildScript
{
    public static void BuildAndroid()
    {
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

        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != BuildResult.Succeeded)
        {
            throw new System.Exception("Build failed: " + report.summary.result);
        }
    }
}
