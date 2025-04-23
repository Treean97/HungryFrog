using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneNextObjectsUI : MonoBehaviour
{
    [SerializeField]
    public List<Image> _SlotImages; // UI ���Կ� ǥ���� Text ����Ʈ

    public void UpdateQueueDisplay(Queue<ShootChanceInfo> tQueue)
    {
        ShootChanceInfo[] tArray = tQueue.ToArray();

        for (int i = 0; i < _SlotImages.Count; i++)
        {
            _SlotImages[i].sprite = tArray[i].ShootObjectsData.GetShootObjectSpriteOnUI;
        }

        
    }
}
