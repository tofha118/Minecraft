using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���۴��Դϴ�.
//�����ϴ� �����۵��� �ö󰡴� ����
public class Production : MonoBehaviour
{
    static public Production instance = null;

    private ItemSlot[] slots;

    public Item[] Combination_Item;

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
    public static Production Instance
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
        //GetComponentsInChildren�� �̿��� ItemSlotŸ���� ���� �ڽĵ��� ���� slots�� �ֽ��ϴ�. ->���� �����۵��� �ö� ���� (9��)�� ���մϴ�.
        slots = GetComponentsInChildren<ItemSlot>();

        Combination_Item = new Item[9];  //

    }

    //�����۵��� �ʱ�ȭ �����ִ� �Լ��Դϴ�.
    public void Reset_ItemSlot()
    {
        for (int i = 0; i < 9; i++)
        {
            slots[i].item = null;
            slots[i].ClearSlot();
            Combination_Item[i] = null;
        }
    }

    //������ �������� �������� ������ Combination_Item �迭�� ���縦 ���� ����, ���� �÷��� ������ �������� �ش� �����ǰ� �ִ����� �˻��մϴ�.
    public void copySlot_Item()
    {
        //�ش� ������ �����۵��� Combination_Item�迭�� ���� �մϴ�.
        for (int i = 0; i < 9; i++)
        {
            //�������� ���� ��찡 �ƴ϶�� ����
            if (slots[i].item != null)
            {
                Combination_Item[i] = slots[i].item;
            }
            else  //�������� ���� ���� null�� �ֽ��ϴ�.
            {
                Combination_Item[i] = null;
            }
        }

        //�ΰ��� ������� ���չ� ã�⸦ �߽��ϴ�.

        //�� ����� �׳� csv���� �о�� ������ �ϳ��ϳ� �� �˻��ؼ� ã�� ����Դϴ�.
        //  DataManager.Instance.CheckItem();

        //�� ����� dictionary�� �̿��ؼ� Ű��(���� ���ڿ��� ����߽��ϴ�) ���� �����۰� �������� ������ �ѹ��� ã�� �� �ֵ��� �ϴ� ����Դϴ�.
        DataManager2.Instance.Find_Recipe();

     
    }
}
