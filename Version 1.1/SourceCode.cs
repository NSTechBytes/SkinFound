using System;
using System.IO;
using System.Runtime.InteropServices;
using Rainmeter;

namespace PluginSkinFound
{
    public class Measure
    {
        private string[] skinNames;
        private string skinsFolderPath;
        private string outputFormat = "Whole";

        public void Reload(API api)
        {
            string skinNamesSetting = api.ReadString("SkinName", "").Trim();
            outputFormat = api.ReadString("Output", "Whole").Trim();

            // Retrieve the Skins directory path from the #SKINSPATH# variable
            skinsFolderPath = api.ReplaceVariables("#SKINSPATH#").Trim();

            if (string.IsNullOrEmpty(skinNamesSetting))
            {
                api.Log(API.LogType.Error, "SkinFound.dll: No SkinName provided.");
                skinNames = Array.Empty<string>();
            }
            else
            {
                // Split skin names and remove any extra spaces around them
                skinNames = skinNamesSetting.Split('|');
                for (int i = 0; i < skinNames.Length; i++)
                {
                    skinNames[i] = skinNames[i].Trim();
                }
            }
        }

        public string CheckSkinsExist(API api)
        {
            if (skinNames == null || skinNames.Length == 0)
            {
                return "No skins specified.";
            }

            string result = "";
            int availableSkinsCount = 0;  // Counter for the "SumSkin" output format

            foreach (string skin in skinNames)
            {
                string skinPath = Path.Combine(skinsFolderPath, skin);
                bool skinExists = Directory.Exists(skinPath);

                if (outputFormat.Equals("NameOnly", StringComparison.OrdinalIgnoreCase))
                {
                    if (skinExists)
                    {
                        result += $"{skin}\n";
                        api.Log(API.LogType.Debug, $"SkinFound.dll: {skin} is available.");
                    }
                }
                else if (outputFormat.Equals("Whole", StringComparison.OrdinalIgnoreCase))
                {
                    result += $"{skin}: {(skinExists ? 1 : 0)}\n";
                    api.Log(API.LogType.Debug, $"SkinFound.dll: {skin} is {(skinExists ? "available" : "NOT available")}.");
                }
                else if (outputFormat.Equals("SumSkin", StringComparison.OrdinalIgnoreCase))
                {
                    if (skinExists)
                    {
                        availableSkinsCount++;
                    }
                }
            }

            // If output format is "SumSkin", return the count of available skins
            if (outputFormat.Equals("SumSkin", StringComparison.OrdinalIgnoreCase))
            {
                result = $"{availableSkinsCount}";
            }

            return result.TrimEnd('\n');
        }
    }

    public static class Plugin
    {
        static IntPtr stringBuffer = IntPtr.Zero;

        [DllExport]
        public static void Initialize(ref IntPtr data, IntPtr rm)
        {
            data = GCHandle.ToIntPtr(GCHandle.Alloc(new Measure()));
        }

        [DllExport]
        public static void Finalize(IntPtr data)
        {
            if (data != IntPtr.Zero)
            {
                GCHandle.FromIntPtr(data).Free();
            }

            if (stringBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(stringBuffer);
                stringBuffer = IntPtr.Zero;
            }
        }

        [DllExport]
        public static void Reload(IntPtr data, IntPtr rm, ref double maxValue)
        {
            var measure = (Measure)GCHandle.FromIntPtr(data).Target;
            var api = new API(rm);
            measure.Reload(api);
        }

        [DllExport]
        public static double Update(IntPtr data)
        {
            return 0.0;
        }

        [DllExport]
        public static IntPtr GetString(IntPtr data)
        {
            var measure = (Measure)GCHandle.FromIntPtr(data).Target;
            var api = new API(IntPtr.Zero); // Only used for logging purposes
            string result = measure.CheckSkinsExist(api);

            if (stringBuffer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(stringBuffer);
                stringBuffer = IntPtr.Zero;
            }

            stringBuffer = Marshal.StringToHGlobalUni(result);
            return stringBuffer;
        }
    }
}
