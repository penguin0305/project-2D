using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using JetBrains.Annotations;
using UnityEngine.Rendering.Universal;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEditor.Build.Content;
//using Cinemachine;


public class StageManager : MonoBehaviour
{

    //스크립트 참조
    [Header("References")]
    public PlayerInputHandler player;
    //public Enemy enemy;
    public MapLoader mapLoader;

    //이벤트 발행
    public event System.Action<bool> OnStageFail;
    public event System.Action<bool> OnStageClear;
    public event System.Action OnStageEscape;
    public event System.Action<int> OnAddScore;

    //스테이지 내에서 필요한 변수
    bool IsClear = false;
    bool CheckTrigger = false;
    int StageScore = 0;
    float SpawnBudget;
    //추가//

    // 아이템을 보관하는 임시 인벤토리 생성
    private List<ItemData> tmpinventory = new List<ItemData>(); // 멀티플레이의 경우 딕셔너리 사용 <PID, List<ItemData>>

    /* 합칠 때 주석 해제
    private void OnEnable()
    {
        // 다른 스크립트의 이벤트 구독
        player.OnDeath += GameOver;
        player.OnEscape += StageEscape;
        player.OnGetItem += GetItem;
        enemy.OnDeath += CalcPoint;
        enemy.OnDeath += ClearAssurance;
        enemy.OnDeath += StageClear;
    }
    */

    void Start()
    {
        IsClear = false; CheckTrigger = false;
    }

    //함수 이름: ClearAssurance
    //기능: 클리어 조건을 전부 충족시키는 함수
    //파라미터: X
    //반환값: X
    public void ClearAssurance(bool boss)
    {
        if (boss)
        {
            IsClear = true; CheckTrigger = true;
        }
        Debug.Log("Meet Clear Condition");
    }


    /* 이 이하는 이벤트 체이닝으로, 나중에 합칠 때 인자, 기능 수정 필요 */

    //함수 이름: CalcPoint
    //기능: 적을 처치했을 때 점수와 획득골드를 받아와서 처리하는 함수
    //파라미터: int score -> 해당 스테이지의 점수 증가량
    //반환값: X (이벤트를 구독하는 스크립트에게 스테이지 점수를 전달)
    public void CalcPoint(int score)
    {
        StageScore += score;
        OnAddScore?.Invoke(StageScore);
    }
   

    //함수 이름: GameOver
    //기능: 스테이지 실패를 알리는 함수
    //파라미터: X
    //반환값: X ( 이벤트를 구독하는 스크립트에게 bool IsClear ( 일반적으로 false ) 전달 )
    public void GameOver()
    {
        OnStageFail?.Invoke(IsClear);
        Debug.Log("Game Over");
        tmpinventory.Clear();
    }

    //함수 이름: StageClear
    //기능: 스테이지 클리어를 알리는 함수
    //파라미터: X
    //반환값: X ( 이벤트를 구독하는 스크립트에게 bool IsClear ( 일반적으로 true ) 전달 )
    public void StageClear(bool boss)
    {
        if (CheckTrigger && boss) OnStageClear?.Invoke(IsClear);
        Debug.Log("Stage Clear");
        /* 게임 매니저의 글로벌 인벤토리에 추가 */
    }

    //함수 이름: StageEscape
    //기능: 게임이 중간에 중단되었음을 알리는 함수
    //파라미터: bool trigger -> player로부터 '정당한 방식'임을 true로 받거나 그렇지 않으면 false
    //반환값: X (이벤트 전달값도 없음)
    public void StageEscape(bool trigger)
    {
        OnStageEscape?.Invoke();
        Debug.Log("Stage Escape");
        if(!trigger) tmpinventory.Clear();
        /* else 게임매니저의 글로벌 인벤토리에 추가 */
    }


    //함수 이름: GetItem
    //기능: 획득한 아이템을 임시 인벤토리에 추가
    //파라미터: ItemData item -> 인벤토리에 추가할 아이템
    //반환값: X
    public void GetItem(ItemData item)
    {
        if (item.id != "clear") tmpinventory.Add(item); // 멀티플레이 구현 시 동기화 처리해야
        Debug.Log(item.id + " 획득");

        if (item.id == "tmp") CheckTrigger = true; // id는 임시. 특정 지역을 지나기 위한 트리거 체크(필요 시)
        if (item.id == "clear") // 클리어 처리 테스트용. "clear" id를 가진 아이템 필요
        {
            ClearAssurance(true);
            StageClear(true);
        }

    }

    /*   이벤트 체이닝 끝    */


    // 깊이에 따른 난이도 조절
    public void ModifyDifficulty()
    {
        // enemy spawncost에 가중치를 부여
    }


    void Update()
    {
        /*
         //맵의 최상단으로 이동 시 탈출 시도임을 확인
         if (PlayerTransform.position.y > -Threshold)
         {
             StageEscape(false); // 플레이를 중단하되, 획득한 아이템이 초기화
         }
         */
    }

    /* 합칠 때 주석 해제
    private void OnDisable()
    {
        // 이벤트 구독 해제.
        player.OnDeath -= GameOver;
        player.OnEscape -= StageEscape;
        player.OnGetItem -= GetItem;
        enemy.OnDeath -= CalcPoint;
        enemy.OnDeath -= ClearAssurance;
        enemy.OnDeath -= StageClear;
    }
    */
}
