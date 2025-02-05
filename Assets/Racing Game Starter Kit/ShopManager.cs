using RGSK;
using UnityEngine;
using UnityEngine.Purchasing; // Подключаем Unity IAP
using UnityEngine.UI;
using UnityEngine.Purchasing.Extension;

public class ShopManager : MonoBehaviour, IDetailedStoreListener
{
    private IStoreController storeController;

    void Start()
    {
        if (storeController == null)
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            builder.AddProduct("cash_1", ProductType.Consumable);
            builder.AddProduct("cash_2", ProductType.Consumable);
            builder.AddProduct("cash_3", ProductType.Consumable);
            builder.AddProduct("cash_4", ProductType.Consumable);
            builder.AddProduct("cash_5", ProductType.Consumable);

            UnityPurchasing.Initialize(this, builder);
        }
    }

    public void BuyProduct(string productId)
    {
        if (storeController != null && storeController.products.WithID(productId) != null)
        {
            storeController.InitiatePurchase(productId);
        }
        else
        {
            Debug.LogError("Ошибка покупки: продукт не найден!");
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"Ошибка инициализации IAP: {error}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"Ошибка инициализации IAP: {error}, {message}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        switch (args.purchasedProduct.definition.id)
        {
            case "cash_1":
                PlayerData.instance.AddCurrency(50000);
                break;
            case "cash_2":
                PlayerData.instance.AddCurrency(100000);
                break;
            case "cash_3":
                PlayerData.instance.AddCurrency(300000);
                break;
            case "cash_4":
                PlayerData.instance.AddCurrency(500000);
                break;
            case "cash_5":
                PlayerData.instance.AddCurrency(1000000);
                break;
        }

        FindObjectOfType<PlayerSettings>()?.UpdateUIToMatchSettings();
        Debug.Log($"Покупка завершена: {args.purchasedProduct.definition.id}");

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.LogError($"Покупка не удалась: {product.definition.id}, Причина: {reason}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.LogError($"Покупка не удалась: {product.definition.id}, Причина: {failureDescription.reason}, Детали: {failureDescription.message}");
    }
}