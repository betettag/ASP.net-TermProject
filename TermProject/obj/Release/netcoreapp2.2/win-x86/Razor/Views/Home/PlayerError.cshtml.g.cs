#pragma checksum "C:\Users\Admin\Desktop\cs295n\ASP.net-TermProject\stuff\stuff\TermProject\Views\Home\PlayerError.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2a99ee9ba07f7b59f3d210725b876530f1ee4db0"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_PlayerError), @"mvc.1.0.view", @"/Views/Home/PlayerError.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/PlayerError.cshtml", typeof(AspNetCore.Views_Home_PlayerError))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\Admin\Desktop\cs295n\ASP.net-TermProject\stuff\stuff\TermProject\Views\_ViewImports.cshtml"
using TermProject;

#line default
#line hidden
#line 2 "C:\Users\Admin\Desktop\cs295n\ASP.net-TermProject\stuff\stuff\TermProject\Views\_ViewImports.cshtml"
using TermProject.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2a99ee9ba07f7b59f3d210725b876530f1ee4db0", @"/Views/Home/PlayerError.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b7b02c88ad269961eab07a9fb03adffde11086d3", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_PlayerError : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Player>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("text-lg-center text-md-center text-xl-center text-sm-center"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "C:\Users\Admin\Desktop\cs295n\ASP.net-TermProject\stuff\stuff\TermProject\Views\Home\PlayerError.cshtml"
  
    ViewData["Title"] = "Guest User Error";
    ViewData["Player"] = Model;
    ViewData["Username"] = Model.Username;
    ViewData["Score"] = Model.Score;
    ViewData["BootStrap"] = true;

#line default
#line hidden
            BeginContext(218, 27, true);
            WriteLiteral("<!DOCTYPE html>\r\n\r\n<html>\r\n");
            EndContext();
            BeginContext(245, 106, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "2a99ee9ba07f7b59f3d210725b876530f1ee4db04194", async() => {
                BeginContext(251, 93, true);
                WriteLiteral("\r\n    <meta name=\"viewport\" content=\"width=device-width\" />\r\n    <title>PlayerError</title>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(351, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(353, 482, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "2a99ee9ba07f7b59f3d210725b876530f1ee4db05475", async() => {
                BeginContext(428, 191, true);
                WriteLiteral("\r\n    <div class=\"jumbotron\">\r\n        <h2>Currently a Guest User</h2>\r\n        Please login or Register by Creating a Duel and use the other features<br />\r\n        <a class=\"btn btn-action\"");
                EndContext();
                BeginWriteAttribute("href", " href=\"", 619, "\"", 676, 2);
#line 20 "C:\Users\Admin\Desktop\cs295n\ASP.net-TermProject\stuff\stuff\TermProject\Views\Home\PlayerError.cshtml"
WriteAttributeValue("", 626, Url.Action("NewDuel","Home", ViewData["Player"]), 626, 49, false);

#line default
#line hidden
                WriteAttributeValue("", 675, ";", 675, 1, true);
                EndWriteAttribute();
                BeginContext(677, 75, true);
                WriteLiteral(">Create a New Duel/Register</a><br/>\r\n            <a class=\"btn btn-action\"");
                EndContext();
                BeginWriteAttribute("href", " href=\"", 752, "\"", 807, 2);
#line 21 "C:\Users\Admin\Desktop\cs295n\ASP.net-TermProject\stuff\stuff\TermProject\Views\Home\PlayerError.cshtml"
WriteAttributeValue("", 759, Url.Action("Login","Home", ViewData["Player"]), 759, 47, false);

#line default
#line hidden
                WriteAttributeValue("", 806, ";", 806, 1, true);
                EndWriteAttribute();
                BeginContext(808, 20, true);
                WriteLiteral(">Login</a>\r\n</div>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(835, 11, true);
            WriteLiteral("\r\n</html>\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<Player> Html { get; private set; }
    }
}
#pragma warning restore 1591