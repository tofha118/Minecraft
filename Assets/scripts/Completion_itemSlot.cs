using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//완성된 아이템이 등장 할 슬롯입니다.
//기본적으로 ItemSlot을 상속받습니다.
public class Completion_itemSlot : ItemSlot
{
    public static Completion_itemSlot instance=null;

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
        //상속받은 ItemSlot의 Init을 해줍니다
        base.Init();
    }
    //슬롯 초기화
    public void ResetSlot()
    {
        this.item = null;
        this.ClearSlot();
    }
    //드래그 끝났을 때
    public override void OnEndDrag(PointerEventData eventData)
    {
        //드래그 슬롯이 안비어있을 때 비워줌
        if (DragSlot.Instance.ItemSlot != null)
        {
            DragSlot.Instance.SetColor(0);
            DragSlot.Instance.ItemSlot = null;
        }
        //완성된 아이템의 슬롯을 초기화 시켜줌.
        Production.Instance.Reset_ItemSlot();
    }

}
