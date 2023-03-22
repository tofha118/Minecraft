using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//�ϼ��� �������� ���� �� �����Դϴ�.
//�⺻������ ItemSlot�� ��ӹ޽��ϴ�.
public class Completion_itemSlot : ItemSlot
{
    public static Completion_itemSlot instance=null;

    void Awake()
    {
        //�̱���
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    public static Completion_itemSlot Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }
    void Start()
    {
        //��ӹ��� ItemSlot�� Init�� ���ݴϴ�
        base.Init();
    }
    //���� �ʱ�ȭ
    public void ResetSlot()
    {
        this.item = null;
        this.ClearSlot();
    }
    //�巡�� ������ ��
    public override void OnEndDrag(PointerEventData eventData)
    {
        //�巡�� ������ �Ⱥ������ �� �����
        if (DragSlot.Instance.ItemSlot != null)
        {
            DragSlot.Instance.SetColor(0);
            DragSlot.Instance.ItemSlot = null;
        }
        //�ϼ��� �������� ������ �ʱ�ȭ ������.
        Production.Instance.Reset_ItemSlot();
    }

}
