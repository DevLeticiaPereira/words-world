using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class WordManager : Singleton<WordManager>
{
	[SerializeField] private string googleSheetId = "1RY9GYaMqRN-Yp4xQZqm0e-NdsvleIiSkHa5zWCK9Dn8";

	private string[] sheetData;

	private void Start()
	{
		StartCoroutine(DownloadGoogleSheet());
	}

	private IEnumerator DownloadGoogleSheet()
	{
		string url = "https://docs.google.com/spreadsheets/d/" + googleSheetId + "/export?format=csv";

		UnityWebRequest request = UnityWebRequest.Get(url);
		yield return request.SendWebRequest();

		if (request.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("Failed to download Google Sheet. Error: " + request.error);
			yield break;
		}

		string text = request.downloadHandler.text;

		sheetData = text.Split("\r\n");
	}

	private string[] GetSheetData()
	{
		return sheetData;
	}

	public bool IsWorldValid(string word)
	{
		if (string.IsNullOrEmpty(word) && sheetData.Length == 0)
		{
			return false;
		}

		int min = 0;
		int max = sheetData.Length - 1;

		while (min <= max)
		{
			int index = (min + max)/2;

			int result = string.Compare(word, sheetData[index], StringComparison.OrdinalIgnoreCase);

			if (result == 0)
			{
				return true;
			}
			if (result < 0)
			{
				max = index - 1;
			}
			else
			{
				min = index + 1;
			}
		}
		return false;
	}
}
