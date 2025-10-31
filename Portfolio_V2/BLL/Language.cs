namespace Portfolio_V2.BLL
{
    public static class Language
    {
        public const string Portuguese = "pt";
        public const string English = "en";

        public static string FromHeaderOrQuery(HttpRequest req)
        {
            if (req.Query.TryGetValue("lang", out var q))
            {
                var v = q.ToString().ToLowerInvariant();
                if (v.StartsWith(English)) return English;
                return Portuguese;
            }
            var accept = req.Headers.AcceptLanguage.ToString();
            if (!string.IsNullOrWhiteSpace(accept) && accept.ToLowerInvariant().Contains(English)) return English;
            return Portuguese;
        }
    }
}


