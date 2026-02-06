using UnityEngine;
using VContainer;

public class Boss : enemyController
{
    private StageManager _stageManager;
    [Inject]
    public void Construct(StageManager stageManager)
    {
        _stageManager = stageManager;
    }

     public override void die()
    {
        // 나중에 디자인패턴 상 수정 필요 (돌아가게만 함) //
        _stageManager.ClearAssurance();
        dropper.DropItems();//아이템 드랍 함수 인스펙터창에서 프리팹과 드랍가중치 설정가능
        Destroy(gameObject, 0.1f);
    }
}
