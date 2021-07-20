using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 槍系統
/// </summary>
public class GunSystem : MonoBehaviour
{
    [SerializeField]
    [Header("目標物")]
    private TargetSystem g_Target;

    [SerializeField]
    [Header("氣力條")]
    private RectTransform g_StrengthBar;
    [SerializeField]
    [Header("音效來源")]
    private AudioSource g_Audio;

    [SerializeField]
    [Header("按鍵上升百分比")]
    private float g_PercentageOfKeyUp = 15;
    [SerializeField]
    [Header("按鍵下降百分比")]
    private float g_KeyDownPercentage = -20;
    [SerializeField]
    [Header("自然下降")]
    private float g_NaturalDecline = -10;

    /// <summary>
    /// 目前氣力
    /// </summary>
    private float m_CurrentStrength;
    /// <summary>
    /// 最大氣力
    /// </summary>
    private float m_MaximumStrength = 100;
    /// <summary>
    /// 現在裝備的裝備模組
    /// </summary>
    private EquipmentModule m_NowEquipped;
    /// <summary>
    /// 射速
    /// </summary>
    private float m_RateOfFire;
    /// <summary>
    /// 累計射擊時間
    /// </summary>
    private float m_CumulativeShootingTime;
    /// <summary>
    /// 子彈效果
    /// </summary>
    private GameObject m_BulletEffect;


    /// <summary>
    /// 設定遊戲中的裝備模組
    /// </summary>
    /// <param name="_NowEquipped">裝備模組</param>
    public void SetNowEquipped(EquipmentModule _NowEquipped){
        m_NowEquipped = _NowEquipped;
    }

    private void Start()
    {
        m_BulletEffect = this.transform.GetChild(0).gameObject;
        m_BulletEffect.SetActive(false);
        m_CurrentStrength = 1;
        m_RateOfFire = RecordSystem.g_EquipmentModule.g_ShotSeconds / (float)RecordSystem.g_EquipmentModule.g_Shots;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(m_CurrentStrength);
        if(g_Target.GameProgress()){
            ButtonControl();
            FullEnergy();
        }else if(m_CurrentStrength != 0){
            m_CurrentStrength = 0;
        }
        

        
    }
    /// <summary>
    /// 按鍵控制
    /// </summary>
    private void ButtonControl(){

        if(Input.GetKey(KeyCode.UpArrow)){
            GasGathering(g_PercentageOfKeyUp);
        }else if(Input.GetKey(KeyCode.DownArrow)){
            CutBackGathering(g_KeyDownPercentage);
        }else{
            CutBackGathering(g_NaturalDecline);
        }
        
    }
    /// <summary>
    /// 集滿能量
    /// </summary>
    private void FullEnergy(){
        if(m_CurrentStrength >= m_MaximumStrength - 5){ //集滿能量 發射
            m_CumulativeShootingTime += Time.deltaTime;
            if(m_RateOfFire < m_CumulativeShootingTime){
                m_BulletEffect.SetActive(true);
                g_Audio.Play();
                m_CumulativeShootingTime = 0;
                g_Target.BeingAttacked(RecordSystem.g_EquipmentModule.g_AttackPower);
            }
        }else{
            m_BulletEffect.SetActive(false);
            m_CumulativeShootingTime = 0;
        }
    }
    /// <summary>
    /// 累積氣力
    /// </summary>
    private void GasGathering(float _Value){
        if(m_CurrentStrength < m_MaximumStrength){
            IncreaseOrDecrease(_Value);
        }else{
            m_CurrentStrength = m_MaximumStrength;
        }
    }
    /// <summary>
    /// 減少氣力
    /// </summary>
    private void CutBackGathering(float _Value){
        if(m_CurrentStrength > 0){
            IncreaseOrDecrease(_Value);
        }else{
            m_CurrentStrength = 0;
        }
    }
    /// <summary>
    /// 氣力條的增減
    /// </summary>
    /// <param name="_Value">增加或減少的氣力</param>
    private void IncreaseOrDecrease(float _Value){
        
        m_CurrentStrength += _Value * Time.deltaTime;
        g_StrengthBar.sizeDelta = new Vector2 (g_StrengthBar.sizeDelta.x, m_CurrentStrength * 6); //氣力條 高度最高600 / 100(最大氣力) = 6
        
    }
}
