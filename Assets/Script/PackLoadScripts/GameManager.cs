using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private packageTable PackageTable;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Start()
    {
        UIManager.Instance.OpenPanel(UIConst.MainPanel);
    }

    public void DeletePackageItems(List<string> uids)
    {
        foreach (string uid in uids)
        {
            DeletePackageItem(uid, false);
        }
        packageLocalData.Instance.SavePackage();
    }

    public void DeletePackageItem(string uid, bool needSave = true)
    {
        PackageLocalItem packageLocalItem = GetPackageLocalItemByUId(uid);
        if (packageLocalItem == null)
            return;
        packageLocalData.Instance.items.Remove(packageLocalItem);
        if (needSave)
        {
            packageLocalData.Instance.SavePackage();
        }
    }

    public packageTable GetPackageTable()
    {
        if (PackageTable == null)
        {
            PackageTable = Resources.Load<packageTable>("TableData/PackageTable");
        }
        return PackageTable;
    }

    //1.���� 2.ʳ��
    //�������ͻ�ȡ���õı�������
    public List<PackageTableitem> GetPackageDataByType(int type)
    {
        List<PackageTableitem> packageItems = new List<PackageTableitem>();
        foreach (PackageTableitem packagelItem in GetPackageTable().DateList)
        {
            if(packagelItem.type == type)
            {
                packageItems.Add(packagelItem);
            }
        }
        return packageItems;
    }

    //����鿨�����һ������
    public PackageLocalItem GetLotterRandom1()
    {
        List<PackageTableitem> packageItems = GetPackageDataByType(GameConst.PackageTypeWeapon);
        int index = Random.Range(0, packageItems.Count);
        PackageTableitem packageItem = packageItems[index];
        PackageLocalItem packageLocalItem = new()
        {
            uid = System.Guid.NewGuid().ToString(),
            id = packageItem.id,
            num = 1,
            level = 1,
            isNew = CheckWeaponIsNew(packageItem.id),
        };
        packageLocalData.Instance.items.Add(packageLocalItem);
        packageLocalData.Instance.SavePackage();
        return packageLocalItem;
    }

    //����鿨�����ʮ������
    public List<PackageLocalItem> GetLotterRandom10(bool sort = false)
    {
        //����鿨
        List<PackageLocalItem> packagelocalitems = new();
        for (int i = 0; i <10 ; i++)
        {
            PackageLocalItem packageLocalItem = GetLotterRandom1();
            packagelocalitems.Add(packageLocalItem);
        }
        //��������
        if(sort)
        {
            packagelocalitems.Sort(new PackageItemComparer());
        }
        return packagelocalitems;
    }

    public bool CheckWeaponIsNew(int id)
    {
        foreach (PackageLocalItem packageLocalItem in GetPackageLocalData())
        {
            if (packageLocalItem.id == id)
            {
                return false;
            }
        }
        return true;
    }

    public List<PackageLocalItem> GetPackageLocalData()
    {
        return packageLocalData.Instance.LoadPackage();
    }

    public PackageTableitem GetPackageItemById(int id)
    {
        List<PackageTableitem> packageDataList = GetPackageTable().DateList;
        foreach (PackageTableitem item in packageDataList)
        {
            if(item.id == id)
            {
                return item;
            }
        }
        return null;
    }

    public PackageLocalItem GetPackageLocalItemByUId(string uid)
    {
        List<PackageLocalItem> packageDataList = GetPackageLocalData();
        foreach (PackageLocalItem item in packageDataList)
        {
            if (item.uid == uid)
            {
                return item;
            }
        }
        return null;
    }

    public List<PackageLocalItem> GetSortPackageLocalData()
    {
        List<PackageLocalItem> localItems = packageLocalData.Instance.LoadPackage();
        localItems.Sort(new PackageItemComparer());
        return localItems;
    }
}

public class PackageItemComparer : IComparer<PackageLocalItem>
{
    public int Compare(PackageLocalItem a, PackageLocalItem b)
    {
        PackageTableitem x = GameManager.Instance.GetPackageItemById(a.id);
        PackageTableitem y = GameManager.Instance.GetPackageItemById(b.id);
        //���Ȱ�star�Ӵ�С����
        int starCompatison = y.star.CompareTo(x.star);

        //���star��ͬ,��id�Ӵ�С����
        if(starCompatison == 0)
        {
            int idComparison = y.id.CompareTo(x.id);
            if(idComparison == 0)
            {
                return b.level.CompareTo(a.level);
            }
            return idComparison;
        }
        return starCompatison;
    }
}

public class GameConst
{
    //��������
    public const int PackageTypeWeapon = 1;
    //ʳ������
    public const int PackageTypeFood = 2;
}

