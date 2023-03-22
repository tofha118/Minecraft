using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//�������� �� ����
public class ItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    //������ Ÿ��
    public enum SlotType
    {
        STORAGE_BOX,  //������
        PRODUCTION,  //���� �������� �ö󰡴� ����
        COMPLETION_ITEM  //�ϼ��� �������� ���� ����
    }

    private Text text_Count;  //�������� ������ ��� text
    public SlotType slot_type;  //������ Ÿ��
    public Image MiniItem;  //�̹����� ����
    public Item item = null;  //������ ������� ������ ����
    public int itemCount = 0;  //������ ī��Ʈ

    public void Init()  //�ʱ�ȭ �۾�
    {
        MiniItem = GetComponent<Image>();  //�ش� ������ �̹��� ������ ����

        if (item != null)  //�������� �����ϸ� �ش� ���Կ� �������� �־���.
        {
            AddItem(item);
        } 
        if (item == null)  //�������� ���� ���� ������ �̹��� �����ϰ� ��.
        {
            SetColor(0);
        }
        text_Count = GetComponentInChildren<Text>();

        if (itemCount == 0)  //�����۰����� ������. ->�������� ���°�� �������� text�� ���� ����
        {
            text_Count.text = "";
        }
        else  //������ ������ �ִ°�� ������ �������� ��
        {
            text_Count.text = itemCount.ToString();
        }
    }

    void Start()
    {
        Init();
    }
    //�̹����� �÷� ����
    public void SetColor(float _alpha)
    {
        Color color = GetComponent<Image>().color;
        color.a = _alpha;
        this.GetComponent<Image>().color = color;
    }
    //_item�� _count��ŭ �ش� ���Կ� �߰���Ŵ.
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

    //�������� ī��Ʈ��ŭ text�� ������Ʈ ��Ŵ.
    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        Text countText = GetComponentInChildren<Text>();
        countText.text = itemCount.ToString();

        //0�ΰ�� ������ �ʱ�ȭ��Ŵ
        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }
    //���� ������ �ʱ�ȭ ��Ŵ
    public void ClearSlot()
    {
        item = null;  //������ ����
        itemCount = 0; //ī��Ʈ 0 
        //������ count�ϴ� text�� �ʱ�ȭ ��Ŵ
        Text countText = GetComponentInChildren<Text>();
        countText.text = "";
        //�̹����� �����ϰ� ��
        this.MiniItem.color = new Color(255, 255, 255, 0);
    }

    //�巡�� ���� ���� �� ����
    public void OnBeginDrag(PointerEventData eventData)
    {
        //�������� �ִ� ���
        if (item != null)
        {
            //�巡�� ������ ������ ���Կ� �ش� ���� ���� ����
            DragSlot.Instance.ItemSlot = this;
            //�巡�� ���Կ� ���� �������� �̹����� �������� ��.
            DragSlot.Instance.DragSetImage(MiniItem);
            //�巡�� ������ ��ġ�� ���� ���콺(�巡�� ����) ��ġ�� �Ѱ���
            DragSlot.Instance.transform.position = eventData.position;
        }
    }

    //�巡�� ���� �� ��� �巡�� ������ ��ġ�� (�巡������ ���콺 ��ġ��)���� ��Ű�� ���� 
    public void OnDrag(PointerEventData eventData)
    {
        //�������� ���� ��
        if (item != null)
        {
            //�巡������ ��ġ�� �Ѱ��� �巡�� ���Կ� �Ѱ���.
            DragSlot.Instance.transform.position = eventData.position;
        }
    }

    //�巡�� ������ ��
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        //�巡�� ������ ItemSlot�� �������� �ִٸ�
        if (DragSlot.Instance.ItemSlot != null)
        {
            //�巡�� ���� �ʱ�ȭ ������
            //�÷� �ʱ�ȭ(�����ϰ�)
            DragSlot.Instance.SetColor(0);
            //������ ������
            DragSlot.Instance.ItemSlot = null;
        }


    }

    //���մ� ������ ������ ������ �ϳ����� �ø� �� �ְ� �ϱ� ���ؼ� ���� �Լ��Դϴ�.
    public void ItemDownChange()
    {
        //�����۰� ������ ī��Ʈ ������ �Ͻ������� �����ŵ�ϴ�.
        Item _tempItem = item;  
        int _tempItemCount = itemCount;

        //������ ������ �ϳ��� �ö󰥼��ֵ��� ī��Ʈ�� 1�� �ֽ��ϴ�.
        //�巡������ �������� 1�� �߰� ��Ŵ.
        AddItem(DragSlot.Instance.ItemSlot.item, 1);

        //�̹��� ���İ� �־���.
        SetColor(1);

        //_tempItem�� �ִ� ���
        if (_tempItem != null)
        {
            //�巡�� ���Կ� �ش� �����۰� ������ ī��Ʈ�� �߰� ��ŵ�ϴ�.
            DragSlot.Instance.ItemSlot.AddItem(_tempItem, _tempItemCount);
            //�巡�� ������ �̹��� �������� ����
            DragSlot.Instance.ItemSlot.SetColor(1);
        }
        else
        {
            //�巡�� ������ ������ ī��Ʈ�� -1��ŭ ���ݴϴ�.
            DragSlot.Instance.ItemSlot.SetSlotCount(-1);
        }

        //�巡�� ������ ������ �ʱ�ȭ
        DragSlot.Instance.ItemSlot = null;
        //�̹��� ������
        DragSlot.Instance.ResetImage();
        //�÷����� �����ϰ� ����
        DragSlot.Instance.SetColor(0);

    }

    //�巡�� ���� ������ ��� ���� ��
    public virtual void OnDrop(PointerEventData eventData)
    {
        //�巡������ ���԰� ������� ���� ������ ���ٸ� ����
        if (DragSlot.Instance.ItemSlot == this)
        {
            return;
        }
       
        if (DragSlot.Instance.ItemSlot != null)  //�巡�� �� ������ �����۽����� ������� ���� ��
        {
            switch (DragSlot.Instance.ItemSlot.slot_type)  //�巡�� ������ Ÿ������ ����
            {
                case ItemSlot.SlotType.STORAGE_BOX:  //�巡�� ������ �����۽���Ÿ���� �������ϰ��
                    switch (slot_type)  //������ ������ Ÿ������ ����
                    {
                        case ItemSlot.SlotType.PRODUCTION:  //�巡�� ������ �������̰� ������ ������ Ÿ���� ����
                            if (item == null)
                            {
                                ItemDownChange();               // ������ ������ �ϳ����� �ű� �� �ֵ��� ������.
                                Production.Instance.copySlot_Item();  //�ش� �������� ���չ��� �ִ��� Ȯ��
                            }
                            else  //�������� �ִ� ��쿡�� ������ ü����(������ ü����)
                            {
                                ChangeSlot();
                            }
                            break;

                        case ItemSlot.SlotType.STORAGE_BOX:  //�巡�� ������ �������̰������ ������ Ÿ���� �������ϰ��

                            if (item != null && DragSlot.Instance.ItemSlot.item.itemcode == item.itemcode)  //�巡�� ������ �����۰� ���� ���Կ� ����ִ� �������� ������ ���� ��.
                            {
                                SetSlotCount(DragSlot.Instance.ItemSlot.itemCount);  //�巡�� ������ ����ִ� �������� ������ŭ �����ش�.
                                DragSlot.Instance.ItemSlot.ClearSlot();  //�巡�� ������ �ʱ�ȭ ������.

                            }
                            else  //������ ������ ���� ���� ��
                            {
                                ChangeSlot();  //������ �ٲ�

                            }

                            break;

                    }
                    break;

                case ItemSlot.SlotType.PRODUCTION:   //�巡�� ������ �����۽���Ÿ���� �����ϰ��
                    switch (slot_type)  //������ ������ Ÿ������ ����
                    {
                        case ItemSlot.SlotType.PRODUCTION:  //�巡�� ������ �����̰� ������ ������ Ÿ���� ���� �ϰ��
                            ChangeSlot();  //������ �ٲ�
                            Production.Instance.copySlot_Item();  //���� ���Կ� �ö��ִ� �����۳��� �����ؼ� �ϼ��� �������� �ִ����� �˻�
                            break;

                        case ItemSlot.SlotType.STORAGE_BOX:  //�巡�� ������ �����̰� ������ Ÿ���� �������ϰ��

                            if (item != null && DragSlot.Instance.ItemSlot.item.itemcode == item.itemcode)  //�巡�� ������ �����۰� ���� ���Կ� ����ִ� �������� ������ ���� ��.
                            {
                                SetSlotCount(DragSlot.Instance.ItemSlot.itemCount);  //�巡�� ������ ����ִ� �������� ������ŭ �����ش�.
                                DragSlot.Instance.ItemSlot.ClearSlot();  //�巡�� ���� �ʱ�ȭ

                            }
                            else  //������ ������ ���� ���� ��
                            {
                                if (item == null || itemCount == 1)  //�������� ���ų� ������ ī��Ʈ�� 1�϶�
                                {
                                    ChangeSlot();  //��� ���⼭ ī��Ʈ�� Ȯ���ؼ� ü���� ���Ѿ���. �����Կ��� �ϳ��� �ö󰥼��ֵ���.
                                }

                            }
                            Production.Instance.copySlot_Item();   //���� ���Կ� �ö��ִ� �����۳��� �����ؼ� �ϼ��� �������� �ִ����� �˻�
                            break;
                    }
                    break;

                case ItemSlot.SlotType.COMPLETION_ITEM:  //������ ���� Ÿ���� �ϼ��� �������� �ö� ������ ��
                    switch(slot_type)
                    {
                        case ItemSlot.SlotType.STORAGE_BOX:  //���������� ������� ��
                            if (item == null)  //�������� ���� �����
                            {
                                ChangeSlot();  //���� �ٲ���.
                            }
                            break;
                    }
                 
                    break;

            }
        }
    }

    //���� ü���� (������ �ٲٱ�)
    public void ChangeSlot()
    {
        //�����۰� ������ �ӽ� ����
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        //�巡�� ������ �����۰� ������ �ش� ���Կ� �־���
        AddItem(DragSlot.Instance.ItemSlot.item, DragSlot.Instance.ItemSlot.itemCount);

        //�̹��� �� ����
        SetColor(1);

        //�������� �ִ� ���
        if (_tempItem != null)
        {
            //�巡�� ���Կ� �ش罽�Կ� �ִ� �����۰� ���� �־���
            DragSlot.Instance.ItemSlot.AddItem(_tempItem, _tempItemCount);
            //�̹��� �� ����
            DragSlot.Instance.ItemSlot.SetColor(1);
        }
        else  //������ ���� ���
        {
            //�巡�� ���� �ʱ�ȭ
            DragSlot.Instance.ItemSlot.ClearSlot();
        }

        //�巡�� ���� �ʱ�ȭ
        DragSlot.Instance.ItemSlot = null;
        DragSlot.Instance.ResetImage();
        DragSlot.Instance.SetColor(0);
    }

    //Ŭ������ ���
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
