using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 遊戲機制
/// </summary>
public class GameMechanics : MonoBehaviour
{
    [SerializeField]
    [Header("遊戲時間")]
    private int g_GameTime;
    [SerializeField]
    [Header("時間UI")]
    private Transform g_TimeUI;

    [SerializeField]
    [Header("目標物")]
    private TargetSystem g_Target;

    [SerializeField]
    [Header("裝備系統")]
    private UnlockEquipmentSystem g_UnlockEquipmentSystem;

    [SerializeField]
    [Header("結算畫面")]
    private GameObject g_SettlementScreen;

    /// <summary>
    /// 遊戲是否進行
    /// </summary>
    private bool m_GameProgress;
    /// <summary>
    /// 計算中的遊戲時間
    /// </summary>
    private int m_GameTime;
    /// <summary>
    /// 時間條
    /// </summary>
    private Image m_TimeImage;
    /// <summary>
    /// 遊戲時間文字顯示
    /// </summary>
    private Text m_TimeText;

    /// <summary>
    /// 分數
    /// </summary>
    private int m_Fraction;

    private void Start()
    {
        //不需要重複抓取
        m_TimeImage = g_TimeUI.GetChild(0).GetComponent<Image>();
        m_TimeText = g_TimeUI.GetChild(2).GetComponent<Text>();
        //gameObject.SetActive(false);
        //
        
        Initialization();
        
        
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialization(){
        ContinueTheGame();
        g_SettlementScreen.SetActive(false);
        //g_UnlockEquipmentSystem.Reorganize();
        m_Fraction = 0;

        m_GameProgress = true;
        TimeManagement();
        g_Target.UpdateStatus();
    }

    /// <summary>
    /// 時間管理
    /// </summary>
    private void TimeManagement(){
        m_GameTime = g_GameTime;
        m_TimeText.text = "" + m_GameTime;
        StartCoroutine(Reciprocal());
    }
    /// <summary>
    /// 倒數計時
    /// </summary>
    private IEnumerator Reciprocal(){
        //Debug.Log("W");
        yield return new WaitForSeconds(1f);
        if(m_GameProgress && m_GameTime > 0){
            m_GameTime--;
            m_TimeImage.fillAmount = (float)m_GameTime / (float)g_GameTime;
            m_TimeText.text = "" + m_GameTime;
            IncreasedDifficulty();
            StartCoroutine(Reciprocal());
        }else if(m_GameTime <= 0){
            g_UnlockEquipmentSystem.UnlockEquipment(m_Fraction);
            g_SettlementScreen.SetActive(true);
            m_GameProgress = false;
            g_Target.GameSettlement();
            // TimePause();
        }
    }
    /// <summary>
    /// 目標物變更
    /// </summary>
    private void IncreasedDifficulty(){
        //難度增加
        if((g_GameTime - m_GameTime) % 50 == 0){
            g_Target.SetStatus();
        }

        //消失計時
        g_Target.DisappearingCountdown();
        
    }
    /// <summary>
    /// 變更分數
    /// </summary>
    /// <param name="_Score">增加的數值</param>
    public void ScoreChange(int _Score){
        //Debug.Log(m_Fraction + " + " + _Score);
        m_Fraction += _Score;
        if(m_Fraction < 0){
            m_Fraction = 0;
        }
    }
    /// <summary>
    /// 暫停
    /// </summary>
    public void TimePause(){
        
            Time.timeScale = 0f;
        
            
        
    }
    /// <summary>
    /// 繼續遊戲
    /// </summary>
    public void ContinueTheGame(){
        Time.timeScale = 1f;
    }
}
