using UnityEngine;
using System.Collections;

/// <summary>
/// UI跟隨目標物
/// </summary>
public class FollowWorldObj : MonoBehaviour {
    [SerializeField]
    GameObject worldPos;//3D物體（人物）
    [SerializeField]
    RectTransform rectTrans;//UI元素（如：血條等）
    public Vector2 offset;//偏移量

    // Update is called once per frame
    void Update () {
        Vector2 screenPos =Camera.main.WorldToScreenPoint(worldPos.transform.position);
        rectTrans.position = screenPos + offset;
    }
}