using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// NPC對話顯示系統
/// </summary>
public class InterfaceControl : MonoBehaviour
{
    private bool m_StartConversation; //是否開始對話
    private int m_dialogue;

    [SerializeField]
    [Header("對話NPC顯示")]
    private Image m_dialogueNPC;

    [SerializeField]
    [Header("對話Text")]
    private Text m_dialogbox_text;

    private bool m_dialogbox_displayed;

    [SerializeField]
    [Header("下一個畫面")]
    private GameObject m_Center;

    [SerializeField]
    [Header("對話")]
    private DialogueModule[] m_Dialogue;
    
    private void Start()
    {
        m_StartConversation = true;
        
        Initialization();
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.anyKeyDown){
            if(m_dialogbox_displayed){
                m_dialogue++;
            }
            if(m_dialogue >= m_Dialogue.Length){
                m_Center.SetActive(true);
                m_StartConversation = false;
                
                gameObject.SetActive(false);

            }else if(m_dialogbox_displayed){
                m_dialogbox_text.text = "";
                m_dialogbox_displayed = false;

                m_dialogueNPC.sprite = m_Dialogue[m_dialogue].ShowNPC();
                m_dialogueNPC.SetNativeSize();
                StartCoroutine(DialogueAnimation(m_Dialogue[m_dialogue].ShowConversationText()));
            }else{
                m_dialogbox_displayed = true;
            }
        }else if(!m_StartConversation){
            m_StartConversation = true;
            Initialization();
        }
    }
    /// <summary>
    /// 對話文字顯示
    /// <para>讓文字一個個出現</para>
    /// </summary>
    private IEnumerator DialogueAnimation(string _dialogue){
        if(!m_dialogbox_displayed){
            m_dialogbox_text.text += _dialogue[0].ToString();
        }else{
            m_dialogbox_text.text += _dialogue;
            _dialogue = _dialogue.Substring(_dialogue.Length);
        }

        yield return new WaitForSeconds(0.1f);
        if(_dialogue.Length > 1){
            StartCoroutine(DialogueAnimation(_dialogue.Substring(1)));
        }else{
            m_dialogbox_displayed = true;
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void Initialization(){
        m_dialogue = 0;
        m_dialogbox_displayed = false;
        m_dialogbox_text.text = "";

        
        m_dialogueNPC.sprite = m_Dialogue[m_dialogue].ShowNPC();
        m_dialogueNPC.SetNativeSize();
        StartCoroutine(DialogueAnimation(m_Dialogue[m_dialogue].ShowConversationText()));
    }
}
