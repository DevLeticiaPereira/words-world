using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelSetup))]
public class LevelSetupEditor : Editor
{
	public override void OnInspectorGUI()
	{
		LevelSetup levelSetup = (LevelSetup)target;

		EditorGUILayout.LabelField("Crosswords Grid Settings", EditorStyles.boldLabel);

		int newRows = EditorGUILayout.IntField("Number of Rows", levelSetup.Rows);
		int newColumns = EditorGUILayout.IntField("Number of Columns", levelSetup.Columns);

		if (newRows != levelSetup.Rows || newColumns != levelSetup.Columns)
		{
			levelSetup.ResizeArray(newRows, newColumns);
		}

		if (GUILayout.Button("Create Grid"))
		{
			levelSetup.InitializeArray();
		}

		EditorGUILayout.Space();

		if (levelSetup.CrosswordSetup != null)
		{
			EditorGUILayout.LabelField("Grid: ", EditorStyles.boldLabel);

			for (int i = 0; i < levelSetup.Rows; i++)
			{
				EditorGUILayout.BeginHorizontal();

				for (int j = 0; j < levelSetup.Columns; j++)
				{
					string charString = EditorGUILayout.TextField(levelSetup.CrosswordSetup[i, j].ToString(), GUILayout.MaxWidth(20));

					if (GUILayout.Button("X", GUILayout.Width(20)))
					{
						charString = "";
						levelSetup.CrosswordSetup[i, j] = '\0';
					}

					if (charString.Length > 0)
					{
						levelSetup.CrosswordSetup[i, j] = charString[0];
					}
				}

				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.Space();
		}
		EditorUtility.SetDirty(target);
	}
}
