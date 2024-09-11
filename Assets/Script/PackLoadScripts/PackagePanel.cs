using System.Collections;
using System.Collections.Generic;
using static packageLocalData;
using UnityEngine;
using UnityEngine.UI;

public enum PackageMode
{
    normal,
    delete,
    sort,
}

public class PackagePanel : BasePanel
{
    private Transform UIMenu;
    private Transform UIMenuWeapon;
    private Transform UIMenuFood;
    private Transform UITabName;
    private Transform UICloseBtn;
    private Transform UICenter;
    private Transform UIScrollView;
    private Transform UIDerailPanel;
    private Transform UILeftBtn;
    private Transform UIRightBtn;
    private Transform UIDeletePanel;
    private Transform UIDelerteBackBtn;
    private Transform UIDeleteInfoText;
    private Transform UIDeleteConfirmBtn;
    private Transform UIBottomMenus;
    private Transform UIDeleteBtn;
    private Transform UIDetailBtn;

    public GameObject PackageUIItemPrefab;

    // ��ǰ���洦��ʲôģʽ��
    public PackageMode curMode = PackageMode.normal;
    public List<string> deleteChooseUid;

    private string _chooseUid;
    public string chooseUID
    {
        get
        {
            return _chooseUid;
        }
        set
        {
            _chooseUid = value;
            RefreshDetail();
        }
    }
    // ���ɾ��ѡ����
    public void AddChooseDeleteUid(string uid)
    {
        this.deleteChooseUid ??= new List<string>();
        if (!this.deleteChooseUid.Contains(uid))
        {
            this.deleteChooseUid.Add(uid);
        }
        else
        {
            this.deleteChooseUid.Remove(uid);
        }
        RefreshDeletePanel();
    }

    private void RefreshDeletePanel()
    {
        RectTransform scrollContent = UIScrollView.GetComponent<ScrollRect>().content;
        foreach (Transform cell in scrollContent)
        {
            PackageCell packageCell = cell.GetComponent<PackageCell>();
            //����ѡ��״̬
            packageCell.RefreshDeleteState();
        }
    }


    override protected void Awake()
    {
        base.Awake();
        InitUI();
    }

    private void Start()
    {
        RefreshUI();
    }

    private void InitUI()
    {
        InitUIName();
        Initclik();
    }

    void RefreshUI()
    {
        RefreshScroll();
    }

    private void RefreshDetail()
    {
        //�ҵ�uid��Ӧ�Ķ�̬����
        PackageLocalItem localItem = GameManager.Instance.GetPackageLocalItemByUId(chooseUID);
        //ˢ���������
        UIDerailPanel.GetComponent<Packagedetail>().Refresh(localItem, this);
    }

    private void RefreshScroll()
    {
        // �������������ԭ������Ʒ
        RectTransform scrollContent = UIScrollView.GetComponent<ScrollRect>().content;
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }
        foreach (PackageLocalItem localData in GameManager.Instance.GetSortPackageLocalData())
        {
            Transform PackageUIItem = Instantiate(PackageUIItemPrefab.transform, scrollContent) as Transform;
            PackageCell packageCell = PackageUIItem.GetComponent<PackageCell>();
            packageCell.Refresh(localData, this);
        }

    }



    private void InitUIName()
    {
        UIMenu = transform.Find("TopCanter/Menus");
        UIMenuWeapon = transform.Find("TopCanter/Menus/Weapon");
        UIMenuFood = transform.Find("TopCanter/Menus/Food");
        UITabName = transform.Find("LeftTop/Back Text");
        UICloseBtn = transform.Find("RightTop/Exit");
        UICenter = transform.Find("Canter");
        UIScrollView = transform.Find("Canter/Scroll View");
        UIDerailPanel = transform.Find("Canter/DetailPanel");
        UILeftBtn = transform.Find("on Left");
        UIRightBtn = transform.Find("on right");

        UIDeletePanel = transform.Find("Buttom/DetelePanl");
        UIDelerteBackBtn = transform.Find("Buttom/DetelePanl/Back");
        UIDeleteInfoText = transform.Find("Buttom/DetelePanl/Info Text");
        UIDeleteConfirmBtn = transform.Find("Buttom/DetelePanl/ConfirmBtn");
        UIBottomMenus = transform.Find("Buttom/ButtomMenus");
        UIDeleteBtn = transform.Find("Buttom/ButtomMenus/DeleteBtn");
        UIDetailBtn = transform.Find("Buttom/ButtomMenus/DetailBtn");

        UIDeletePanel.gameObject.SetActive(false);
        UIBottomMenus.gameObject.SetActive(true);
    }
    
    void Initclik()
    {
        UIMenuWeapon.GetComponent<Button>().onClick.AddListener(OnClickWeapon);
        UIMenuFood.GetComponent<Button>().onClick.AddListener(OnClickFood);
        UICloseBtn.GetComponent<Button>().onClick.AddListener(OnClickClose);
        UILeftBtn.GetComponent<Button>().onClick.AddListener(OnClickLeft);
        UIRightBtn.GetComponent<Button>().onClick.AddListener(OnClickRight);

        UIDelerteBackBtn.GetComponent<Button>().onClick.AddListener(OnDelerteBack);
        UIDeleteConfirmBtn.GetComponent<Button>().onClick.AddListener(OnDeleteConfirm);
        UIDeleteBtn.GetComponent<Button>().onClick.AddListener(OnDelete);
        UIDetailBtn.GetComponent<Button>().onClick.AddListener(OnDelail);
    }

    void OnClickWeapon()
    {
        print(">>>>> OnClickWeapon");
    }

    void OnClickFood()
    {
        print(">>>>> OnClickFood");
    }

    void OnClickClose()
    {
        print(">>>>> OnClickClose");
        ClosePanel();
        UIManager.Instance.OpenPanel(UIConst.MainPanel);
    }

    void OnClickLeft()
    {
        print(">>>>> OnClickLeft");
    }

    void OnClickRight()
    {
        print(">>>>> OnClickRight");
    }

    //�˳�ɾ��ģʽ
    void OnDelerteBack()
    {
        print(">>>>> OnDelerteBack");
        curMode = PackageMode.normal;
        UIDeletePanel.gameObject.SetActive(false);
        //����ѡ�е�ɾ���б�
        deleteChooseUid = new List<string>();
        //ˢ��ѡ��״̬
        RefreshDeletePanel();
    }

    //ȷ��ɾ��ģʽ
    void OnDeleteConfirm()
    {
        print(">>>>> OnDeleteConfirm");
        if (this.deleteChooseUid == null)
        {
            return;
        }
        if(this.deleteChooseUid.Count == 0)
        {
            return; 
        }
        GameManager.Instance.DeletePackageItems(this.deleteChooseUid);
        //ɾ�����ˢ���±�������
        RefreshUI();
    }

    //����ɾ��ģʽ
    void OnDelete()
    {
        print(">>>>> OnDelete");
        curMode = PackageMode.delete;
        UIDeletePanel.gameObject.SetActive(true);
    }

    void OnDelail()
    {
        print(">>>>> OnDelail");
    }

   
}
