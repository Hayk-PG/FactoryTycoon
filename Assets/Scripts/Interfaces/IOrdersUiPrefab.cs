using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IOrdersUiPrefab 
{
    int OrderNumber { get;}
    float OrderQuantity { get; set; }
    float OrderPrice { get; set; }
    float OrderTime { get; set; }
    string OrderShortDescription { get; set; }
    string OrderDescription { get; set; }
    Text Text { get; set; }

    Button DetailsButton { get; set; }
    Button YesButton { get; set; }
    Button NoButton { get; set; }

    void OrderInfoTab(float OrderQuantity, float OrderPrice, float OrderTime);

    void Details();

    void TakeTheOrder();

    void RejectTheOrder();
}
