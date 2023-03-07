using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Managers
{
	public class SaveManager  : Singleton<SaveManager>
	{
		public SaveData UserSaveData { get; private set; }

		[Serializable]
		public class SaveData
		{
			public int JourneyScore;
			public int LastCompletedLevel;
		}

		protected override void Awake()
		{
			base.Awake();
			UserSaveData = new ();
			if (!Directory.Exists(Application.persistentDataPath + "/UserSaveData/"))
				Directory.CreateDirectory(Application.persistentDataPath + "/UserSaveData/");
		}

		public void SaveGameData(int newJourneyScore, int lastCompletedLevel)
		{
			UserSaveData.JourneyScore = newJourneyScore;
			UserSaveData.LastCompletedLevel = lastCompletedLevel;
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(Application.persistentDataPath + "/UserSaveData/userData.dat", FileMode.Create);
			formatter.Serialize(stream, UserSaveData);
			stream.Close();
		}

		public SaveData LoadSaveData()
		{
			if (File.Exists(Application.persistentDataPath + "/UserSaveData/userData.dat"))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				FileStream stream = new FileStream(Application.persistentDataPath + "/UserSaveData/userData.dat", FileMode.Open);
				UserSaveData = formatter.Deserialize(stream) as SaveData;
				stream.Close();
			}
			return UserSaveData;
		}
	}
}
