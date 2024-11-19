
using DialogueEditor;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPC : MonoBehaviour
{
    private NPCConversation conversation;
    private QuestManager questManager;
    private bool walkInBypass;
    private bool TalkedToFirst;
    private Player _player;
    public static bool Pos1F, Pos2F, Pos3F, Neg1F, Neg2F, Neg3F;

    TextMeshProUGUI interactText;
    private bool Limbo;
    private bool Death;
    private bool Freedom;
    [SerializeField] private Animator Limbomator;
    [SerializeField] private GameObject Deathmator;
    [SerializeField] private Animator Freedommator;

    // Start is called before the first frame update
    void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
        conversation = GetComponent<NPCConversation>();
        _player = FindObjectOfType<Player>();
        interactText = GameObject.Find("TalkText").GetComponent<TextMeshProUGUI>();

    }


    void OnTriggerEnter(Collider other)
    {

        switch (tag)
        {
            case "WalkinBypass":
                walkInBypass = true;
                break;
        }




        if (other.tag == "Player" && name != "You" && !ConversationManager.Instance.IsConversationActive)
        {
           
            interactText.enabled = true;
        }

        if (!ConversationManager.Instance.IsConversationActive && walkInBypass && !TalkedToFirst && Death != true)
        {
            ConversationManager.Instance.StartConversation(conversation);
            if (ConversationManager.Instance.GetBool("TalkedToFirst") != null)
            {
                if (ConversationManager.Instance.GetBool("TalkedToFirst") == false)
                {
                    questManager.convoLock();
                    TalkedToFirst = true;
                }

            }



        }

        if (other.tag == "Player" && !ConversationManager.Instance.IsConversationActive && _player.isTalking &&
            !walkInBypass && Death != true)
        {
            ConversationManager.Instance.StartConversation(conversation);
            questManager.convoLock();
           interactText.enabled = false;
        }


    }

    void OnTriggerStay(Collider other)
    {
        switch (tag)
        {
            case "WalkinBypass":
                walkInBypass = true;
                break;
        }

        if (!ConversationManager.Instance.IsConversationActive && !_player.isTalking && Death != true)
        {

          //  print("unlocked");
            questManager.convoUnlock();
            walkInBypass = false;
        }

        if (other.tag == "Player" && !ConversationManager.Instance.IsConversationActive && _player.isTalking &&
            !walkInBypass && Death != true)
        {
            ConversationManager.Instance.StartConversation(conversation);
            questManager.convoLock();
            interactText.enabled = false;

        }
        



    }

    void OnTriggerExit(Collider other)
    {
        questManager.convoUnlock();
        walkInBypass = false;
        interactText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        print(Pos1F);
        print(Pos2F);
        print(Pos3F);
        print(Neg1F);
        print(Neg2F);
        print(Neg3F);
    }

    public void DisableThis()
    {
        gameObject.SetActive(false);
        questManager.convoUnlock();
        walkInBypass = false;

    }


    public void CheckConvo()
    {
        if (Pos1F)
        {
            if (ConversationManager.Instance.GetBool("Positive1Found") != null)
            {
                ConversationManager.Instance.SetBool("Positive1Found", true);
            }
        }
        if (Pos2F)
        {
            if (ConversationManager.Instance.GetBool("Positive2Found") != null)
            {
                ConversationManager.Instance.SetBool("Positive2Found", true);
            }
        }
        if (Pos3F)
        {
            if (ConversationManager.Instance.GetBool("Positive3Found") != null)
            {
                ConversationManager.Instance.SetBool("Positive3Found", true);
            }
        }

        if (Neg1F)
        {
            if (ConversationManager.Instance.GetBool("Negative1Found") != null)
            {
                ConversationManager.Instance.SetBool("Negative1Found", true);
            }
        }
        if (Neg2F)
        {
            if (ConversationManager.Instance.GetBool("Negative2Found") != null)
            {
                ConversationManager.Instance.SetBool("Negative2Found", true);
            }
        }
        if (Neg3F)
        {
            if (ConversationManager.Instance.GetBool("Negative3Found") != null)
            {
                ConversationManager.Instance.SetBool("Negative3Found", true);
            }
        }
        if(Freedom)
        {
          //  Freedommator.Play("Door Open");
        }
        if(Limbo)
        {
            
        }

        if (Death)
        {
           // Deathmator.SetActive(true);
           /// StartCoroutine(deathanddie());
        }

        print(name);
     
        switch (name)
        {
            case "NPC 1":
                if (ConversationManager.Instance.GetBool("Positive1Found") && !Pos1F)
                {
                    Pos1F = true;
                }
                break;
            case "NPC 2":
                if (ConversationManager.Instance.GetBool("Negative1Found") && !Neg1F)
                {
                    Neg1F = true;
                }
                break;
            case "NPC 3":
                if (ConversationManager.Instance.GetBool("Positive2Found") && !Pos2F)
                {
                    Pos2F = true;
                }
                break;
            case "NPC 4":
                if (ConversationManager.Instance.GetBool("Negative2Found") && !Neg2F)
                {
                    Neg2F = true;
                }
                break;
            case "NPC 5":
                if (ConversationManager.Instance.GetBool("Positive3Found") && !Pos3F)
                {
                    Pos3F  = true;
                }
                break;
            case "NPC 6":
                if (ConversationManager.Instance.GetBool("Negative3Found") && !Neg3F)
                {
                    Neg3F  = true;
                }
                break;
            case "Monster":
                if (ConversationManager.Instance.GetBool("Limbo") && !Limbo)
                {
                    Limbo  = true;
                }
                else if (ConversationManager.Instance.GetBool("Death") && !Death)
                {
                    Death  = true;
                }
                else if (ConversationManager.Instance.GetBool("Freedom") && !Freedom)
                {
                    Freedom  = true;
                }

                break;
        }
    }
    private IEnumerator deathanddie()
    {
        questManager.convoLock();
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainMenu");
      
    }
}


