using System;
using UnityEngine;

/// <summary>
/// 對話模組
/// </summary>
[Serializable]
public class DialogueModule
{
    [SerializeField]
    [Header("對話文字")]
    [Multiline(3)]
    private string g_DialogueText;
    [SerializeField]
    [Header("對話NPC顯示")]
    private Sprite g_DialogueNPC;

    /// <summary>
    /// 顯示對話文字
    /// </summary>
    /// <returns></returns>
    public string ShowConversationText(){
        return g_DialogueText;
    }
    /// <summary>
    /// 顯示NPC
    /// </summary>
    /// <returns></returns>
    public Sprite ShowNPC(){
        return g_DialogueNPC;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    /*void Update()
    {
        if(_D2 != null && m_Rubbish == null){
            _D2.GiveCoordinates(true);
        }
    }*/
}

