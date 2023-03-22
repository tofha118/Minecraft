using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager2 : MonoBehaviour
{
    public static DataManager2 instance=null; 
    
    //csv에서 아이템 조합 레시피를 읽어올 변수
    List<Dictionary<string, object>> data;
 
    //scriptableobject 로 저장되어있는 item의 정보를 담을 list
    public List<Item> ItemList;

    //아이템 조합레시피를 담아놓을 변수
    List<Dictionary<string,Dictionary<Item,int>>> recipe;

    //아이템 조합 창에 올라가있는 아이템들을 문자열로 바꿔서 저장할 변수
    string Production_string ="|";

    void Awake()
    {
        //싱글톤
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }   
    }
    //싱글톤
    public static DataManager2 Instance
    {
        get
        {
            if(instance==null)
            {
                return null;
            }
            return instance;
        }
    }

    void Start()
    {
       
        //recipe에 dictinary 할당
        recipe = new List<Dictionary<string, Dictionary<Item, int>>>();
     
        //data에 csv에 저장되어있는 아이템 조합법을 읽어옴
        data = CSVReader.Read("Minecraft_ItemTable");
        //data에서 읽어온 아이템 조합법들을 문자열로 바꿔줌
        Init_String_Data();
    }

    //레시피 찾기
    public void Find_Recipe()
    {
        //아이템 조합창에 올라가있는 아이템들을 문자열로 변환시켜줌.
        //바뀐 문자열은 Production_string에 저장됨.
        Init_String_ProductionSlot();
        Item item = new Item();  //찾은 아이템을 담아둘 변수를 할당시킴

        //문자열로 변환되어있는 recipe를 foreach문을 통해 순회
        foreach (var reciepe in recipe)
        {
            //만약 recipe안에 Production_string과 일치하는 문자열이 존재한다면 ->완성 아이템이 존재한다는 뜻
            if (reciepe.ContainsKey(Production_string))
            {
                foreach (var t in reciepe[Production_string].Keys)  //Production_string에 해당하는 조합 아이템의 keys값(아이템)이 있는지 확인함
                {
                    item = t; //해당하는 아이템을 item에 넣어줌. 
                }

                //reciepe[Production_string]의 값을 temp에 저장 시켜줌. (아이템과 아이템 카운트를 찾아내기 위함)
                var temp = reciepe[Production_string];

                //위에서 찾아온 아이템 값을 넣어주면 아이템 카운트 값이 나옴.
                var a = temp[item];

                //완성 아이템 슬롯에 해당 아이템과 아이템 개수를 넣어줍니다.
                Completion_itemSlot.Instance.AddItem(item, a);
                return;  //아이템 찾았으면 함수 리턴

            }
            else
            {
                Completion_itemSlot.Instance.ResetSlot();  //완성아이템이 존재하지 않을 때 완성될 아이템이 올라갈 슬롯을 비워줌.
            }
           
        }
    }


    //아이템 슬롯(조합 아이템들이 올라가있는 슬롯)들을 문자로열로 바꿔서 Production_string에 저장시키는 작업
    public void Init_String_ProductionSlot()
    {
        Production_string = string.Empty;  //Production_string을 초기화 시킴
        Production_string = "|";  //Production_string 초기에 "|"문자 하나 넣어줌.
        //해당 아이템의 코드를 문자열로 바꿔서 문자열로 완성 시킴.
        for (int i=0; i<9; i++)
        {
            //아이템이 없는 경우는 0| 으로 문자열을 이어 붙여줌
            if (Production.Instance.Combination_Item[i]==null)
            {
                Production_string += "0|";
            }
            //아이템이 있는 경우에는 해당아이템의 아이템 코드 + | 문자열로 이어붙여준다.
            else
            {
                Production_string += Production.Instance.Combination_Item[i].itemcode.ToString();
                Production_string += "|";
            }
        }
    }

    //csv에서 읽어온 레시피를 문자열로 바꾸어 저장시켜 주는 작업
    public void Init_String_Data()
    {
        //아이템의 코드들로 문자열을 만들어줍니다.
        for (int i = 0; i < data.Count; i++)
        {
            //먼저 초기 문자열 | 로 생성
            string tempString = "|";
            Item tempItem=new Item();  //아이템을 저장해 놓을 변수 하나 선언
           
            int itemCount=0;  //아이템 카운트를 저장해 놓을 변수 하나 선언

            //csv에서 읽어온 아이템 코드들로 문자열을 만듭니다.
            for (int j = 1; j < 10; j++)
            {
                tempString += data[i]["Index" + j.ToString()];
                tempString += "|";
            }
         
            //아이템 리스트 안에 있는 아이템들을 foreach문을 통해 순회
            foreach (var item in ItemList)
            {
                //아이템의 코드와 csv에서 읽어온 아이템의 코드가 일치하는 경우
                if (item.itemcode==(int)data[i]["ItemCode"])
                {
                    tempItem = item;  //아이템을 저장시키고
                    itemCount = (int)data[i]["Count"];  //csv에서 아이템의 개수를 읽어옵니다.
                    //여기에서 아이템의 개수란, 만들어지는 아이템의 개수를 뜻합니다.
                    //예를 들면 나무 원목 하나를 올려놓으면 나무아이템이 4개가 만들어지기 때문에 이걸 구분할 아이템 개수를 만들었습니다.
                }
            }

            //dictinary에 아이템과 아이템 개수를 묶어서 저장합니다.
            Dictionary<Item, int> tempItem_and_count=new Dictionary<Item, int>();
            tempItem_and_count.Add(tempItem, itemCount);

            //아까 위에서 아이템 코드로 만든 문자열들을 string 키값으로 만들고, 이 문자열을 이용하면 위에서 만든 아이템과 아이템 개수를 묶은 dictionary를 찾을 수 있습니다.
            Dictionary<string, Dictionary<Item, int>> tempRec = new Dictionary<string, Dictionary<Item, int>>();
            //만든 것들을 dictionary로 묶어서 리스트에 추가 시킵니다.
            tempRec.Add(tempString, tempItem_and_count);
            recipe.Add(tempRec);

        }
    }
}


//              아이템 이름  1번슬롯   2번슬롯 3번슬롯 4번슬롯 5번슬롯 6번슬롯  7번슬롯 8번슬롯  9번슬롯  아이템 코드  완성될 아이템 개수
//csv는 순서대로 ItemName	Index1	Index2	Index3	Index4	Index5	Index6	Index7	Index8	Index9	ItemCode	Count 입니다.

//dictinary를 이용해서 csv에서 읽어온 조합표를 문자열로 바꿔줍니다.
//예를 들어서 막대기의 경우는 0	2	0	0	2	0	0	0	0	7 가 조합입니다. (순서대로 0~9번 슬롯을 뜻합니다.)
//그러면 막대기의 경우 문자열은 |0|2|0|0|2|0|0|0|0|7| 이런 형식의 문자열이 만들어집니다.
//그러면 이 문자열 자체가 키값이 되어서 이 키를 입력하면 막대기 아이템과 막대기의 개수가 나오게 됩니다.


