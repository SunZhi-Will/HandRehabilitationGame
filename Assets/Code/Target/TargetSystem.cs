using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 目標物系統
/// </summary>
public class TargetSystem : MonoBehaviour
{
    [SerializeField]
    [Header("遊戲機制")]
    private GameMechanics g_GameMechanics;
    [SerializeField]
    [Header("生命UI")]
    private Image g_HPUI;
    [SerializeField]
    [Header("等級UI")]
    private GameObject g_TargetUI;

    [SerializeField]
    [Header("炸彈率")]
    private int g_BombRate;
    [SerializeField]
    [Header("炸彈持續時間")]
    private int g_BombDuration;

    [SerializeField]
    [Header("目標物")]
    private Sprite[] g_Target;
    
    [SerializeField]
    [Header("生命等級")]
    private int[] g_LifeLevel;
    [SerializeField]
    [Header("分數等級")]
    private int[] g_ScoreGrade;

    [SerializeField]
    [Header("爆炸效果")]
    private Sprite[] g_ExplosionEffect;
    [SerializeField]
    [Header("爆炸音效")]
    private AudioClip[] g_ExplosionAudioClip;

    /// <summary>
    /// 是否初始化
    /// </summary>
    private bool m_Start = false;
    /// <summary>
    /// 遊戲是否進行
    /// </summary>
    private bool m_GameProgress;
    /// <summary>
    /// 等級Text
    /// </summary>
    private Text m_TargetLevel;
    /// <summary>
    /// 變換目標圖示
    /// </summary>
    private SpriteRenderer m_ChangeTargetIcon;
    /// <summary>
    /// 目標物等級
    /// </summary>
    private int m_Level;
    /// <summary>
    /// 等級範圍
    /// </summary>
    private int m_GradeRange;
    /// <summary>
    /// 生命最大值
    /// </summary>
    private int m_MaxHP;
    /// <summary>
    /// 現在生命值
    /// </summary>
    private int m_HP;
    /// <summary>
    /// 消失秒數
    /// </summary>
    private float m_SecondsToDisappear;
    /// <summary>
    /// 原血量百分比
    /// </summary>
    private float m_OriginalBVP;
    /// <summary>
    /// 變動後的血量百分比
    /// </summary>
    private float m_CTPOfHP;
    /// <summary>
    /// 目標物動畫
    /// </summary>
    private Animator m_Animator;

    /// <summary>
    /// 爆炸物件
    /// </summary>
    private GameObject m_Explosive;
    /// <summary>
    /// 爆炸物圖片
    /// </summary>
    private SpriteRenderer m_ExplosiveSpriteRenderer;

    /// <summary>
    /// 音效來源
    /// </summary>
    private AudioSource m_Audio;
    private void Start()
    {
        m_Audio = GetComponent<AudioSource>();
        m_TargetLevel = g_TargetUI.transform.GetChild(0).GetComponent<Text>();
        m_Explosive = transform.GetChild(1).gameObject;
        m_ExplosiveSpriteRenderer = m_Explosive.GetComponent<SpriteRenderer>();
        m_ChangeTargetIcon = gameObject.GetComponent<SpriteRenderer>();
        m_Animator = gameObject.GetComponent<Animator>();
        
        m_Start = true;
    }

    // Update is called once per frame
    void Update()
    {
        g_HPUI.fillAmount -= (m_OriginalBVP - m_CTPOfHP) * Time.deltaTime * 2;
        m_OriginalBVP = g_HPUI.fillAmount;
    }

    /// <summary>
    /// 開始產生目標物
    /// </summary>
    public void UpdateStatus(){
        if(!m_Start){
            Start();
        }
        m_GameProgress = true;
        m_GradeRange = 1;
        SetGoals();
    }
    /// <summary>
    /// 更新等級範圍
    /// </summary>
    public void SetStatus(){
        
        if(m_GradeRange + 1 <=  g_LifeLevel.Length){
            m_GradeRange++;
        }
    }
    /// <summary>
    /// 設定目標物
    /// </summary>
    private void SetGoals(){
        m_Animator.SetTrigger("Appear");
        m_Level = Random.Range(0, g_BombRate);

        m_Explosive.SetActive(false);
        
        if(m_Level == 0){
            g_TargetUI.SetActive(false);
            m_ExplosiveSpriteRenderer.sprite = g_ExplosionEffect[0]; //爆炸效果
            m_ChangeTargetIcon.sprite = g_Target[m_Level];
            m_Audio.clip = g_ExplosionAudioClip[0];
            m_SecondsToDisappear = g_BombDuration;
        }else{
            m_Level = Random.Range(m_GradeRange - 1, m_GradeRange);
            m_ExplosiveSpriteRenderer.sprite = g_ExplosionEffect[1]; //爆炸效果
            m_Audio.clip = g_ExplosionAudioClip[1];
            if(m_Level == 0){ //剛開始一等
                m_Level++;
            }else if(m_Level == g_LifeLevel.Length){ //快結束時都出現最高等
                m_Level--;
            }
            g_TargetUI.SetActive(true);
            m_TargetLevel.text = "Lv." + m_Level;
            m_ChangeTargetIcon.sprite = g_Target[Random.Range(1, g_Target.Length)];
            m_SecondsToDisappear = Random.Range(7, 9);
        }
        
        
        m_MaxHP = g_LifeLevel[m_Level];
        m_HP = m_MaxHP;
        m_OriginalBVP = 2;
        m_CTPOfHP = 2;
        g_HPUI.fillAmount = 1;
        
        
    }
    /// <summary>
    /// 0.5秒無敵
    /// </summary>
    /// <returns></returns>
    private IEnumerator SecondInvincible(){
        yield return new WaitForSeconds(0.5f);
        _Explosion = false;
    }
    /// <summary>
    /// 消失倒數
    /// </summary>
    public void DisappearingCountdown(){
        m_SecondsToDisappear--;
        if(m_SecondsToDisappear <= 0){
            SetGoals();
        }
    }

    /// <summary>
    /// 受到攻擊
    /// </summary>
    public void BeingAttacked(int _Hurt){
        if(!_Explosion && m_GameProgress){
            m_HP -= _Hurt;
            m_CTPOfHP = (float)m_HP / (float)m_MaxHP;
            if(m_HP <= 0){
                StartCoroutine(ExplosionEffect());
            }
        }
    }
    /// <summary>
    /// 是否處於爆炸效果
    /// </summary>
    private bool _Explosion = false;
    /// <summary>
    /// 爆炸效果
    /// </summary>
    private IEnumerator ExplosionEffect(){
        _Explosion = true;
        m_SecondsToDisappear = 9;
        m_Explosive.SetActive(true);
        m_Audio.Play();
        g_GameMechanics.ScoreChange(g_ScoreGrade[m_Level]);
        yield return new WaitForSeconds(1f);
        SetGoals();
        StartCoroutine(SecondInvincible());
    }
    /// <summary>
    /// 遊戲結算
    /// </summary>
    public void GameSettlement(){
        m_GameProgress = false;
    }
    /// <summary>
    /// 進行回傳
    /// </summary>
    public bool GameProgress(){
        return m_GameProgress;
    }
}
