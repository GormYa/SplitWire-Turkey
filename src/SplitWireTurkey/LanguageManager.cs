using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SplitWireTurkey
{
    /// <summary>
    /// Dil yönetimi için sınıf
    /// </summary>
    public static class LanguageManager
    {
        private static Dictionary<string, object> _currentTranslations = new Dictionary<string, object>();
        private static string _currentLanguage = "TR";

        /// <summary>
        /// Mevcut dil
        /// </summary>
        public static string CurrentLanguage => _currentLanguage;

        /// <summary>
        /// Dil dosyasını yükler
        /// </summary>
        public static bool LoadLanguage(string languageCode)
        {
            try
            {
                _currentLanguage = languageCode;
                
                var languagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "res", "Languages", $"{languageCode.ToLower()}.json");
                
                if (!File.Exists(languagePath))
                {
                    // Fallback olarak TR dilini dene
                    if (languageCode != "TR")
                    {
                        languagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "res", "Languages", "tr.json");
                    }
                    
                    if (!File.Exists(languagePath))
                    {
                        return false;
                    }
                }

                var jsonContent = File.ReadAllText(languagePath);
                _currentTranslations = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonContent);
                
                return _currentTranslations != null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Dil yüklenirken hata: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Çeviri metnini alır
        /// </summary>
        public static string GetText(string key, params object[] args)
        {
            try
            {
                if (_currentTranslations == null || !_currentTranslations.ContainsKey(key))
                {
                    return key; // Anahtar bulunamazsa anahtarı döndür
                }

                var value = _currentTranslations[key];
                
                if (value is JsonElement element)
                {
                    var text = element.GetString();
                    if (string.IsNullOrEmpty(text))
                    {
                        return key;
                    }

                    // String.Format benzeri işlem
                    if (args != null && args.Length > 0)
                    {
                        try
                        {
                            return string.Format(text, args);
                        }
                        catch
                        {
                            return text;
                        }
                    }

                    return text;
                }

                return key;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Çeviri alınırken hata: {ex.Message}");
                return key;
            }
        }

        /// <summary>
        /// İç içe geçmiş çeviri anahtarından metin alır (örn: "tabs.main")
        /// </summary>
        public static string GetText(string category, string key, params object[] args)
        {
            try
            {
                if (_currentTranslations == null || !_currentTranslations.ContainsKey(category))
                {
                    return $"{category}.{key}";
                }

                var categoryValue = _currentTranslations[category];
                if (categoryValue is JsonElement categoryElement)
                {
                    var categoryDict = JsonSerializer.Deserialize<Dictionary<string, object>>(categoryElement.GetRawText());
                    if (categoryDict != null && categoryDict.ContainsKey(key))
                    {
                        var value = categoryDict[key];
                        if (value is JsonElement element)
                        {
                            var text = element.GetString();
                            if (string.IsNullOrEmpty(text))
                            {
                                return $"{category}.{key}";
                            }

                            // String.Format benzeri işlem
                            if (args != null && args.Length > 0)
                            {
                                try
                                {
                                    return string.Format(text, args);
                                }
                                catch
                                {
                                    return text;
                                }
                            }

                            return text;
                        }
                    }
                }

                return $"{category}.{key}";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"İç içe çeviri alınırken hata: {ex.Message}");
                return $"{category}.{key}";
            }
        }
    }
}
