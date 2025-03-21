﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
    The IngamePlayScene
*/
public class ChatSceneManager : MonoBehaviour
{    
    public GameObject MessageWrapperPrefab;
    public GameObject ScrollViewObject;
    public GameObject ScrollViewContent;
    float[] offset = {ChattingConfig.VerticalLayoutGroupOffset[0], ChattingConfig.VerticalLayoutGroupOffset[1], ChattingConfig.VerticalLayoutGroupOffset[2], ChattingConfig.VerticalLayoutGroupOffset[3]};
    float Spacing = ChattingConfig.VerticalLayoutGroupOffset[4];
    Flow flow;

    void Start()
    {
    }
    public void StartScene()
    {
        IngameDataManager.instance.LoadLevel("first");
        flow = IngameDataManager.instance.GetLevelFlow();
        VerticalLayoutGroup VerticalLayoutGroup = ScrollViewContent.GetComponent<VerticalLayoutGroup>();
        RectTransform rectTransform = ScrollViewContent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, offset[3]);
        VerticalLayoutGroup.spacing = Spacing * 2;
        VerticalLayoutGroup.padding = new RectOffset((int)offset[0], (int)offset[1], (int)offset[2], (int)offset[3]);
        StartCoroutine(ProcessingFlows());
    }

    IEnumerator ProcessingFlows()
    {
        bool Flag = true;
        int i = 0; // index
        while (Flag)
        {
            if (flow.flow[i].type == "chatting")
            {
                foreach(var message in flow.flow[i].chats)
                {
                    // Processing Message
                    print(message.content);
                    GameObject newObject = Instantiate(MessageWrapperPrefab) as GameObject;
                    newObject.transform.SetParent(GameObject.Find("Content").transform);
                    newObject.GetComponent<ChattingWrapperController>().Init(message.author, message.content, message.author != "player" ? 0 : 1);
                    AddScrollViewContentHeight(newObject);
                    MoveToBottom();
                    /*var newObjectRectSize = newObject.GetComponent<ChattingWrapperController>().GetSize();
                    AddScrollViewContentHeight(newObjectRectSize.y);
                    newObject.GetComponent<RectTransform>().sizeDelta = newObjectRectSize;*/

                    yield return new WaitForSeconds(message.delay / 1000);
                }
                i = flow.flow[i].branch.index[0];
            }
            else if (flow.flow[i].type == "emote")
            {
                // Processing Emote Scene
                EmojiSceneManager.instance.StartCoroutine(ProcessingFlows());
                yield return new WaitForSeconds(flow.flow[i].duration / 1000);
            }
            else if (flow.flow[i].type == "end")
            {
                Flag = false;

            }
        }
    }

    void AddScrollViewContentHeight(GameObject GameObject)
    {
        Vector2 RectSize = ScrollViewContent.GetComponent<RectTransform>().sizeDelta;
        float height = GameObject.GetComponent<RectTransform>().sizeDelta.y;
        ScrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(RectSize.x, RectSize.y + height + Spacing);
    }
    void MoveToTop() { MoveScroll(1); }
    void MoveToBottom() { MoveScroll(0); }
    void MoveScroll(float value) { ScrollViewObject.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, value); }
}