using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeItem : MonoBehaviour
{
    [Header(header: "Images")]
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image knifeImage;

    [SerializeField] private GameObject selectedImage;

    [Header(header: "Colors")]
    [SerializeField] private Color knifeUnlockColor;
    [SerializeField] private Color knifeLockColor;
    [SerializeField] private Color knifeUnlockBackgroundColor;
    [SerializeField] private Color knifeLockBackgroundColor;

    [SerializeField] private int price;

    private Shop shop;
    private Knife knife;

    private const string KNIFE_UNCLOCKED = "KnifeUnlocked_";

    public int Index { get; set; }

    public bool IsUnlocked
    {
        get
        {
            if (Index==0)
            {
                return true;

            }

            return PlayerPrefs.GetInt(key: KNIFE_UNCLOCKED + Index, defaultValue: 0) == 1;
        }

        set
        {
            PlayerPrefs.SetInt(KNIFE_UNCLOCKED + Index, value ? 1 : 0);
        }
    }
    
    public bool IsSelected
    {
        get => selectedImage.activeSelf;
        set
        {
            if (value)
            {
                if (shop.KnifeItemSelected !=null)
                {
                    shop.KnifeItemSelected.IsSelected = false;
                }

                shop.KnifeItemSelected = this;
            }
            selectedImage.SetActive(value);
        }
    }

    public int Price => price;

    public Image KnifeImage => knifeImage;

    public void Setup(int index, Shop shop)
    {
        this.shop = shop;
        Index = index;
        knife = this.shop.knifeList[Index];
        KnifeImage.sprite = knife.GetComponent<SpriteRenderer>().sprite;
        UpdateUI();
    }

    public void OnClick()
    {
        if (IsUnlocked && IsSelected)
        {
            GeneralUI.Instance.CloseShop();
        }

        if (!IsSelected)
        {
            IsSelected = true;
        }

        if (IsUnlocked)
        {
            GameManager.Instance.SelectedKnife = Index;
        }
        shop.UpdateShopUI();
    }

    public void UpdateUI()
    {
        backgroundImage.color = IsUnlocked ? knifeUnlockBackgroundColor : knifeLockBackgroundColor;
        KnifeImage.GetComponent<Mask>().enabled = !IsUnlocked;

        KnifeImage.transform.GetChild(0).GetComponent<Image>().color = IsUnlocked ? knifeUnlockColor : knifeLockColor;
        KnifeImage.transform.GetChild(0).gameObject.SetActive(!IsUnlocked);
    }
}
