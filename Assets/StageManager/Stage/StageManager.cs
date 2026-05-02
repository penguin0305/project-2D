using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using JetBrains.Annotations;
/*
using UnityEngine.Rendering.Universal;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEditor.Build.Content;
*/


public class StageManager : MonoBehaviour
{

    //��ũ��Ʈ ����
    [Header("References")]
    public Player player;


    //�̺�Ʈ ����
    public event System.Action OnStageFail;
    public event System.Action<List<itemData>> OnStageClear;
    public event System.Action OnStageEscape;
    public event System.Action<int> OnAddScore;

    //�������� ������ �ʿ��� ����
    bool IsClear = false;
    bool CheckTrigger = false;
    int StageScore = 0;

    // �������� �����ϴ� �ӽ� �κ��丮 ����   ItemData Ŭ���� ��������� �װſ� �°� ���� 
    private List<itemData> tmpinventory = new List<itemData>(); // ��Ƽ�÷����� ��� ��ųʸ� ��� <PID, List<ItemData>>
 
    private void OnEnable()
    {
        
        /*
        enemy.OnDeath += CalcPoint;
        Object.OnGetItem += GetItem;
        */
    }

    void Start()
    {
        IsClear = false; CheckTrigger = false;
    }


    //�Լ� �̸�: CalcPoint
    //���: ���� óġ���� �� ������ ȹ���带 �޾ƿͼ� ó���ϴ� �Լ�
    //�Ķ����: int score -> �ش� ���������� ���� ������
    //��ȯ��: X (�̺�Ʈ�� �����ϴ� ��ũ��Ʈ���� �������� ������ ����)
    public void CalcPoint(int score)
    {
        StageScore += score;
        OnAddScore?.Invoke(StageScore);
    }
   
    public void ClearAssurance()
    {
        Debug.Log("CA");
        IsClear = true;
        StageEnd();
    }

    //�Լ� �̸�: StageEnd
    //���: �������� ���Ḧ �˸��� �Լ�
    //�Ķ����: bool IsClear -> IsClear == true�̸� Ŭ����
    //��ȯ��: X
    public void StageEnd()
    {
        if(IsClear) // Ŭ���� ������ ����������,
        {
            Debug.Log("Stage Clear");
            OnStageClear?.Invoke(tmpinventory); // �̺�Ʈ�� �������� ��ũ��Ʈ�� tmpinventory�� ���ڷ� ����
        }
        else
        {
            OnStageFail?.Invoke();
            Debug.Log("Game Over");
            tmpinventory.Clear(); // tmpinventory �ʱ�ȭ
        }
    }

    /*
    //�Լ� �̸�: StageEscape
    //���: ������ �߰��� �ߴܵǾ����� �˸��� �Լ�
    //�Ķ����: bool trigger -> player�κ��� '������ ���'���� true�� �ްų� �׷��� ������ false
    //��ȯ��: X (�̺�Ʈ ���ް��� ����)
    public void StageEscape(bool trigger)
    {
        OnStageEscape?.Invoke();
        Debug.Log("Stage Escape");
        if(!trigger) tmpinventory.Clear();
        
    }
    */


    //�Լ� �̸�: GetItem
    //���: ȹ���� �������� �ӽ� �κ��丮�� �߰�
    //�Ķ����: ItemData item -> �κ��丮�� �߰��� ������
    //��ȯ��: X
    public void GetItem(itemData item)
    {
        itemData Existing = tmpinventory.Find(x => x.itemID == item.itemID);
        if (Existing != null)
        {
            //Existing.itemQuantity += item.itemQuantity;
            Existing.itemQuantity++;
        }
        else if (item.itemID != 1234)
        {
            tmpinventory.Add(item);
            item.itemQuantity = 1;//���ο� �������� ����Ʈ�� �߰����� �� ���� 1�� ����(����)
        }

        Debug.Log(item.itemID + " ȹ��");
        Debug.Log(item.itemQuantity + "��");
        if (item.itemID == 1234)
            CheckTrigger = true; // ��ȣ�ۿ��� ���� Ʈ���� üũ
        /*
        if (item.itemID == 12345) // Ŭ���� ó�� �׽�Ʈ��
        {
            IsClear = true;
            StageEnd();
        }
        */

        if (item.itemID == 1001)
        {
            if (HistoryManager.Instance != null)
            {
                HistoryManager.Instance.AddcoinCount();
            }
        }
    }

    /*   �̺�Ʈ ü�̴� ��    */

    void Update()
    {
        /*
         //���� �ֻ������ �̵� �� Ż�� �õ����� Ȯ��
         if (PlayerTransform.position.y > -Threshold)
         {
             StageEscape(false); // �÷��̸� �ߴ��ϵ�, ȹ���� �������� �ʱ�ȭ
         }
         */
    }

 
    private void OnDisable()
    {
        // �̺�Ʈ ���� ����
        //enemy.OnDeath -= CalcPoint;
        //Object.OnGetItem -= GetItem;
    }
    
}
