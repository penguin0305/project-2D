using UnityEngine;

public class Boss : enemyController
{
    public event System.Action OnBossDie;

    public override void die()
    {
        OnBossDie?.Invoke();
        dropper.DropItems();//아이템 드랍 함수 인스펙터창에서 프리팹과 드랍가중치 설정가능
        Destroy(gameObject, 0.1f);
    }
}
