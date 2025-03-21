﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EmojiSceneManager : MonoBehaviour
{
    EmojiSpawner emojispawner;
    float BranchScore;
    int dividercount;
    Flow flow;
    bool dupl;
    public static EmojiSceneManager instance = null;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);
    }
    void Start()
    {
        StartScene();
    }

    public void StartScene()
    {
        emojispawner = GameObject.Find("EmojiNote").GetComponent<EmojiSpawner>();
        IngameDataManager.instance.LoadLevel("first");
        flow = IngameDataManager.instance.GetLevelFlow();
        StartCoroutine(ProcessingFlows());


        dupl = false;
    }

    public void ResetScene()
    {
        emojispawner.spawnswitch = false;
        GameManager.instance.InitScore();
        GameManager.instance.InitBranchScore();
    }


    public IEnumerator ProcessingFlows()
    {
        int i = 1; // index


        emojispawner.spawnswitch = true;
        yield return new WaitForSeconds(flow.flow[i].duration / 1000);
        emojispawner.spawnswitch = false;
        BranchScore = GameManager.instance.GetBranchScore();

        dividercount = flow.flow[i].branch.divider.Count;

        for (int a = 0; a < dividercount; a++)
        {

            if (dupl == false)
            {
                if (BranchScore <= flow.flow[i].branch.divider[a])
                {
                    i = flow.flow[i].branch.index[a];
                    dupl = true;
                }
            }
        }
        GameManager.instance.InitBranchScore();


        // Processing chatting Scene
        //chatting.loadchat()

    }
}
     