using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrdersUIPrefabScript : MonoBehaviour, IOrdersUiPrefab
{
    [Header("UI")]
    [SerializeField] Text text;
    [SerializeField] Button details;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    /// <summary>
    /// Number of order
    /// </summary>
    public int OrderNumber {
        get {
            return transform.parent != null ? transform.GetSiblingIndex() : 0;
        }
    }
    /// <summary>
    /// Quantity of the order
    /// </summary>
    public float OrderQuantity { get; set; }
    /// <summary>
    /// Price of the order
    /// </summary>
    public float OrderPrice { get; set; }
    /// <summary>
    /// Time to complete the order
    /// </summary>
    public float OrderTime { get; set; }
    /// <summary>
    /// Short description of the order
    /// </summary>
    public string OrderShortDescription { get; set; }
    /// <summary>
    /// General description of the order
    /// </summary>
    public string OrderDescription { get; set; }
    /// <summary>
    /// UI text of OrdersUIPrefab
    /// </summary>
    public Text Text {
        get {
            return text;
        }
        set {
            text = value;
        }
    }
    /// <summary>
    /// Details button 
    /// </summary>
    public Button DetailsButton {
        get {
            return details;
        }
        set {
            details = value;
        }
    }
    /// <summary>
    /// Confirm button
    /// </summary>
    public Button YesButton {
        get {
            return yesButton;
        }
        set{
            yesButton = value;
        }
    }
    /// <summary>
    /// Reject button
    /// </summary>
    public Button NoButton {
        get {
            return noButton;
        }
        set{
            noButton = value;
        }
    }


    /// <summary>
    /// Sets order's info 
    /// </summary>
    /// <param name="OrderQuantity"></param>
    /// <param name="OrderPrice"></param>
    /// <param name="OrderTime"></param>
    public void OrderInfoTab(float OrderQuantity, float OrderPrice, float OrderTime) {

        this.OrderQuantity = OrderQuantity;
        this.OrderPrice = OrderPrice;
        this.OrderTime = OrderTime;

        Text.text = OrderNumber + ": " + "Order text: " + this.OrderQuantity + ": " + this.OrderPrice + ": " + this.OrderTime;
    }

    /// <summary>
    /// Order's details
    /// </summary>
    public void Details() {
        

    }

    public void TakeTheOrder() {
        

    }

    public void RejectTheOrder() {
        

    }








}
