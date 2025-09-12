using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public GameObject goodPrefab;
    public Transform content;
    public ScrollRect scrollRect;
    public GameObject gold;
    private void OnEnable()
    {
        StartCoroutine(ResetScroll());
    }
    private void OnDisable()
    {
        gold.SetActive(false);
    }
    private void Update()
    {
        int money = GeneralDataLoader.instance.database.database.gold;
        gold.transform.Find("GoldText").GetComponent<TMP_Text>().text = $"{money}";
    }
    public void OpenShop(List<Goods> list)
    {
        foreach (var good in list)
        {
            var obj=Instantiate(goodPrefab,content);
            obj.GetComponent<GoodSlot>().Initialized(good);
        }
        gold.SetActive(true);
    }
    private System.Collections.IEnumerator ResetScroll()
    {
        yield return null; // �ȴ� 1 ֡���� UI �������
        scrollRect.verticalNormalizedPosition = 1f; // 1 = ������0 = �ײ�
    }
}
