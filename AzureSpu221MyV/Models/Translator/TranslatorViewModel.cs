using AzureSpu221MyV.Models.Languages;

namespace AzureSpu221MyV.Models.Translator
{
    public class TranslatorViewModel
    {
        public TranslatorFormModel? FormModel { get; set; }
        public TranslatorResponse? TranslatorResponse { get; set; }
        public LanguageResponse? LanguageResponse { get; set; }
    }
}
