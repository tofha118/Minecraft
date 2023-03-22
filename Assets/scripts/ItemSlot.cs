using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//아이템이 들어갈 슬롯
public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    //슬롯의 타입
    public enum SlotType
    {
        STORAGE_BOX,  //보관함
        PRODUCTION,  //조합 아이템이 올라가는 슬롯
        COMPLETION_ITEM  //완성된 아이템이 나올 슬롯
    }

    private Text text_Count;  //아이템의 개수를 띄울 text
    public SlotType slot_type;  //슬롯의 타입
    public Image MiniItem;  //이미지를 저장
    public Item item = null;  //슬롯이 들고있을 아이템 정보
    public int itemCount = 0;  //아이템 카운트

    public void Init()  //초기화 작업
    {
        MiniItem = GetComponent<Image>();  //해당 슬롯의 이미지 정보를 들고옴

        if (item != null)  //아이템이 존재하면 해당 슬롯에 아이템을 넣어줌.
        {
            AddItem(item);
        } 
        if (item == null)  //아이템이 존재 하지 않으면 이미지 투명하게 함.
        {
            SetColor(0);
        }
        text_Count = GetComponentInChildren<Text>();

        if (itemCount == 0)  //아이템개수가 없을때. ->아이템이 없는경우 보여지는 text가 없게 만듦
        {
            text_Count.text = "";
        }
        else  //아이템 개수가 있는경우 개수를 보여지게 함
        {
            text_Count.text = itemCount.ToString();
        }
    }

    void Start()
    {
        Init();
    }
    //이미지의 컬러 세팅
    public void SetColor(float _alpha)
    {
        Color color = GetComponent<Image>().color;
        color.a = _alpha;
        this.GetComponent<Image>().color = color;
    }
    //_item을 _count만큼 해당 슬롯에 추가시킴.
    public void AddItem(Item _item, int _count = 1)
    {

        item = _item;
        itemCount = _count;

        Color color = GetComponent<Image>().color;
        color.a = 1;
        GetComponent<Image>().color = color;

        GetComponent<Image>().sprite = _item.ItemImege;

        MiniItem.sprite = _item.ItemImege;
        MiniItem.color = color;

        Text countText = GetComponentInChildren<Text>();
        countText.text = _count.ToString();

    }

    //아이템의 카운트만큼 text를 업데이트 시킴.
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        Text countText = GetComponentInChildren<Text>();
        countText.text = itemCount.ToString();

        //0인경우 슬롯을 초기화시킴
        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }
    //슬롯 정보를 초기화 시킴
    public void ClearSlot()
    {
        item = null;  //아이템 빼줌
        itemCount = 0; //카운트 0 
        //아이템 count하는 text도 초기화 시킴
        Text countText = GetComponentInChildren<Text>();
        countText.text = "";
        //이미지도 투명하게 함
        this.MiniItem.color = new Color(255, 255, 255, 0);
    }

    //드래그 시작 했을 때 감지
    public void OnBeginDrag(PointerEventData eventData)
    {
        //아이템이 있는 경우
        if (item != null)
        {
            //드래그 슬롯의 아이템 슬롯에 해당 슬롯 정보 저장
            DragSlot.Instance.ItemSlot = this;
            //드래그 슬롯에 현재 아이템의 이미지를 보여지게 함.
            DragSlot.Instance.DragSetImage(MiniItem);
            //드래그 슬롯의 위치에 현재 마우스(드래그 시작) 위치를 넘겨줌
            DragSlot.Instance.transform.position = eventData.position;
        }
    }

    //드래그 중일 때 계속 드래그 슬롯의 위치를 (드래그중인 마우스 위치로)조정 시키는 역할 
    public void OnDrag(PointerEventData eventData)
    {
        //아이템이 있을 때
        if (item != null)
        {
            //드래그중인 위치를 넘겨줌 드래그 슬롯에 넘겨줌.
            DragSlot.Instance.transform.position = eventData.position;
        }
    }

    //드래그 끝났을 때
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //드래그 슬롯의 ItemSlot에 아이템이 있다면
        if (DragSlot.Instance.ItemSlot != null)
        {
            //드래그 슬롯 초기화 시켜줌
            //컬러 초기화(투명하게)
            DragSlot.Instance.SetColor(0);
            //아이템 없애줌
            DragSlot.Instance.ItemSlot = null;
        }


    }

    //조합대 위에는 무조건 아이템 하나씩만 올릴 수 있게 하기 위해서 만든 함수입니다.
    public void ItemDownChange()
    {
        //아이템과 아이템 카운트 정보를 일시적으로 저장시킵니다.
        Item _tempItem = item;  
        int _tempItemCount = itemCount;

        //무조건 아이템 하나만 올라갈수있도록 카운트는 1을 넣습니다.
        //드래그중인 아이템을 1만 추가 시킴.
        AddItem(DragSlot.Instance.ItemSlot.item, 1);

        //이미지 알파값 넣어줌.
        SetColor(1);

        //_tempItem이 있는 경우
        if (_tempItem != null)
        {
            //드래그 슬롯에 해당 아이템과 아이템 카운트를 추가 시킵니다.
            DragSlot.Instance.ItemSlot.AddItem(_tempItem, _tempItemCount);
            //드래그 슬롯의 이미지 보여지게 세팅
            DragSlot.Instance.ItemSlot.SetColor(1);
        }
        else
        {
            //드래그 슬롯의 아이템 카운트를 -1만큼 빼줍니다.
            DragSlot.Instance.ItemSlot.SetSlotCount(-1);
        }

        //드래그 슬롯의 아이템 초기화
        DragSlot.Instance.ItemSlot = null;
        //이미지 없애줌
        DragSlot.Instance.ResetImage();
        //컬러값도 투명하게 조정
        DragSlot.Instance.SetColor(0);

    }

    //드래그 중인 슬롯을 드롭 했을 때
    public virtual void OnDrop(PointerEventData eventData)
    {
        //드래그중인 슬롯과 드롭했을 때의 슬롯이 같다면 리턴
        if (DragSlot.Instance.ItemSlot == this)
        {
            return;
        }
       
        if (DragSlot.Instance.ItemSlot != null)  //드래그 한 슬롯의 아이템슬롯이 비어있지 않을 때
        {
            switch (DragSlot.Instance.ItemSlot.slot_type)  //드래그 슬롯의 타입으로 나눔
            {
                case ItemSlot.SlotType.STORAGE_BOX:  //드래그 슬롯의 아이템슬롯타입이 보관함일경우
                    switch (slot_type)  //아이템 슬롯의 타입으로 나눔
                    {
                        case ItemSlot.SlotType.PRODUCTION:  //드래그 슬롯이 보관함이고 아이템 슬롯의 타입이 제작
                            if (item == null)
                            {
                                ItemDownChange();               // 무조건 아이템 하나씩만 옮길 수 있도록 설정함.
                                Production.Instance.copySlot_Item();  //해당 아이템의 조합법이 있는지 확인
                            }
                            else  //아이템이 있는 경우에는 슬롯을 체인지(아이템 체인지)
                            {
                                ChangeSlot();
                            }
                            break;

                        case ItemSlot.SlotType.STORAGE_BOX:  //드래그 슬롯이 보관함이고아이템 슬롯의 타입이 보관함일경우

                            if (item != null && DragSlot.Instance.ItemSlot.item.itemcode == item.itemcode)  //드래그 슬롯의 아이템과 현재 슬롯에 담겨있는 아이템의 정보가 같을 때.
                            {
                                SetSlotCount(DragSlot.Instance.ItemSlot.itemCount);  //드래그 슬롯이 담고있는 아이템의 개수만큼 더해준다.
                                DragSlot.Instance.ItemSlot.ClearSlot();  //드래그 슬롯을 초기화 시켜줌.

                            }
                            else  //아이템 정보가 같지 않을 때
                            {
                                ChangeSlot();  //아이템 바꿈

                            }

                            break;

                    }
                    break;

                case ItemSlot.SlotType.PRODUCTION:   //드래그 슬롯의 아이템슬롯타입이 제작일경우
                    switch (slot_type)  //아이템 슬롯의 타입으로 나눔
                    {
                        case ItemSlot.SlotType.PRODUCTION:  //드래그 슬롯이 제작이고 아이템 슬롯의 타입이 제작 일경우
                            ChangeSlot();  //아이템 바꿈
                            Production.Instance.copySlot_Item();  //제작 슬롯에 올라가있는 아이템끼리 조합해서 완성될 아이템이 있는지를 검사
                            break;

                        case ItemSlot.SlotType.STORAGE_BOX:  //드래그 슬롯이 제작이고 슬롯의 타입이 보관함일경우

                            if (item != null && DragSlot.Instance.ItemSlot.item.itemcode == item.itemcode)  //드래그 슬롯의 아이템과 현재 슬롯에 담겨있는 아이템의 정보가 같을 때.
                            {
                                SetSlotCount(DragSlot.Instance.ItemSlot.itemCount);  //드래그 슬롯이 담고있는 아이템의 개수만큼 더해준다.
                                DragSlot.Instance.ItemSlot.ClearSlot();  //드래그 슬롯 초기화

                            }
                            else  //아이템 정보가 같지 않을 때
                            {
                                if (item == null || itemCount == 1)  //아이템이 없거나 아이템 카운트가 1일때
                                {
                                    ChangeSlot();  //대신 여기서 카운트를 확인해서 체인지 시켜야함. 제작함에는 하나만 올라갈수있도록.
                                }

                            }
                            Production.Instance.copySlot_Item();   //제작 슬롯에 올라가있는 아이템끼리 조합해서 완성될 아이템이 있는지를 검사
                            break;
                    }
                    break;

                case ItemSlot.SlotType.COMPLETION_ITEM:  //아이템 슬롯 타입이 완성된 아이템이 올라간 슬롯일 때
                    switch(slot_type)
                    {
                        case ItemSlot.SlotType.STORAGE_BOX:  //보관함으로 드롭했을 때
                            if (item == null)  //아이템이 없는 경우라면
                            {
                                ChangeSlot();  //슬롯 바꿔줌.
                            }
                            break;
                    }
                 
                    break;

            }
        }
    }

    //슬롯 체인지 (아이템 바꾸기)
    public void ChangeSlot()
    {
        //아이템과 개수를 임시 저장
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        //드래그 슬롯의 아이템과 개수를 해당 슬롯에 넣어줌
        AddItem(DragSlot.Instance.ItemSlot.item, DragSlot.Instance.ItemSlot.itemCount);

        //이미지 값 세팅
        SetColor(1);

        //아이템의 있는 경우
        if (_tempItem != null)
        {
            //드래그 슬롯에 해당슬롯에 있던 아이템과 개수 넣어줌
            DragSlot.Instance.ItemSlot.AddItem(_tempItem, _tempItemCount);
            //이미지 값 세팅
            DragSlot.Instance.ItemSlot.SetColor(1);
        }
        else  //아이템 없는 경우
        {
            //드래그 슬롯 초기화
            DragSlot.Instance.ItemSlot.ClearSlot();
        }

        //드래그 슬롯 초기화
        DragSlot.Instance.ItemSlot = null;
        DragSlot.Instance.ResetImage();
        DragSlot.Instance.SetColor(0);
    }

    //클릭했을 경우
    public void OnPointerClick(PointerEventData eventData)
    {
        DragSlot.Instance.ItemSlot = this;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
