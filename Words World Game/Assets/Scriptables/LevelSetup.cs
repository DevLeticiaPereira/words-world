using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class LevelSetup : ScriptableObject
{
	public int Rows { get; private set; }
	public int Columns { get; private set; }

	public char[,] CrosswordSetup;

	public void InitializeArray()
	{
		CrosswordSetup = new char[Rows, Columns];
	}

	public void ResizeArray(int newRows, int newColumns)
	{
		char[,] newArray = new char[newRows, newColumns];

		for (int i = 0; i < Mathf.Min(Rows, newRows); i++)
		{
			for (int j = 0; j < Mathf.Min(Columns, newColumns); j++)
			{
				newArray[i, j] = CrosswordSetup[i, j];
			}
		}

		Rows = newRows;
		Columns = newColumns;
		CrosswordSetup = newArray;
	}

	public HashSet<char> GetLetterInLevel()
	{
		HashSet<char> levelLetters = new HashSet<char>();
		foreach (var letter in CrosswordSetup)
		{
			levelLetters.Add(letter);
		}
		return levelLetters;
	}
}
