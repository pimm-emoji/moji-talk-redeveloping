﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;

/*
    The GameManager Class
    GameManager should be only one in game.
*/
/// <summary>
/// The GameManager Class
/// GameManager should be only one in game.
/// <example>
/// <code>
/// GameManager.instance
/// </code>
/// </example>
/// <item>
/// <term>instance</term>
/// <description>Call <c>GameManager.instance</c> when need to use <c>GameManager</c>.</description>
/// </item>
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Call <c>GameManager.instance</c> when need to use <c>GameManager</c>.
    /// </summary>
    public static GameManager instance = null;
    
    public Dictionary<string, Level> levels; //string값과 레벨데이터 를 가지고 있는 levels라는 딕셔너리
    public List<string> ProfileIndex;

    public string nowLevelID;
    public float elapsedTime = 0;
    public float elapsedFlowTime = 0;
    public string endingID;
    public int perfect = 0;
    public int great = 0;
    public int good = 0;
    public int bad = 0;
    public int miss = 0;

    // "score" variable is gameflow's score.
    // It must be updated through methods
    // * InitScore()
    // * UpdateScore()
    // * SetScore()
    public float userTotalScore = 0f;
    public float branchIndexingScore = 0f;
    public int nowFlowIndex = 1;
    
    public UserData userData;
    void Awake()
    {
        // Set GameManager unique.
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);

        UserDataManager.InitStorage();
    }
    void Start()
    {
        userData = UserDataManager.LoadStorage();
    }

    Flow flow;
    public void InitFlow()
    {
        IngameDataManager.instance.LoadLevel(nowLevelID);
        flow = IngameDataManager.instance.flow;
    }

    public void TriggerFlow()
    {
        if (nowFlowIndex == -1) nowFlowIndex++;
        else
        {
            if (flow.flow[nowFlowIndex].type == "emote") nowFlowIndex = flow.flow[nowFlowIndex].branch.index[0];
            else if (flow.flow[nowFlowIndex].type == "chatting")
            {
                flow.flow[nowFlowIndex].branch.divider.Add(100);
                for (int i = 0; i < flow.flow[nowFlowIndex].branch.divider.Count; i++)
                {
                    if (branchIndexingScore < flow.flow[nowFlowIndex].branch.divider[i]) nowFlowIndex = flow.flow[nowFlowIndex].branch.index[i];
                }
            }
            else if (flow.flow[nowFlowIndex].type == "end")
            {
                if (userData.unlockedLevelData.Find(x => x.id == nowLevelID) == null)
                {
                    userData.unlockedLevelData.Add(new Level(nowLevelID, nowLevelID, new List<Ending>()));
                }
                userData.unlockedLevelData.Find(x => x.id == nowLevelID).endings.Add(new Ending(
                    flow.flow[nowFlowIndex].endingID,
                    flow.flow[nowFlowIndex].endingID
                ));
                UserDataManager.SaveStorage(userData);
            }
        }
    }
    // 새 스테이지 해금 코드 아직 작성 않음

    [ContextMenu("Debug InitScore")] public void InitScore() { userTotalScore = 0f; }
    public void InitBranchScore() { branchIndexingScore = 0f; }


    public void AddScore(float ScoreDelta) { userTotalScore += ScoreDelta; branchIndexingScore += ScoreDelta; }

    public void SetScore(float Score) { userTotalScore = Score; }
    public void SetBranchScore(float Score) { branchIndexingScore = Score; }

    public float GetScore() { return userTotalScore; }
    public float GetBranchScore() { return branchIndexingScore; }

    [ContextMenu("Debug AddScore (ScoreDelta: 1)")] public void DebugAddScore() { AddScore(1); }
    [ContextMenu("Debug SetScore (Score: 4)")] public void DebugSetScore() { SetScore(4); }
    [ContextMenu("Debug GetScore through Print")] public void DebugGetScore() { print(GetScore()); }




    public void AddPerfect() { perfect += 1; }
    public void AddGreat() { great += 1; }
    public void AddGood() { good += 1; }
    public void AddBad() { bad += 1; }
    public void AddMiss() { miss += 1; }


    /*
        The LoadScene Method
        This Method should be enabled when scene number has been assigned.
    // ***********
    /// <summary>
    /// It is recommended to use the GameManager for loading scenes.
    /// This is because most code editors support the reference backlink function,
    /// which makes it convenient to backtrack the scene load code.
    /// <example>
    /// <code>
    /// GameManager.instance.LoadScene(3);
    /// </code>
    /// <code>
    /// GameManager.instance.LoadScene("prev");
    /// </code>
    /// </example>
    /// </summary>
    public void LoadScene(int n)
    {
        SceneManager.LoadScene(n);
    }
    public void LoadScene(string operation)
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        if (operation == "next") SceneManager.LoadScene(current + 1);
        else if (operation == "prev") SceneManager.LoadScene(current - 1);
    }
    */
}
