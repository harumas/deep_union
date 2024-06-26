using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    /// <summary>
    ///     タスクを自動生成するクラス
    /// </summary>
    internal static class TaskGenerator
    {
        private const string taskPath = @"Scripts\GameMain\Task";

        [MenuItem("GameObject/Task", priority = -100000)]
        private static void GenerateTask()
        {
            var dirPath = Path.Combine(Application.dataPath, taskPath);
            var fileName = CreateFileName();
            var filePath = Path.Combine(dirPath, fileName);
            var className = Path.ChangeExtension(fileName, null);

            using (var fs = File.Create(filePath))
            {
                using (var sw = new StreamWriter(fs))
                {
                    var code = CreateCode(className);
                    sw.Write(code);
                }
            }

            GeneratingFlagHolder.instance.IsGenerating = true;
            GeneratingFlagHolder.instance.ClassName = className;

            AssetDatabase.Refresh();
        }


        [InitializeOnLoadMethod]
        private static void OnCompileScripts()
        {
            if (!GeneratingFlagHolder.instance.IsGenerating)
            {
                return;
            }

            LayerMask taskLayer = LayerMask.NameToLayer("Task");

            // ReSharper disable once UseObjectOrCollectionInitializer
            var gameObject = new GameObject("Task");
            gameObject.tag = "Task";
            gameObject.layer = taskLayer;

            var detectableObject = new GameObject("DetectableArea");
            detectableObject.layer = taskLayer;
            detectableObject.transform.SetParent(gameObject.transform);

            detectableObject.AddComponent<SphereCollider>();
            gameObject.AddComponent(FindGeneratedType());

            GeneratingFlagHolder.instance.IsGenerating = false;
        }

        private static Type FindGeneratedType()
        {
            Type generatedType = null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.Name == GeneratingFlagHolder.instance.ClassName)
                    {
                        generatedType = type;
                        break;
                    }
                }

                if (generatedType != null)
                {
                    break;
                }
            }

            return generatedType;
        }

        private static string CreateFileName()
        {
            return $"Task_{Guid.NewGuid():N}.cs";
        }

        private static string CreateCode(string className)
        {
            return $@"using Module.Task;

namespace GameMain.Task
{{
    public class {className} : BaseTask
    {{
    }}
}}
";
        }
    }

    public class GeneratingFlagHolder : ScriptableSingleton<GeneratingFlagHolder>
    {
        private string className;

        private bool isGenerating;

        public bool IsGenerating
        {
            get => isGenerating;
            set
            {
                isGenerating = value;
                Save(false);
            }
        }

        public string ClassName
        {
            get => className;
            set
            {
                className = value;
                Save(false);
            }
        }
    }
}