using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace CourseRegisterSystem.Models;
using System.ComponentModel.DataAnnotations;

public class Config
{

    public Config()
    {
        
    }
    
    public Config(long termId, DateTime startDate, DateTime expiryDate)
    {
        TermId = termId;
        StartDate = startDate;
        ExpiryDate = expiryDate;
    }
    
    [Display(Name = "Kỳ học")]
    public long TermId { get; set; }
    
    [Display(Name = "Ngày bắt đầu")]
    public DateTime StartDate { get; set; }
    
    [Display(Name = "Ngày kết thúc")]
    public DateTime ExpiryDate { get; set; }

    public void SaveConfig()
    {
        var appSettingsPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "appsettings.json");
        var json = File.ReadAllText(appSettingsPath);
        
        var jsonSettings = new JsonSerializerSettings();
        jsonSettings.Converters.Add(new ExpandoObjectConverter());
        jsonSettings.Converters.Add(new StringEnumConverter());
        
        dynamic config = JsonConvert.DeserializeObject<ExpandoObject>(json, jsonSettings);

        config.AppConfig.TermId = TermId.ToString();
        config.AppConfig.StartDate = StartDate.ToString("yyyy-MM-dd HH:mm:ss");
        config.AppConfig.ExpiryDate = ExpiryDate.ToString("yyyy-MM-dd HH:mm:ss");

        var newJson = JsonConvert.SerializeObject(config, Formatting.Indented, jsonSettings);
        
        File.WriteAllText(appSettingsPath, newJson);
    }
}