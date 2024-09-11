using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static packageLocalData;

public class PackageCell : MonoBehaviour , IPointerClickHandler , IPointerEnterHandler , IPointerExitHandler
{
    private Transform UIIcon;
    private Transform UIHead;
    private Transform UINew;
    private Transform UISelect;
    private Transform UILevel;
    private Transform UIStars;
    private Transform UIDeleteselect;

    private PackageLocalItem packageLocalData;
    private PackageTableitem packageTableItem;
    private PackagePanel uiParent;

    private Transform UISelectAni;
    private Transform UIMouseOverAni;

    private void Awake()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        UIIcon = transform.Find("Top/Icon");
        UIHead = transform.Find("Top/Head");
        UINew = transform.Find("Top/New");
        UILevel = transform.Find("Buttom/LevelText");
        UISelect = transform.Find("Select");
        UIStars = transform.Find("Buttom/stars");
        UIDeleteselect = transform.Find("DeleteSelect");
        UISelectAni = transform.Find("SelectAni");
        UIMouseOverAni = transform.Find("MouseOverAni");

        UIDeleteselect.gameObject.SetActive(false);
        UIMouseOverAni.gameObject.SetActive(false);
        UISelectAni.gameObject.SetActive(false);
    }

    public void Refresh(PackageLocalItem packageLocalData , PackagePanel uiParent)
    {
        //数据初始化
        this.packageLocalData = packageLocalData;
        this.packageTableItem = GameManager.Instance.GetPackageItemById(packageLocalData.id);
        this.uiParent = uiParent;
        //等级信息
        UILevel.GetComponent<Text>().text = "Lv." + this.packageLocalData.level.ToString();
        //是否新获得
        UINew.gameObject.SetActive(this.packageLocalData.isNew);
        //物品的图片
        Texture2D t= (Texture2D)Resources.Load(this.packageTableItem.imagePath);
        Sprite temp = Sprite.Create(t, new Rect(0,0,t.width,t.height), new Vector2(0,0));
        UIIcon.GetComponent<Image>().sprite = temp;
        //刷新星级
        RefreshStars();
    }

    public void RefreshStars()
    {
        for(int i = 0; i <UIStars.childCount; i++)
        {
            Transform star = UIStars.GetChild(i);
            if(this.packageTableItem.star > i)
            {
                star.gameObject.SetActive(true);
            }
            else
            {
                star.gameObject.SetActive(false);
            }
        }
    }

    public void RefreshDeleteState()
    {
        if (this.uiParent.deleteChooseUid.Contains(this.packageLocalData.uid))
        {
            this.UIDeleteselect.gameObject.SetActive(true);
        }
        else
        {
            this.UIDeleteselect.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick:" + eventData.ToString());
        if(this.uiParent.curMode == PackageMode.delete)
        {
            this.uiParent.AddChooseDeleteUid(this.packageLocalData.uid);
        }
        if (this.uiParent.chooseUID == this.packageLocalData.uid)
            return;
        //根据点击设置最新的uid -> 进而刷新详情界面
        this.uiParent.chooseUID = this.packageLocalData.uid; 
        UISelectAni.gameObject.SetActive(true);
        UISelectAni.GetComponent<Animator>().SetTrigger("In");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter:" + eventData.ToString());
        UIMouseOverAni.gameObject.SetActive(true);
        UIMouseOverAni.GetComponent<Animator>().SetTrigger("In");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit:" + eventData.ToString());
        UIMouseOverAni.GetComponent<Animator>().SetTrigger("Out");
    }
}
