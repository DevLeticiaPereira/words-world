using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
	[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
	public class LevelSetup : ScriptableObject
	{
		[SerializeField] private int _gridRow;
		[SerializeField] private int _gridColumn;
		[SerializeField] private char[] _levelLetters;
		[SerializeField] private List<WordData> _wordDatas = new();

		public int GridRow => _gridRow;
		public int GridColumn => _gridColumn;
		public char[] LevelLetters => _levelLetters;
		public List<WordData> WordDatas => _wordDatas;
	}

	[Serializable]
	public struct WordData
	{
		public List<LetterData> Word;
	}

	[Serializable]
	public struct LetterData
	{
		public char Letter;
		public GridPosition LetterGridPosition;
	}

	[Serializable]
	public struct GridPosition
	{
		public int Row;
		public int Column;
	}
}
