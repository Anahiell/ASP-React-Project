using Microsoft.AspNetCore.Mvc;

namespace AzureSpu221MyV.Models.Translator
{
    public class TranslatorFormModel
    {
        [FromForm(Name = "lang-from")]
        public String? LanguageFrom { get; set; }
        [FromForm(Name = "lang-to")]
        public String? LanguageTo { get; set; }
        [FromForm(Name = "original-text")]
        public String? OriginalText { get; set; }
        [FromForm(Name = "go-button")]
        public bool IsGoPressed { get; set; }
    }
}
