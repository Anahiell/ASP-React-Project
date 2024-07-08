using Microsoft.AspNetCore.Authentication;

namespace AzureSpu221MyV.Models.Languages
{
    public class LanguageData
    {
        public String name {  get; set; }
        public String nativeName { get; set; }
        public String? dir {  get; set; }
    }
}
