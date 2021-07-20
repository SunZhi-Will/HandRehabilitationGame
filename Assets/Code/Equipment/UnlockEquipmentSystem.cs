using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UnlockEquipmentSystem : MonoBehaviour
{
    [SerializeField]
    [Header("遊戲裝備")]
    private SpriteRenderer g_GameEquipment;
    [SerializeField]
    [Header("射擊子彈")]
    private SpriteRenderer g_ShootingBullets;

    [SerializeField]
    [Header("裝備小圖示")]
    private Image g_EquipmentIcons;

    [SerializeField]
    [Header("結算進度條")]
    private GameObject g_SettlementProgressBar;
    [SerializeField]
    [Header("結算小圖示")]
    private GameObject g_SettlementIcon;
    /*[SerializeField]
    [Header("結算未解鎖圖示")]
    private Sprite g_NotUnlockedIcon;*/
    [SerializeField]
    [Header("結算解鎖圖示")]
    private Sprite g_UnlockIcon;
    [SerializeField]
    [Header("結算解鎖藍色進度條")]
    private Image g_Schedule;

    [SerializeField]
    [Header("裝備音效")]
    private AudioSource g_Audio;

    /// <summary>
    /// 遊戲中的槍系統
    /// </summary>
    private GunSystem m_GunSystem; 

    /// <summary>
    /// 進度條比例
    /// </summary>
    private float m_Proportion;
    /// <summary>
    /// 最大分數
    /// </summary>
    private int m_max; 

    /// <summary>
    /// 結算小圖示背部
    /// </summary>
    private Image[] m_IconBack;
    
    /// <summary>
    /// 進度條改變大小
    /// </summary>
    private float m_ScheduleChangeValue;

    void Start()
    {
        m_GunSystem = g_GameEquipment.gameObject.GetComponent<GunSystem>();
        m_Proportion = g_SettlementProgressBar.GetComponent<RectTransform>().sizeDelta.x / 100; //設定結算進度條比例
        m_max = (int)((float)RecordSystem.m_EquipmentScore[RecordSystem.m_EquipmentScore.Length - 1] / 0.9); //取最高分 且設定擺放位置在九十百分比

        m_IconBack = new Image[RecordSystem.m_Icon.Length];

        EquippedWithWeapons();
        for (int i = 0; i < RecordSystem.m_Icon.Length; i++)
        {
            ResultSetting(i);
        }
    }
    /// <summary>
    /// 裝上裝備
    /// </summary>
    private void EquippedWithWeapons(){
        g_GameEquipment.sprite = RecordSystem.g_EquipmentModule.g_EquipmentDiagram;
        g_ShootingBullets.sprite = RecordSystem.g_EquipmentModule.g_ShootingBullets;
        g_EquipmentIcons.overrideSprite = RecordSystem.g_EquipmentModule.g_EquipmentIcons;
        g_Audio.clip = RecordSystem.g_EquipmentModule.g_ShootingSound; //裝備音效

        m_GunSystem.SetNowEquipped(RecordSystem.g_EquipmentModule);
    }

    GameObject _ECTs; //暫時儲存物件
    /// <summary>
    /// 結果裝備分數設定
    /// </summary>
    private void ResultSetting(int _i){
        
        _ECTs = Instantiate(g_SettlementIcon, g_SettlementIcon.transform.position, g_SettlementIcon.transform.rotation);
        _ECTs.transform.parent = g_SettlementProgressBar.transform;
        _ECTs.GetComponent<RectTransform>().anchoredPosition = g_SettlementIcon.GetComponent<RectTransform>().anchoredPosition + new Vector2((float)RecordSystem.m_EquipmentScore[_i] / (float)m_max * 100 * m_Proportion, 0);
        _ECTs.GetComponent<RectTransform>().localScale = g_SettlementIcon.GetComponent<RectTransform>().localScale;

        _ECTs.transform.GetChild(0).GetComponent<Image>().overrideSprite = RecordSystem.m_Icon[_i];
        _ECTs.transform.GetChild(1).GetComponent<Text>().text = RecordSystem.m_EquipmentScore[_i] + "";
        m_IconBack[_i] = _ECTs.GetComponent<Image>();

    }
    /// <summary>
    /// 進度條原比例
    /// </summary>
    float g_ScheduleValue;
    /// <summary>
    /// 結算解鎖裝備
    /// </summary>
    public void UnlockEquipment(int _Fraction){
        m_ScheduleChangeValue = (float)_Fraction / (float)m_max;
        g_ScheduleValue = (float)_Fraction / (float)m_max;
        g_Schedule.fillAmount = 0;
        StartCoroutine(Timing());
        Debug.Log(_Fraction);
        //g_Schedule.fillAmount = (float)_Fraction / (float)m_max;
        for(int i = 0; i < RecordSystem.m_EquipmentScore.Length; i++){
            if(RecordSystem.m_EquipmentScore[i] <= _Fraction){
                m_IconBack[i].overrideSprite = g_UnlockIcon; //解鎖圖案顯示
                if(PlayerPrefs.GetInt("HaveEquipment", 0) < i){
                    PlayerPrefs.SetInt("HaveEquipment", i);
                    //StoreUnlockedEquipment(i);
                }
            }
        }
    }
    /// <summary>
    /// 進度條滑動計時
    /// </summary>
    /// <returns></returns>
    private IEnumerator Timing(){
        
        yield return new WaitForSeconds(0.001f);
        g_Schedule.fillAmount += m_ScheduleChangeValue * Time.deltaTime;
        m_ScheduleChangeValue -= m_ScheduleChangeValue * Time.deltaTime;
        Debug.Log(m_ScheduleChangeValue);
        if(m_ScheduleChangeValue > 0.01f){
            StartCoroutine(Timing());
        }else{
            g_Schedule.fillAmount = g_ScheduleValue;
        }
    }
    /*
    /// <summary>
    /// 重新整理
    /// </summary>
    public void Reorganize(){
        for(int i = 0; i < g_EquipmentModule.Length; i++){
            m_IconBack[i].overrideSprite = g_NotUnlockedIcon; //解鎖圖案回復
        }
    }
    */
}
