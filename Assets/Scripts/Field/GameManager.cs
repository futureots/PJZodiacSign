using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : Singleton<GameManager>
{
    DataManager dataManager;


    private void Awake()
    {
        dataManager = this.GetOrAddComponent<DataManager>();
    }
    
    #region BattleInit

    /// <summary>
    /// 배틀 진입
    /// </summary>
    /// <param name="fieldModel">사용할 필드 모델(SceneNames)</param>
    /// <param name="fieldController">사용할 필드 컨트롤러(SceneNames)</param>
    /// <remarks>필드모델 + 컨트롤러로 전투씬 로드 및 진입</remarks>
    public void EnterBattle(string fieldModel, string fieldController)
    {
        
    }

    private IEnumerator LoadBattleScene(string model, string controller)
    {
        SceneManager.LoadScene(controller);
        Scene controllerScene = SceneManager.GetActiveScene();

        // Find FieldController
        FieldController fieldController = null;
        foreach (GameObject obj in controllerScene.GetRootGameObjects())
        {
            fieldController = obj.GetComponentInChildren<FieldController>();

            if (!fieldController)
            {
                break;
            }
        }
        // Fail to Load Controller
        if (!fieldController)
        {
            Debug.LogError($"Failed to Load Controller : {controller}");
            yield break;        // INSTANT KILL
        }
        
        // Set Loading UI
        fieldController.SetLoadingUI(true);
        // loading Model Scene
        AsyncOperation modelLoadOps = SceneManager.LoadSceneAsync(model, LoadSceneMode.Additive);
        yield return new WaitUntil(() => modelLoadOps.isDone);
        
        
    }

    /// <summary>
    /// 게임 시작 또는 다음 레벨(레벨이 증가했을 때)
    /// </summary>
    public void StartBattle()
    {
        // 에이전트에 필요한 데이터를 설정하거나 로드
        dataManager.LoadAllData("data");
        var list = dataManager.GetData();

        
        // for(int i = 0; i < list.Length; i++)
        // {
        //     agents[i].SetData(list[i]);
        // }
        // level = dataManager.playerData.stageLevel;
        //
        // // 해당 레벨의 정비 페이즈 부터 시작(없을 경우 0레벨부터 시작)
        // var phaseManager = GetComponent<PhaseManager>();
        // if (phaseManager == null) return;
        // onNextLevel?.Invoke(level);
        // //phaseManager.NextLevel(dataManager.playerData.stageLevel);
    }

    #endregion

    #region BattleEnd
    /// <summary>
    /// 게임 종료 시 처리
    /// </summary>
    void EndGame()
    {
        Debug.Log("게임 종료");
        // 게임 종료 처리 로직 추가
    }
    #endregion
    
    // public bool HasGameEnded(out Agent winner)
    // {
    //     var tiles = _field.GetTiles();
    //     
    //     // LINQ를 사용해서 필드에 남아있는 팀 번호들을 수집
    //     var teams = tiles
    //         .Where(t => !t.isEmpty)
    //         .Select(t => t.occupiedObject.GetComponent<Team>())
    //         .Where(t => t != null)
    //         .Select(t => t.teamNumber)
    //         .Distinct()
    //         .ToList();
    //     
    //     // 팀이 하나만 남아있다면 승리 조건
    //     if (teams.Count == 1)
    //     {
    //         // LINQ FirstOrDefault를 사용해서 해당 팀의 에이전트를 찾기
    //         winner = agents.FirstOrDefault(a => a.team.teamNumber == teams[0]);
    //         return winner != null;
    //     }
    //
    //     winner = null;
    //     return false;
    // }
    //
    // /// <summary>
    // /// 전투 승리 시 다음 레벨로 진행 (BattlePhase용)
    // /// </summary>
    // public bool HandleBattleVictory(Agent winner)
    // {
    //     if (winner is InputManager)
    //     {
    //         Debug.Log("전투 승리! 다음 레벨로 진행합니다.");
    //         
    //         // 플레이어 데이터 저장
    //         dataManager.SetData(winner.UpdateAgentData(), level);
    //         dataManager.SaveAllData("Data");
    //         
    //         // 다음 레벨로 진행
    //         StartCoroutine(GoNextLevel());
    //         return true;
    //     }
    //     else
    //     {
    //         Debug.Log("게임 오버...");
    //         EndGame();
    //         return false;
    //     }
    // }
    //
    //
    // public void SetEntityHpBar()
    // {
    //     if (hpManager == null) return;
    //     foreach (var item in agents)
    //     {
    //         foreach(var entity in item.controller.fieldEntities)
    //         {
    //             hpManager.CreateHpBar(entity);
    //         }
    //     }
    // }
    // #endregion

}
