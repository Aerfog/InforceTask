using InforceTask.Controllers;
using InforceTask.TagHelpers.Base;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace InforceTask.TagHelpers;

public class ItemCreateTagHelper : ItemLinkTagHelperBase
{
    public ItemCreateTagHelper(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory) : base(contextAccessor, urlHelperFactory)
    {
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        BuildContent(output,nameof(UrlsController.Create), "text-success","Create new","plus");
    }
}