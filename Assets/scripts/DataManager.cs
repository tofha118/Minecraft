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
        //싱글톤
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
        data = CSVReader.Read("Minecraft_ItemTable");  //csv에서 조합표를 읽어옵니다.

    }

    void FindItemInfo(int itemcode,int count)
    {
        foreach(var Item in ItemList)  //아이템 리스트를 돌면서 아이템과 받아온 아이템의 코드가 일치하는 경우
        {
            if(Item.itemcode== itemcode)  //해당 아이템을 아이템 개수만큼 추가 시켜줍니다.
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
            if (Production.Instance.Combination_Item[i] != null)  //슬롯의 아이템이 null이 아닐때만 검사
            {
                for (int j = 0; j < data.Count; j++)  //csv에서 가져온 아이템 인덱스와 비교 시작
                {
                    //만약 슬롯의 아이템의 코드와 csv에서 가져온 아이템의 코드와 같다면
                    if (Production.Instance.Combination_Item[i].itemcode == (int)data[j]["Index" + (i + 1).ToString()])
                    {
                        flag=CheckCombination(j);  // 이 인덱스를 넘겨서 해당 csv의 정보를 검사
                        if (flag)  //아이템 찾는것에 성공했다면
                        {
                            completion_ItemCode = (int)data[j]["ItemCode"];  //완성할 아이템의 코드를 저장시킴
                            FindItemInfo(completion_ItemCode, (int)data[j]["Count"]);  //완성된 아이템을 슬롯에 추가시켜준다.
                            break;
                        }
                        else
                        {
                            Completion_itemSlot.Instance.ResetSlot();  //아이템 찾는데 실패했다면 슬롯을 비워줌.
                            continue;
                        }
                    }
                }
            }
        }

        return completion_ItemCode;  //찾은 아이템 코드를 리턴합니다.
    }


    bool CheckCombination(int index)  //받아온 index와 일치하는 아이템 조합을 검사함.
    {
        bool flag=true;

        for(int i=0; i<9; i++)
        {
            if(Production.Instance.Combination_Item[i]==null)  //만약 슬롯이 null일때에 csv의 아이템 코드가 0이 아닌경우 즉시 반복문 탈출
            {
                if((int)data[index]["Index" + (i + 1).ToString()]!=0)
                {     
                    flag = false;
                    break;
                }
            }
            else
            {
                //슬롯의 아이템 코드와 읽어온 정보가 일치하지 않으면 즉시 반복문 탈출
                if (Production.Instance.Combination_Item[i].itemcode != (int)data[index]["Index" + (i + 1).ToString()])
                {
                    flag = false;
                    break;
                }
            }
        }

        //해당하는 아이템을 찾았다면 true, 찾지 못하면 false리턴
        return flag;
    }

   
}
