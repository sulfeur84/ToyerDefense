using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour
{
	[SerializeField] private GameObject shopCards;
	[SerializeField] private GameObject cancelButton;

	public static Action DisplayShopCardA;
	public static Action HideShopCardA;

	private void Awake()
	{
		DisplayShopCardA = DisplayShopCard;
		HideShopCardA = HideShopCard;
	}

	private void DisplayShopCard()
	{
		shopCards.GetComponent<Animator>().SetBool("PanelOn", true);
		cancelButton.SetActive(false);
	}

	private void HideShopCard()
	{
		shopCards.GetComponent<Animator>().SetBool("PanelOn", false);
		cancelButton.SetActive(true);
	}
}
