using UnityEngine;

public class Boss : enemyController
{
     public override void die()
    {
        // 나중에 디자인패턴 상 수정 필요 (돌아가게만 함) //
        var stageManager = FindAnyObjectByType<StageManager>();
        if (stageManager != null)
        {
            stageManager.ClearAssurance();
        }


        dropper.DropItems();//아이템 드랍 함수 인스펙터창에서 프리팹과 드랍가중치 설정가능
        Destroy(gameObject, 0.1f);
    }
}
