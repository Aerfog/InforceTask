using InforceTask.Controllers;
using InforceTask.TagHelpers.Base;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace InforceTask.TagHelpers;

public class ItemListTagHelper : ItemLinkTagHelperBase
{
    public ItemListTagHelper(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory) : base(contextAccessor, urlHelperFactory)
    {
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        BuildContent(output,nameof(UrlsController.UrlsTable), "text-default","Back to List","list");
    }
}