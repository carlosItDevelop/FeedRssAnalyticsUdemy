using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Cooperchip.FeedRssBlogsAnalyticsApi.Configurations.FiltersAndAttibutes
{
    public class GlobalProducesResponseTypeAttibutes : ProducesResponseTypeAttribute
    {
        public GlobalProducesResponseTypeAttibutes(Type type, int statusCode) : base(type, statusCode)
        {
            if (statusCode == 200 || statusCode == 201 || statusCode == 204)
            {
                ContentType = "application/json";
            }
        }

        public string? ContentType { get; set; }

    }

}
