using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Voxhull
{
    public class FileHandler : MonoBehaviour
    {
        [SerializeField] private string path = "";
        [SerializeField] private string persistentPath = "";
        [SerializeField] private string folder = "save";
        [SerializeField] private string fileName = "save";
        [SerializeField] private string fileExt = "json";
        private PlayerData playerData;

        // Start is called before the first frame update
        void Start()
        {
            SetPaths();
        }


        private void SetPaths()
        {
            path = Application.dataPath +
                Path.AltDirectorySeparatorChar +
                folder +
                Path.AltDirectorySeparatorChar +
                fileName + "." + fileExt;
            persistentPath = Application.persistentDataPath + 
                Path.AltDirectorySeparatorChar +
                folder +
                Path.AltDirectorySeparatorChar +
                fileName + "." + fileExt;
        }

        public void SetPlayerData(PlayerData playerData)
        {
            this.playerData = playerData;
        }

        public void Quicksave(InputAction.CallbackContext context)
        {
            Save();
        }

        public void Quickload(InputAction.CallbackContext context)
        {
            Load();
        }

        public void Save()
        {
            var savePath = persistentPath;
            Debug.Log("Saving data to: " + savePath);
            var json = JsonUtility.ToJson(playerData);
            Debug.Log(json);

            var writer = new StreamWriter(savePath);
            writer.Write(json);
        }

        public void Load()
        {
            var reader = new StreamReader(persistentPath);
            var json = reader.ReadToEnd();

            var data = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log($"{data}");
        }
    }
}