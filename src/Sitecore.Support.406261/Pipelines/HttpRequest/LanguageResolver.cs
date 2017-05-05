namespace Sitecore.Support.Pipelines.HttpRequest
{
    using Diagnostics;
    using Globalization;
    using Sitecore.Pipelines.HttpRequest;
    using System.Web;

    public class LanguageResolver : HttpRequestProcessor
    {
        private Language ExtractLanguageFromQueryString(HttpRequest request)
        {
            Language language;
            string name = request.QueryString.Get("sc_lang");
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            if (!Language.TryParse(name, out language))
            {
                return null;
            }
            return language;
        }

        private Language GetLanguageFromRequest(HttpRequest request)
        {
            Language language = this.ExtractLanguageFromQueryString(request);
            if (language != null)
            {
                return language;
            }
            return Context.Data.FilePathLanguage;
        }

        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Language languageFromRequest = this.GetLanguageFromRequest(args.Context.Request);
            if (languageFromRequest != null)
            {
                Context.Language = languageFromRequest;
                Tracer.Info("Language changed to \"" + languageFromRequest.Name + "\" as the query string of the current request contains language (sc_lang).");
            }
        }
    }
}