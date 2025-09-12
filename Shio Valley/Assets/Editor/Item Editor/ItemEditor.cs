using Codice.Client.BaseCommands;
using Codice.CM.Common;
using Unity.Burst.Intrinsics;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System.IO;
public class ItemEditor : EditorWindow
{

    [MenuItem("Tools/ItemEditor")]
    public static void ShowExample()
    {
        ItemEditor wnd = GetWindow<ItemEditor>();
        wnd.titleContent = new GUIContent("Item Editor");
    }
    private ScrollView itemList;
    private VisualTreeAsset itemTemplate;
    private VisualElement root;
    private bool isUpdate;
    private ItemDatabase itemDatabase;

    private VisualElement detail;
    private VisualElement itemImage;
    private IntegerField id;
    private TextField Itemname;
    private IntegerField price;
    private UnityEditor.UIElements.ObjectField icon;
    private string iconPath;
    private string iconKey;
    private Toggle canUse;
    private Toggle canCustomized;
    private Toggle canEat;
    private IntegerField maxStack;

    private EnumField type;
    private TextField description;
    private int curId;
    private Button deleteButton;

    private Toggle spring;
    private Toggle summer;
    private Toggle autumn;
    private Toggle winter;
    public void CreateGUI()
    {
        isUpdate = false;
        root = rootVisualElement;
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Item Editor/ItemEditor.uxml");
        visualTree.CloneTree(root);

        itemList = root.Q<ScrollView>("ItemList");
        itemTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/Item Editor/ItemTemplate.uxml");
        RefreshList();

        detail = root.Q<VisualElement>("ItemDetail");
        itemImage = detail.Q<VisualElement>("ItemImage");
        id = detail.Q<IntegerField>("Id");
        Itemname = detail.Q<TextField>("Name");
        price = detail.Q<IntegerField>("Price");
        type = detail.Q<EnumField>("Type");
        description = detail.Q<TextField>("Description");
        icon = detail.Q<UnityEditor.UIElements.ObjectField>("IconPath");
        canUse = detail.Q<Toggle>("canUse");
        canCustomized = detail.Q<Toggle>("canCustomized");
        canEat = detail.Q<Toggle>("canEat");
        maxStack = detail.Q<IntegerField>("maxStack");
        spring = detail.Q<Toggle>("Spring");
        summer = detail.Q<Toggle>("Summer");
        autumn = detail.Q<Toggle>("Autumn");
        winter = detail.Q<Toggle>("Winter");

        icon.RegisterValueChangedCallback(e =>
        {
            if (e.newValue == null)
            {
                iconPath = null;
                itemImage.style.backgroundImage = null;
            }
            else
            {
                iconPath = AssetDatabase.GetAssetPath(e.newValue);
                string guid = AssetDatabase.AssetPathToGUID(iconPath);
                AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
                AddressableAssetEntry entry = settings.FindAssetEntry(guid);
                if (entry != null)
                {
                    iconKey = entry.address; // Addressable Key
                }

                itemImage.style.backgroundImage = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath)?.texture;
            }
        });
        var addButton = root.Q<Button>("AddButton");
        addButton.clicked += OpenAddMenu;
        var saveButton = root.Q<Button>("SaveButton");
        saveButton.clicked += SaveItem;
        deleteButton = root.Q<Button>("DeleteButton");
        deleteButton.clicked += DeleteItem;
    }
    private void AddItem(Item item,bool color,string _iconPath)
    {
        var newItem = itemTemplate.CloneTree();
        newItem.style.backgroundColor = color ? new Color(56 / 256f, 56 / 256f, 56 / 256f) : new Color(47 / 256f, 47 / 256f, 47 / 256f);

        var avatar = newItem.Q<VisualElement>("ItemImage");
        if(AssetDatabase.LoadAssetAtPath<Sprite>(_iconPath)!=null)
        avatar.style.backgroundImage = AssetDatabase.LoadAssetAtPath<Sprite>(_iconPath).texture;

        newItem.Q<Label>("ItemName").text = item.name;

        newItem.RegisterCallback<MouseUpEvent>(e=>OpenItemDetail(item,_iconPath));//触发打开详细界面

        itemList.contentContainer.Add(newItem);
    }
    private void RefreshList()
    {
        itemList.contentContainer.Clear();
        itemDatabase = ItemDataManager.Load();
        itemDatabase.items.Sort((a, b) => a.id.CompareTo(b.id));
        bool color = true;
        foreach (var item in itemDatabase.items)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            foreach (var group in settings.groups)
            {
                foreach (var entry in group.entries)
                {
                    if (entry.address == item.iconKey)
                    {
                        iconPath = AssetDatabase.GUIDToAssetPath(entry.guid);
                    }
                }
            }
            AddItem(item, color, iconPath);
            if (color)color = false;
            else color = true;
        }
    }

    private void OpenItemDetail(Item item,string _iconPath)
    {
        isUpdate = true;
        detail.style.display = DisplayStyle.None;
        deleteButton.style.display = DisplayStyle.None;
        curId = item.id;
        id.value = item.id;
        Itemname.value = item.name;
        price.value = item.price;
        type.value = item.type;
        if (AssetDatabase.LoadAssetAtPath<Sprite>(_iconPath) != null)
        {
            var _image = AssetDatabase.LoadAssetAtPath<Sprite>(_iconPath).texture;
            icon.value = _image;
            itemImage.style.backgroundImage = _image;
        }
        description.value = item.description;
        canUse.value = item.canUse;
        canCustomized.value = item.canCustomized;
        canEat.value = item.canEat;
        maxStack.value = item.maxStack;
        spring.value = item.canUsedinseason[0];
        summer.value = item.canUsedinseason[1];
        autumn.value = item.canUsedinseason[2];
        winter.value = item.canUsedinseason[3];
        detail.style.display = DisplayStyle.Flex;
        deleteButton.style.display = DisplayStyle.Flex;
    }

    private void OpenAddMenu()
    {
        isUpdate = false;
        detail.style.display = DisplayStyle.None;
        deleteButton.style.display = DisplayStyle.None;
        itemImage.style.backgroundImage = null;
        id.value = 0;
        Itemname.value = null;
        price.value = 0;
        if (icon != null)
        {
            icon.value = null;
        }
        type.value = ItemEnum.Weapon;
        description.value = null;
        canUse.value = false;
        canCustomized.value = false;
        canEat.value = false;
        maxStack.value = 1;
        spring.value = true;
        summer.value = true;
        autumn.value = true;
        winter.value = true;
        detail.style.display = DisplayStyle.Flex;
    }
    private void SaveItem()
    {
        if (isUpdate)
        {
            if (EmptyChecker())
            {
                EditorUtility.DisplayDialog("保存失败", "信息未完整", "确定");
                return;
            }
            Item item = new Item();
            item.id = id.value;
            item.name = Itemname.value;
            item.description = description.value;
            item.price = price.value;
            item.iconKey = iconKey;
            item.type = (ItemEnum)type.value;
            item.canUse = canUse.value;
            item.canCustomized = canCustomized.value;
            item.canEat = canEat.value;
            item.maxStack = maxStack.value;
            item.canUsedinseason = new List<bool>();
            item.canUsedinseason.Add(spring.value);
            item.canUsedinseason.Add(summer.value);
            item.canUsedinseason.Add(autumn.value);
            item.canUsedinseason.Add(winter.value);
            int index = itemDatabase.items.FindIndex(i => i.id == curId);
            if(index != -1)
            {
                itemDatabase.items[index] = item;
            }
            itemDatabase.items.Sort((a, b) => a.id.CompareTo(b.id));
            ItemDataManager.Save(itemDatabase);
            RefreshList();
        }
        else
        {
            if (itemDatabase.items.Exists(item => item.id == id.value)||id.value==0)
            {
                EditorUtility.DisplayDialog("保存失败","非法id","确定");
                return;
            }
            if(EmptyChecker())
            {
                EditorUtility.DisplayDialog("保存失败", "信息未完整", "确定");
                return;
            }
            Item item = new Item();
            item.id = id.value;
            item.name = Itemname.value;
            item.description = description.value;
            item.price = price.value;
            item.iconKey = iconKey;
            item.type = (ItemEnum)type.value;
            item.canUse = canUse.value;
            item.canCustomized = canCustomized.value;
            item.canEat = canEat.value;
            item.maxStack = maxStack.value;
            item.canUsedinseason = new List<bool>();
            item.canUsedinseason.Add(spring.value);
            item.canUsedinseason.Add(summer.value);
            item.canUsedinseason.Add(autumn.value);
            item.canUsedinseason.Add(winter.value);
            itemDatabase.items.Add(item);
            itemDatabase.items.Sort((a,b)=>a.id.CompareTo(b.id));
            ItemDataManager.Save(itemDatabase);
            RefreshList();
        }
    }
    private bool EmptyChecker()
    {
        if (string.IsNullOrWhiteSpace(Itemname.value) || string.IsNullOrWhiteSpace(description.value) || iconPath == null)
        {
            return true;
        }
        return false;
    }
    private void DeleteItem()
    {
        itemDatabase.items.Remove(itemDatabase.items.Find(i=>i.id == curId));
        ItemDataManager.Save(itemDatabase);
        RefreshList();
        deleteButton.style.display = DisplayStyle.None;
        detail.style.display = DisplayStyle.None;
    }
}
