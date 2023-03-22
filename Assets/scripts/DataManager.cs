using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static public DataManager instance = null;

    List<Dictionary<string, object>> data;
    public List<Item> ItemList;
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
    public static DataManager Instance
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


    // Start is called before the first frame update
    void Start()
    {
        data = CSVReader.Read("Minecraft_ItemTable");  //csv���� ����ǥ�� �о�ɴϴ�.

    }

    void FindItemInfo(int itemcode,int count)
    {
        foreach(var Item in ItemList)  //������ ����Ʈ�� ���鼭 �����۰� �޾ƿ� �������� �ڵ尡 ��ġ�ϴ� ���
        {
            if(Item.itemcode== itemcode)  //�ش� �������� ������ ������ŭ �߰� �����ݴϴ�.
            {
                Completion_itemSlot.Instance.AddItem(Item, count);
            }
        }
    }

    public int CheckItem()
    {
        bool flag;
        int completion_ItemCode=0;

        for (int i=0; i<9; i++)
        {
            if (Production.Instance.Combination_Item[i] != null)  //������ �������� null�� �ƴҶ��� �˻�
            {
                for (int j = 0; j < data.Count; j++)  //csv���� ������ ������ �ε����� �� ����
                {
                    //���� ������ �������� �ڵ�� csv���� ������ �������� �ڵ�� ���ٸ�
                    if (Production.Instance.Combination_Item[i].itemcode == (int)data[j]["Index" + (i + 1).ToString()])
                    {
                        flag=CheckCombination(j);  // �� �ε����� �Ѱܼ� �ش� csv�� ������ �˻�
                        if (flag)  //������ ã�°Ϳ� �����ߴٸ�
                        {
                            completion_ItemCode = (int)data[j]["ItemCode"];  //�ϼ��� �������� �ڵ带 �����Ŵ
                            FindItemInfo(completion_ItemCode, (int)data[j]["Count"]);  //�ϼ��� �������� ���Կ� �߰������ش�.
                            break;
                        }
                        else
                        {
                            Completion_itemSlot.Instance.ResetSlot();  //������ ã�µ� �����ߴٸ� ������ �����.
                            continue;
                        }
                    }
                }
            }
        }

        return completion_ItemCode;  //ã�� ������ �ڵ带 �����մϴ�.
    }


    bool CheckCombination(int index)  //�޾ƿ� index�� ��ġ�ϴ� ������ ������ �˻���.
    {
        bool flag=true;

        for(int i=0; i<9; i++)
        {
            if(Production.Instance.Combination_Item[i]==null)  //���� ������ null�϶��� csv�� ������ �ڵ尡 0�� �ƴѰ�� ��� �ݺ��� Ż��
            {
                if((int)data[index]["Index" + (i + 1).ToString()]!=0)
                {     
                    flag = false;
                    break;
                }
            }
            else
            {
                //������ ������ �ڵ�� �о�� ������ ��ġ���� ������ ��� �ݺ��� Ż��
                if (Production.Instance.Combination_Item[i].itemcode != (int)data[index]["Index" + (i + 1).ToString()])
                {
                    flag = false;
                    break;
                }
            }
        }

        //�ش��ϴ� �������� ã�Ҵٸ� true, ã�� ���ϸ� false����
        return flag;
    }

   
}
