using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� �巡������ �������� ���������� �ӽ������� �����ϰ�, �巡�����϶� �������� �巡������ ���콺�� ���󰡸鼭 �̹����� �������� �մϴ�.
public class DragSlot : MonoBehaviour
{
    static public DragSlot instance = null;

    public ItemSlot ItemSlot;  //�巡������ ������ ����
  

    [SerializeField]
    private Image imageItem;  //������ �̹���

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
    //�̱���
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

    //�̹����� ������.
    public void ResetImage()
    {
        imageItem = null;
    }

    //�巡������ ������ �̹����� �ش� ���Կ� ����
    public void DragSetImage(Image _itemImage)
    {
        imageItem = _itemImage;
        imageItem.sprite = _itemImage.sprite;

        SetColor(1);
        GetComponent<Image>().sprite = imageItem.sprite;

    }
    //�̹��� �÷� ����
    public void SetColor(float _alpha)
    {
        Color color = this.GetComponent<Image>().color;
        color.a = _alpha;
        this.GetComponent<Image>().color = color;

    }

}
