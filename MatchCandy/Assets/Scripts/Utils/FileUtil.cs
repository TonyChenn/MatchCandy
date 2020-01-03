using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Modules.UI
{
    public class FileUtils{

        /// <summary>
        /// 生成所有panel的Json
        /// </summary>
        /// <param name="folderPath">panel所在目录</param>
        /// <param name="jsonPath">所在目录</param>
        public static void CreatePanelsJsonFile(string uiPanelsPath,string jsonPath)
        {
            string Full_PanelPrefabsFolderPath = Application.dataPath + uiPanelsPath;
            string Full_JsonPath = jsonPath;

            DirectoryInfo fileInfos = new DirectoryInfo(Full_PanelPrefabsFolderPath);

            var panelList = new List<Panel>();

            foreach (var file in fileInfos.GetFiles("*.prefab"))
            {
                Panel panel = new Panel();
                panel.Name = Path.GetFileNameWithoutExtension(uiPanelsPath + "/" + file.Name);
                panel.Path = "Panel/" + panel.Name;
                panelList.Add(panel);
            }
            string json = LitJson.JsonMapper.ToJson(panelList);
            string fileName = Application.dataPath+ Full_JsonPath + "/panel.json";
            Debug.Log(fileName);
            WriteFile(fileName, json);

            Debug.Log("Panel Json生成成功");
            GeneratePanelEnumFile(uiPanelsPath, jsonPath);
        }

        /// <param name="PrefabfolderPath">panel所在目录</param>
        /// <param name="path">enum 枚举所在目录</param>
        public static void GeneratePanelEnumFile(string prefabPath, string path)
        {
            DirectoryInfo fileInfos = new DirectoryInfo(Application.dataPath + prefabPath);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("namespace Modules.UI");
            builder.AppendLine("{");
            builder.AppendLine("\tpublic enum UIType");
            builder.AppendLine("\t{");
            builder.AppendLine("\t\tNone,");
            foreach (var file in fileInfos.GetFiles("*.prefab"))
            {
                string fileName = file.Name.Replace(".prefab", "");
                builder.Append("\t\t" + fileName);
                builder.Append(",\n");
            }
            builder.AppendLine("\t}");
            builder.AppendLine("}");

            string str_path = Application.dataPath + path + "/UIType.cs";
            WriteFile(str_path, builder.ToString());

            Debug.Log("Panel Enum 生成成功");
        }

        public static string ReadFile(string path)
        {
            string json = "";
            if (File.Exists(path))
                json = File.ReadAllText(path);
            else
                Debug.LogError("json 路径不存在"+path);
            return json;
        }
        public static void WriteFile(string path,string content)
        {
            if (!File.Exists(path))
                File.Create(path).Dispose();
            File.WriteAllText(path, content);
        }

        /// <summary>
        /// 游戏关卡Json
        /// </summary>
        public static string GameLevelJson
        {
            get
            {
                return ReadFile(Application.persistentDataPath + "/level.json");
            }
            set
            {
                Debug.Log("写关卡文件" + Application.persistentDataPath);
                WriteFile(Application.persistentDataPath + "/level.json", value);
            }
        }
        /// <summary>
        /// 商品信息Json
        /// </summary>
        public static string GameGoodsJson
        {
            get
            {
                return ReadFile(Application.persistentDataPath + "/goods.json");
            }
            set
            {
                Debug.Log("写商品文件"+Application.persistentDataPath);
                WriteFile(Application.persistentDataPath + "/goods.json", value);
            }
        }
    }

    class Panel
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}

