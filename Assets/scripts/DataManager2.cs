using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager2 : MonoBehaviour
{
    public static DataManager2 instance=null; 
    
    //csv���� ������ ���� �����Ǹ� �о�� ����
    List<Dictionary<string, object>> data;
 
    //scriptableobject �� ����Ǿ��ִ� item�� ������ ���� list
    public List<Item> ItemList;

    //������ ���շ����Ǹ� ��Ƴ��� ����
    List<Dictionary<string,Dictionary<Item,int>>> recipe;

    //������ ���� â�� �ö��ִ� �����۵��� ���ڿ��� �ٲ㼭 ������ ����
    string Production_string ="|";

    void Awake()
    {
        //�̱���
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
    //�̱���
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
       
        //recipe�� dictinary �Ҵ�
        recipe = new List<Dictionary<string, Dictionary<Item, int>>>();
     
        //data�� csv�� ����Ǿ��ִ� ������ ���չ��� �о��
        data = CSVReader.Read("Minecraft_ItemTable");
        //data���� �о�� ������ ���չ����� ���ڿ��� �ٲ���
        Init_String_Data();
    }

    //������ ã��
    public void Find_Recipe()
    {
        //������ ����â�� �ö��ִ� �����۵��� ���ڿ��� ��ȯ������.
        //�ٲ� ���ڿ��� Production_string�� �����.
        Init_String_ProductionSlot();
        Item item = new Item();  //ã�� �������� ��Ƶ� ������ �Ҵ��Ŵ

        //���ڿ��� ��ȯ�Ǿ��ִ� recipe�� foreach���� ���� ��ȸ
        foreach (var reciepe in recipe)
        {
            //���� recipe�ȿ� Production_string�� ��ġ�ϴ� ���ڿ��� �����Ѵٸ� ->�ϼ� �������� �����Ѵٴ� ��
            if (reciepe.ContainsKey(Production_string))
            {
                foreach (var t in reciepe[Production_string].Keys)  //Production_string�� �ش��ϴ� ���� �������� keys��(������)�� �ִ��� Ȯ����
                {
                    item = t; //�ش��ϴ� �������� item�� �־���. 
                }

                //reciepe[Production_string]�� ���� temp�� ���� ������. (�����۰� ������ ī��Ʈ�� ã�Ƴ��� ����)
                var temp = reciepe[Production_string];

                //������ ã�ƿ� ������ ���� �־��ָ� ������ ī��Ʈ ���� ����.
                var a = temp[item];

                //�ϼ� ������ ���Կ� �ش� �����۰� ������ ������ �־��ݴϴ�.
                Completion_itemSlot.Instance.AddItem(item, a);
                return;  //������ ã������ �Լ� ����

            }
            else
            {
                Completion_itemSlot.Instance.ResetSlot();  //�ϼ��������� �������� ���� �� �ϼ��� �������� �ö� ������ �����.
            }
           
        }
    }


    //������ ����(���� �����۵��� �ö��ִ� ����)���� ���ڷο��� �ٲ㼭 Production_string�� �����Ű�� �۾�
    public void Init_String_ProductionSlot()
    {
        Production_string = string.Empty;  //Production_string�� �ʱ�ȭ ��Ŵ
        Production_string = "|";  //Production_string �ʱ⿡ "|"���� �ϳ� �־���.
        //�ش� �������� �ڵ带 ���ڿ��� �ٲ㼭 ���ڿ��� �ϼ� ��Ŵ.
        for (int i=0; i<9; i++)
        {
            //�������� ���� ���� 0| ���� ���ڿ��� �̾� �ٿ���
            if (Production.Instance.Combination_Item[i]==null)
            {
                Production_string += "0|";
            }
            //�������� �ִ� ��쿡�� �ش�������� ������ �ڵ� + | ���ڿ��� �̾�ٿ��ش�.
            else
            {
                Production_string += Production.Instance.Combination_Item[i].itemcode.ToString();
                Production_string += "|";
            }
        }
    }

    //csv���� �о�� �����Ǹ� ���ڿ��� �ٲپ� ������� �ִ� �۾�
    public void Init_String_Data()
    {
        //�������� �ڵ��� ���ڿ��� ������ݴϴ�.
        for (int i = 0; i < data.Count; i++)
        {
            //���� �ʱ� ���ڿ� | �� ����
            string tempString = "|";
            Item tempItem=new Item();  //�������� ������ ���� ���� �ϳ� ����
           
            int itemCount=0;  //������ ī��Ʈ�� ������ ���� ���� �ϳ� ����

            //csv���� �о�� ������ �ڵ��� ���ڿ��� ����ϴ�.
            for (int j = 1; j < 10; j++)
            {
                tempString += data[i]["Index" + j.ToString()];
                tempString += "|";
            }
         
            //������ ����Ʈ �ȿ� �ִ� �����۵��� foreach���� ���� ��ȸ
            foreach (var item in ItemList)
            {
                //�������� �ڵ�� csv���� �о�� �������� �ڵ尡 ��ġ�ϴ� ���
                if (item.itemcode==(int)data[i]["ItemCode"])
                {
                    tempItem = item;  //�������� �����Ű��
                    itemCount = (int)data[i]["Count"];  //csv���� �������� ������ �о�ɴϴ�.
                    //���⿡�� �������� ������, ��������� �������� ������ ���մϴ�.
                    //���� ��� ���� ���� �ϳ��� �÷������� ������������ 4���� ��������� ������ �̰� ������ ������ ������ ��������ϴ�.
                }
            }

            //dictinary�� �����۰� ������ ������ ��� �����մϴ�.
            Dictionary<Item, int> tempItem_and_count=new Dictionary<Item, int>();
            tempItem_and_count.Add(tempItem, itemCount);

            //�Ʊ� ������ ������ �ڵ�� ���� ���ڿ����� string Ű������ �����, �� ���ڿ��� �̿��ϸ� ������ ���� �����۰� ������ ������ ���� dictionary�� ã�� �� �ֽ��ϴ�.
            Dictionary<string, Dictionary<Item, int>> tempRec = new Dictionary<string, Dictionary<Item, int>>();
            //���� �͵��� dictionary�� ��� ����Ʈ�� �߰� ��ŵ�ϴ�.
            tempRec.Add(tempString, tempItem_and_count);
            recipe.Add(tempRec);

        }
    }
}


//              ������ �̸�  1������   2������ 3������ 4������ 5������ 6������  7������ 8������  9������  ������ �ڵ�  �ϼ��� ������ ����
//csv�� ������� ItemName	Index1	Index2	Index3	Index4	Index5	Index6	Index7	Index8	Index9	ItemCode	Count �Դϴ�.

//dictinary�� �̿��ؼ� csv���� �о�� ����ǥ�� ���ڿ��� �ٲ��ݴϴ�.
//���� �� ������� ���� 0	2	0	0	2	0	0	0	0	7 �� �����Դϴ�. (������� 0~9�� ������ ���մϴ�.)
//�׷��� ������� ��� ���ڿ��� |0|2|0|0|2|0|0|0|0|7| �̷� ������ ���ڿ��� ��������ϴ�.
//�׷��� �� ���ڿ� ��ü�� Ű���� �Ǿ �� Ű�� �Է��ϸ� ����� �����۰� ������� ������ ������ �˴ϴ�.


