using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//현재 드래그중인 아이템의 슬롯정보를 임시적으로 저장하고, 드래그중일때 아이템이 드래그중인 마우스를 따라가면서 이미지르 보여지게 합니다.
public class DragSlot : MonoBehaviour
{
    static public DragSlot instance = null;

    public ItemSlot ItemSlot;  //드래그중인 슬롯을 저장
  

    [SerializeField]
    private Image imageItem;  //보여질 이미지

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
    //싱글톤
    public static DragSlot Instance
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

    //이미지를 없애줌.
    public void ResetImage()
    {
        imageItem = null;
    }

    //드래그중인 슬롯의 이미지를 해당 슬롯에 넣음
    public void DragSetImage(Image _itemImage)
    {
        imageItem = _itemImage;
        imageItem.sprite = _itemImage.sprite;

        SetColor(1);
        GetComponent<Image>().sprite = imageItem.sprite;

    }
    //이미지 컬러 세팅
    public void SetColor(float _alpha)
    {
        Color color = this.GetComponent<Image>().color;
        color.a = _alpha;
        this.GetComponent<Image>().color = color;

    }

}
