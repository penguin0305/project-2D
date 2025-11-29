using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using JetBrains.Annotations;
using UnityEngine.Rendering.Universal;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEditor.Build.Content;


public class StageManager : MonoBehaviour
{

    //스크립트 참조
    [Header("References")]
    public PlayerStats player;
    //public Enemy enemy;
    public MapLoader mapLoader;

    //이벤트 발행
    public event System.Action OnStageFail; // StageManager가 OnStageFail이라는 이벤트를 발행
    public event System.Action<List<ItemData>> OnStageClear;
    public event System.Action OnStageEscape;
    public event System.Action<int> OnAddScore;

    //스테이지 내에서 필요한 변수
    bool IsClear = false;
    bool CheckTrigger = false;
    int StageScore = 0;
    //추가//

    // 아이템을 보관하는 임시 인벤토리 생성   ItemData 클래스 만들어지면 그거에 맞게 수정 
    private List<ItemData> tmpinventory = new List<ItemData>(); // 멀티플레이의 경우 딕셔너리 사용 <PID, List<ItemData>>
 
    private void OnEnable()
    {
        /* 다른 스크립트의 이벤트 구독
        player.OnDeath += StageEnd;
        enemy.OnDeath += CalcPoint;
        Object.OnGetItem += GetItem;
        */
    }

    void Start()
    {
        IsClear = false; CheckTrigger = false;
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
   

    //함수 이름: StageEnd
    //기능: 스테이지 종료를 알리는 함수
    //파라미터: bool IsClear -> IsClear == true이면 클리어
    //반환값: X
    public void StageEnd(bool IsClear)
    {
        if(IsClear) // 클리어 조건을 만족했으면,
        {
            OnStageClear?.Invoke(tmpinventory); // 이벤트를 구독중인 스크립트에 tmpinventory를 인자로 전달
            Debug.Log("Stage Clear");
        }
        else
        {
            OnStageFail?.Invoke();
            Debug.Log("Game Over");
            tmpinventory.Clear(); // tmpinventory 초기화
        }
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
        ItemData Existing = tmpinventory.Find(x => x.id == item.id);
        if (Existing != null) {
            Existing.quantity += item.quantity;
        }
        else if (item.id != "clear") 
            tmpinventory.Add(item);

        Debug.Log(item.id + " 획득");

        if (item.id == "tmp")
            CheckTrigger = true; // 상호작용을 위한 트리거 체크

        if (item.id == "clear") // 클리어 처리 테스트용
        {
            IsClear = true;
            StageEnd(IsClear);
        }

    }

    /*   이벤트 체이닝 끝    */

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
        // 이벤트 구독 해제
        player.OnDeath -= StageEnd;
        enemy.OnDeath -= CalcPoint;
        Object.OnGetItem -= GetItem;
    }
    */
}
