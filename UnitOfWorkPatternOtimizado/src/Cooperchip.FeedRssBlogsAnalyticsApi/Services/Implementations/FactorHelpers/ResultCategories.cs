using HtmlAgilityPack;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Services.Implementations.FactorHelpers
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
