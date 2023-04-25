using HtmlAgilityPack;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.Factory
{
    public static class ResultCategories
    {
        public static string GenerateCategory(HtmlDocument htmlDocument)
        {
            string category = "Videos";
            if (htmlDocument.GetElementbyId("ImgCategory") != null)
            {
                category = htmlDocument.GetElementbyId("ImgCategory").GetAttributeValue("title", "");
            }
            return category;
        }
    }
}
