using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//제작대입니다.
//조합하는 아이템들이 올라가는 슬롯
public class Production : MonoBehaviour
{
    static public Production instance = null;

    private ItemSlot[] slots;

    public Item[] Combination_Item;

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
        //GetComponentsInChildren을 이용해 ItemSlot타입을 가진 자식들을 전부 slots에 넣습니다. ->조합 아이템들이 올라갈 슬롯 (9개)를 뜻합니다.
        slots = GetComponentsInChildren<ItemSlot>();

        Combination_Item = new Item[9];  //

    }

    //아이템들을 초기화 시켜주는 함수입니다.
    public void Reset_ItemSlot()
    {
        for (int i = 0; i < 9; i++)
        {
            slots[i].item = null;
            slots[i].ClearSlot();
            Combination_Item[i] = null;
        }
    }

    //슬롯의 정보에서 아이템의 정보만 Combination_Item 배열에 복사를 해준 다음, 현재 올려진 아이템 조합으로 해당 레시피가 있는지를 검사합니다.
    public void copySlot_Item()
    {
        //해당 슬롯의 아이템들을 Combination_Item배열에 복사 합니다.
        for (int i = 0; i < 9; i++)
        {
            //아이템이 없는 경우가 아니라면 복사
            if (slots[i].item != null)
            {
                Combination_Item[i] = slots[i].item;
            }
            else  //아이템이 없는 경우면 null을 넣습니다.
            {
                Combination_Item[i] = null;
            }
        }

        //두가지 방법으로 조합법 찾기를 했습니다.

        //이 방법은 그냥 csv에서 읽어온 파일을 하나하나 다 검사해서 찾는 방법입니다.
        //  DataManager.Instance.CheckItem();

        //이 방법은 dictionary를 이용해서 키값(저는 문자열을 사용했습니다) 으로 아이템과 아이템의 개수를 한번에 찾을 수 있도록 하는 방법입니다.
        DataManager2.Instance.Find_Recipe();

     
    }
}
