using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Packagedetail : MonoBehaviour
{
    private Transform UIStars;
    private Transform UIDescription;
    private Transform UIIcon;
    private Transform UITitle;
    private Transform UILevelText;
    private Transform UISkillDescription;

    private PackageLocalItem packageLocalData;
    private PackageTableitem packageTableitem;
    private PackagePanel uiParent;

    private void Awake()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        UIStars = transform.Find("Center/stars");
        UIDescription = transform.Find("Center/Decsription");
        UIIcon = transform.Find("Center/Icon");
        UITitle = transform.Find("Top/Title");
        UILevelText = transform.Find("Buttom/LevenPnl/LevetText");
        UISkillDescription = transform.Find("Buttom/Decsription");
    }

    public void Refresh(PackageLocalItem packagelocalData , PackagePanel uiParent)
    {
        //初始化:动态逻辑、静态数据、父物体逻辑
        this.packageLocalData = packagelocalData;
        this.packageTableitem = GameManager.Instance.GetPackageItemById(packagelocalData.id);
        this.uiParent = uiParent;
        //等级
        UILevelText.GetComponent<Text>().text = string.Format("Lv.{0}/40" , this.packageLocalData.level.ToString());
        //简短描述
        UIDescription.GetComponent<Text>().text = this.packageTableitem.description;
        //详细描述
        UISkillDescription.GetComponent<Text>().text = this.packageTableitem.skillDescription;
        //物体名称
        UITitle.GetComponent<Text>().text = this.packageTableitem.name;
        //图片加载
        Texture2D t = (Texture2D)Resources.Load(this.packageTableitem.imagePath);
        Sprite temp = Sprite.Create(t , new Rect(0, 0, t.width , t.height), new Vector2(0,0));
        UIIcon.GetComponent<Image>().sprite = temp;
        //星级处理
        RefreshStars();
    }

    public void RefreshStars()
    {
        for (int i = 0; i <UIStars.childCount; i++)
        {
            Transform star = UIStars.GetChild(i);
            if(this.packageTableitem.star > i)
            {
                star.gameObject.SetActive(true);
            }
            else
            {
                star.gameObject.SetActive(false);
            }
        }
    }
}
