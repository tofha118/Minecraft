using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//item ������ scriptableobject�� ����
//�޴� ����
[CreateAssetMenu(fileName ="Item Data",menuName ="Scriptable object/Item Data")]
public class Item : ScriptableObject
{
    public int itemcode;  //������ �ڵ�
    public Sprite ItemImege;  //������ �̹���
    public string ItemName;  //������ �̸�
}
