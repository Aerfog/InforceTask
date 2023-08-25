using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace InforceTask.TagHelpers.Base;

public abstract class ItemLinkTagHelperBase : TagHelper
{
    protected readonly IUrlHelper UrlHelper;
    public int? ItemId { get; set; }
    
    protected ItemLinkTagHelperBase(IActionContextAccessor contextAccessor,
        IUrlHelperFactory urlHelperFactory)
    {
        UrlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
    }
    
    protected void BuildContent(TagHelperOutput output, string actionName,
        string className, string displayText, string fontAwesomeName)
    {
        output.TagName = "a";
        var target = (ItemId.HasValue)
            ? UrlHelper.Action(actionName, "Urls", new {id = ItemId})
            : UrlHelper.Action(actionName, "Urls");
        output.Attributes.SetAttribute("href", target);
        output.Attributes.Add("class",className);
        output.Content.AppendHtml($@"{displayText}<i class=""fas fa-{fontAwesomeName}""></i>");
    }
}