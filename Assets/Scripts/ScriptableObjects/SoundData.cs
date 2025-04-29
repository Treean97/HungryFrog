//using UnityEngine;

//[CreateAssetMenu(fileName = "SFXObjectData", menuName = "Scriptable Object/SFX Data")]
//public class SoundData : ScriptableObject
//{
//    public string _SoundName;
//    public AudioClip _Clip;
//    public float _Volume = 1f;
//}

using UnityEngine;

[CreateAssetMenu(fileName = "SFXObjectData", menuName = "Scriptable Object/SFX Data")]
public class SoundData : ScriptableObject
{
    public string _SoundName;
    public AudioClip _Clip;
    public float _Volume = 1f;
    public SoundCategory _Category; // 추가: 사운드 카테고리
}
