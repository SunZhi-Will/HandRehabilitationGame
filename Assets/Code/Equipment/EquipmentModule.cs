using System;
using UnityEngine;

/// <summary>
/// 裝備模組
/// </summary>
[Serializable]
public class EquipmentModule
{
    [SerializeField]
    [Header("裝備名稱")]
    public string g_Name;
    [SerializeField]
    [Header("未獲得裝備卡圖示")]
    public Sprite g_NotEquipped;
    [SerializeField]
    [Header("已獲得裝備卡圖示")]
    public Sprite g_GetEquipped;
    [SerializeField]
    [Header("裝備圖")]
    public Sprite g_EquipmentDiagram;
    [SerializeField]
    [Header("裝備小圖示")]
    public Sprite g_EquipmentIcons;
    [SerializeField]
    [Header("裝備分數")]
    public int g_EquipmentScore;

    [SerializeField]
    [Header("射擊子彈")]
    public Sprite g_ShootingBullets;

    [SerializeField]
    [Header("攻擊力")]
    public int g_AttackPower;
    [SerializeField]
    [Header("射擊次數")]
    public int g_Shots;
    [SerializeField]
    [Header("射擊秒數")]
    public float g_ShotSeconds;

    [SerializeField]
    [Header("射擊音效")]
    public AudioClip g_ShootingSound;

    void Start()
    {
        
    }

}
