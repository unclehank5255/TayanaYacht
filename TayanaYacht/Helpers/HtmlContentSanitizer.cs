using Ganss.Xss;

namespace TayanaYacht.Helpers
{
    /// <summary>
    /// 清理後台 CKEditor 輸入的 HTML：移除 script 等危險內容，保留前台樣式需要的 class。
    /// </summary>
    public static class HtmlContentSanitizer
    {
        public static string Sanitize(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }

            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Add("class");
            return sanitizer.Sanitize(html);
        }
    }
}
