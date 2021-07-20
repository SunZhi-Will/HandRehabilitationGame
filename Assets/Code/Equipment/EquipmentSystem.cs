using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 裝備系統
/// </summary>
public class EquipmentSystem : MonoBehaviour
{
    [SerializeField]
    [Header("裝備目錄")]
    private GameObject g_EquipmentCatalog;
    

    [SerializeField]
    [Header("拖移助手")]
    private MyScrollRectHelper g_MyScrollRectHelper;

    [SerializeField]
    [Header("未裝備按鈕圖示")]
    private Sprite g_NotEquipped; 
    [SerializeField]
    [Header("已裝備按鈕圖示")]
    private Sprite g_Equipped; 

    [SerializeField]
    [Header("裝備牌模板")]
    private GameObject g_EquipmentCardTemplate;

    [SerializeField]
    [Header("裝備模組")]
    private EquipmentModule[] g_EquipmentModule;
    [SerializeField]
    [Header("進入遊戲按鈕")]
    private GameObject g_EnterGameButton;
    
    private GridLayoutGroup m_DirectoryGroup; //裝備目錄群組元件
    private int m_NowEquipped; //現在裝備
    private int m_HaveEquipment; //擁有的裝備
    private Image[] m_Equipment; //所有裝備卡圖示
    private Image[] m_EButtonImage; //所有裝備卡按鈕圖示
    private Button[] m_EquipmentButton; //所有裝備按鈕
    /// <summary>
    /// 結算小圖示
    /// </summary>
    private Sprite[] m_Icon;
    /// <summary>
    /// 裝備分數
    /// </summary>
    private int[] m_EquipmentScore;

    

    private void Start()
    {
        m_NowEquipped = 0;
        
        m_HaveEquipment = PlayerPrefs.GetInt("HaveEquipment", 0); //讀取擁有的槍
        EquipmentCardSettings(); // 裝備卡設定
        EquippedWithWeapons(PlayerPrefs.GetInt("NowEquipped", 0)); //讀取現在裝備
        
        

    }
    /// <summary>
    /// 裝備卡設定
    /// </summary>
    private void EquipmentCardSettings(){
        m_DirectoryGroup = g_EquipmentCatalog.GetComponent<GridLayoutGroup>(); //群組元件
        m_DirectoryGroup.constraintCount = g_EquipmentModule.Length;

        m_Equipment = new Image[g_EquipmentModule.Length];
        m_EButtonImage = new Image[g_EquipmentModule.Length];
        m_EquipmentButton = new Button[g_EquipmentModule.Length];
        m_Icon = new Sprite[g_EquipmentModule.Length];
        m_EquipmentScore = new int[g_EquipmentModule.Length];

        GameObject _ECT; //暫時物件代替
        RectTransform[] _EquipmentCardLocation = new RectTransform[g_EquipmentModule.Length];  //所有裝備卡位置
        for (int i = 0; i < g_EquipmentModule.Length; i++)
        {
            //生成裝備卡
            _ECT = Instantiate(g_EquipmentCardTemplate, g_EquipmentCardTemplate.transform.position, g_EquipmentCardTemplate.transform.rotation);
            _ECT.transform.parent = g_EquipmentCatalog.transform;
            
            _EquipmentCardLocation[i] = _ECT.GetComponent<RectTransform>();
            _EquipmentCardLocation[i].localScale = g_EquipmentCardTemplate.GetComponent<RectTransform>().localScale;
            m_Equipment[i] = _ECT.transform.GetChild(0).GetComponent<Image>(); // 卡牌圖案元件
            m_EButtonImage[i] = _ECT.transform.GetChild(1).GetComponent<Image>(); //按鈕圖案元件
            m_EquipmentButton[i] = _ECT.transform.GetChild(1).GetComponent<Button>();
            ButtonTriggerValueTransfer(i);
            StoreUnlockedEquipment(i);
            
            m_Icon[i] = g_EquipmentModule[i].g_EquipmentIcons; //結算小圖案儲存
            m_EquipmentScore[i] = g_EquipmentModule[i].g_EquipmentScore; //裝備分數儲存
            //ResultSetting(i);
        }
        RecordSystem.m_Icon = m_Icon; //紀錄結算小圖案
        RecordSystem.m_EquipmentScore = m_EquipmentScore; //紀錄裝備分數

        g_MyScrollRectHelper.Initialization(_EquipmentCardLocation); //傳值給拖移模組
    }
    /// <summary>
    /// 解鎖裝備卡
    /// </summary>
    private void StoreUnlockedEquipment(int _i){
        if(m_HaveEquipment >= _i){ //解鎖裝備
            m_EquipmentButton[_i].gameObject.SetActive(true);
            m_Equipment[_i].overrideSprite = g_EquipmentModule[_i].g_GetEquipped;
        }else{
            m_EquipmentButton[_i].gameObject.SetActive(false);
            m_Equipment[_i].overrideSprite = g_EquipmentModule[_i].g_NotEquipped;
        }
    }

    /// <summary>
    /// 按鍵觸發傳值
    /// <para>for 迴圈跑得比 AddListener 快</para>
    /// <para>所以 迴圈跑完 的時候 AddListener 才會被執行</para>
    /// <para>為此做了這函式，以防上述事情</para>
    /// </summary>
    private void ButtonTriggerValueTransfer(int _Number){
        m_EquipmentButton[_Number].onClick.AddListener(delegate{EquippedWithWeapons(_Number);});
    }

    /// <summary>
    /// 裝上裝備
    /// </summary>
    public void EquippedWithWeapons(int _Number){
        
        m_EButtonImage[m_NowEquipped].overrideSprite = g_NotEquipped;

        m_NowEquipped = _Number;
        
        m_EButtonImage[m_NowEquipped].overrideSprite = g_Equipped;
        PlayerPrefs.SetInt("NowEquipped", m_NowEquipped);
        RecordSystem.g_EquipmentModule = g_EquipmentModule[m_NowEquipped]; //紀錄現在裝備
    }
    /// <summary>
    /// 被選取的裝備
    /// </summary>
    public void  EquipmentBeingSelected(int _Number){
        if(m_HaveEquipment < _Number){
            g_EnterGameButton.SetActive(false);
        }else{
            g_EnterGameButton.SetActive(true);
            //EquippedWithWeapons(_Number);
        }
    }

}
