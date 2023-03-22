using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//item 정보를 scriptableobject로 만듦
//메뉴 생성
[CreateAssetMenu(fileName ="Item Data",menuName ="Scriptable object/Item Data")]
public class Item : ScriptableObject
{
    public int itemcode;  //아이템 코드
    public Sprite ItemImege;  //아이템 이미지
    public string ItemName;  //아이템 이름
}
