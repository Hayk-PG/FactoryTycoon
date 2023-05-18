using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrdersController : MonoBehaviour
{
    [Header("PREFAB")]
    [SerializeField] OrdersUIPrefabScript ordersUIPrefab;

    [Header("TRANSFORM")]
    [SerializeField] Transform container;
    [SerializeField] Transform detailsScreen;

    [SerializeField] bool create;

    [HideInInspector]
    public List<OrdersUIPrefabScript> OrderTabsList = new List<OrdersUIPrefabScript>();



    void Update() {

        CreateOrdersTab();

        OnClickOrderTabsButtons(OrderTabsList != null);
    }


    public void CreateOrdersTab() {

        if (create) {
            OrdersUIPrefabScript ordersUIPrefabCopy = Instantiate(ordersUIPrefab, container);
            ordersUIPrefabCopy.OrderInfoTab(Random.Range(10, 1000), Random.Range(100, 10000), Random.Range(60, 180));
            OrderTabsList.Add(ordersUIPrefabCopy);
            create = false;
        }
    }

    void OnClickOrderTabsButtons(bool isOrderTabsListActive) {

        if (isOrderTabsListActive) {

            for (int o = 0; o < OrderTabsList.Count; o++) {

                OrderTabsList[0].DetailsButton.onClick.RemoveAllListeners();
                OrderTabsList[0].YesButton.onClick.RemoveAllListeners();
                OrderTabsList[0].NoButton.onClick.RemoveAllListeners();

                OrderTabsList[0].DetailsButton.onClick.AddListener(()=> { });
                OrderTabsList[0].YesButton.onClick.AddListener(() => { });
                OrderTabsList[0].NoButton.onClick.AddListener(() => { });
            }
        }
    }













}
