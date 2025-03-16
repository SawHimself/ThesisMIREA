using System.Text.Json;

namespace Services.SecuritySettings
{
    public class SecurityProvider : ISecurityProvider
    {
        private static readonly string JsonFilePath = "Settings.json";

        public static bool GetRule(string rule)
        {
            var rules = LoadRules();
            if(rules == null) 
            {
                //Console.WriteLine("Error");
                return false; 
            }
            return rules[rule];
        }

        public static bool UpdateRule(string ruleName, bool state)
        {
            var rules = LoadRules();
            if (rules == null) 
            {
                /*Console.WriteLine("Error");*/
                return false; 
            }

            if (rules.ContainsKey(ruleName))
            {
                rules[ruleName] = state;
            }
            else
            {
                rules.Add(ruleName, state);
            }

            return SaveRules(rules);
        }

        private static Dictionary<string, bool>? LoadRules()
        {
            try
            {
                if (!File.Exists(JsonFilePath)) return null;

                var json = File.ReadAllText(JsonFilePath);
                using var document = JsonDocument.Parse(json);
                var root = document.RootElement.GetProperty("Rule");

                var rules = new Dictionary<string, bool>();
                foreach (var property in root.EnumerateObject())
                {
                    rules[property.Name] = property.Value.GetBoolean();
                }

                return rules;
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error loading rules: {ex.Message}");
                return null;
            }
        }

        private static bool SaveRules(Dictionary<string, bool> rules)
        {
            try
            {
                var jsonObject = new Dictionary<string, object>
                {
                    { "Rule", rules }
                };

                var json = JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(JsonFilePath, json);
                return true;
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error saving rules: {ex.Message}");
                return false;
            }
        }

        // ISecurityProvider interface implementation
        bool ISecurityProvider.GetRule(string rule) => GetRule(rule);

        bool ISecurityProvider.UpdateRule(string ruleName, bool state) => UpdateRule(ruleName, state);
    }
}
