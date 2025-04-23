using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectShootChance))]
public class ObjectShootChanceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ObjectShootChance tTarget = (ObjectShootChance)target;
        var tList = tTarget._ShootChances;

        EditorGUILayout.LabelField("Shoot Chances (합계 100%)", EditorStyles.boldLabel);
        int tCount = EditorGUILayout.IntField("오브젝트 수", tList.Count);

        // 리스트 추가 혹은 제거
        while (tCount > tList.Count)
            tList.Add(new ShootChanceInfo());

        while (tCount < tList.Count && tList.Count > 1)
            tList.RemoveAt(tList.Count - 1);

        // 합계
        float tTotal = 0f;

        // UI 표시
        for (int i = 0; i < tList.Count; i++)
        {
            var tInfo = tList[i];
            EditorGUILayout.Space(3);
            EditorGUILayout.LabelField($"Object {i}", EditorStyles.miniBoldLabel);

            tInfo.ShootObjectsData = (ShootObject)EditorGUILayout.ObjectField("Data", tInfo.ShootObjectsData, typeof(ShootObject), false);

            // 마지막 요소는 값을 정할 수 없고 100에 맞춰짐
            if (i == tList.Count - 1)
            {
                EditorGUILayout.LabelField("Chance (Auto)", $"{100f - tTotal:0.##}%");
                tInfo.Chance = Mathf.Max(0f, 100f - tTotal);
            }
            else
            {
                tInfo.Chance = EditorGUILayout.FloatField("Chance", tInfo.Chance);
                tTotal += tInfo.Chance;
            }
        }

        
        EditorGUILayout.Space(10);
        if (Mathf.Approximately(tTotal + tList[^1].Chance, 100f))
        {
            EditorGUILayout.LabelField("총합: 100.00%", EditorStyles.helpBox);
        }
        else
        {
            EditorGUILayout.HelpBox($"현재 총합: {tTotal + tList[^1].Chance:0.##}%. 마지막 항목은 자동 보정됩니다.", MessageType.Info);
        }

        // 변경 감지 후 저장
        if (GUI.changed)
        {
            EditorUtility.SetDirty(tTarget);
        }
    }
}
