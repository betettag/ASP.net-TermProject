#pragma checksum "C:\Users\Admin\Desktop\cs295n\ASP.net-TermProject\TermProject\TermProject\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2c0e1ab9b10024e113ff57d399cf0619ac6f6f1f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(AspNetCore.Views_Home_Index))]
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
#line 1 "C:\Users\Admin\Desktop\cs295n\ASP.net-TermProject\TermProject\TermProject\Views\_ViewImports.cshtml"
using TermProject;

#line default
#line hidden
#line 2 "C:\Users\Admin\Desktop\cs295n\ASP.net-TermProject\TermProject\TermProject\Views\_ViewImports.cshtml"
using TermProject.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2c0e1ab9b10024e113ff57d399cf0619ac6f6f1f", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b7b02c88ad269961eab07a9fb03adffde11086d3", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<Player>
    {
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
            BeginContext(0, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\Users\Admin\Desktop\cs295n\ASP.net-TermProject\TermProject\TermProject\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";
    ViewData["Player"]= Model;
    ViewData["Username"]= Model.Username;
    ViewData["Score"] = Model.Score;
    ViewData["BootStrap"] = true;

#line default
#line hidden
            BeginContext(211, 8, true);
            WriteLiteral("<html>\r\n");
            EndContext();
            BeginContext(219, 17, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "2c0e1ab9b10024e113ff57d399cf0619ac6f6f1f3814", async() => {
                BeginContext(225, 4, true);
                WriteLiteral("\r\n\r\n");
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
            BeginContext(236, 2, true);
            WriteLiteral("\r\n");
            EndContext();
            BeginContext(238, 2856, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "2c0e1ab9b10024e113ff57d399cf0619ac6f6f1f5000", async() => {
                BeginContext(244, 1129, true);
                WriteLiteral(@"
    <div class=""text-center jumbotron"" style=""border-radius: 30px; background: black;"">
        <h1 class=""display-4 text-white"">Welcome to Internet Against Humanity</h1>
        <p class=""text-white"">
            The site is a community game of cards against humanity which is akin to apples to apples.
            There are users and each user gets a hand (5 cards), and a prompt to “answer to”.
            The prompt is sometimes a question but it does not have to be.
            The user has to try to answer or give the best fitting or funniest card in order to
            hopefully beat another user who had the same prompt and answers a different card.
            The way the scores are settled is by voting.
        </p>
    </div>
    <div class=""card-deck"" style=""display: flex;"">
        <div class=""card bg-dark text-center"" style=""width: 18rem;"">
            <!--<img class=""card-img-top"" src=""..."" alt=""Card image cap"">-->
            <div class=""card-body text-white"">
                <h5");
                WriteLiteral(" class=\"card-title\">Stats</h5>\r\n                <p class=\"card-text\">\r\n                    Player Count: ");
                EndContext();
                BeginContext(1374, 19, false);
#line 32 "C:\Users\Admin\Desktop\cs295n\ASP.net-TermProject\TermProject\TermProject\Views\Home\Index.cshtml"
                             Write(ViewBag.playerCount);

#line default
#line hidden
                EndContext();
                BeginContext(1393, 44, true);
                WriteLiteral("<br />\r\n                    Newest Player:\r\n");
                EndContext();
#line 34 "C:\Users\Admin\Desktop\cs295n\ASP.net-TermProject\TermProject\TermProject\Views\Home\Index.cshtml"
                     if (ViewBag.playerNew == "Guest")
                    {
                        ViewBag.playerNew = "Nobody :<";
                    }

#line default
#line hidden
                BeginContext(1597, 20, true);
                WriteLiteral("                    ");
                EndContext();
                BeginContext(1618, 17, false);
#line 38 "C:\Users\Admin\Desktop\cs295n\ASP.net-TermProject\TermProject\TermProject\Views\Home\Index.cshtml"
               Write(ViewBag.playerNew);

#line default
#line hidden
                EndContext();
                BeginContext(1635, 35, true);
                WriteLiteral("<br />\r\n                    Duels: ");
                EndContext();
                BeginContext(1671, 18, false);
#line 39 "C:\Users\Admin\Desktop\cs295n\ASP.net-TermProject\TermProject\TermProject\Views\Home\Index.cshtml"
                      Write(ViewBag.duelsCount);

#line default
#line hidden
                EndContext();
                BeginContext(1689, 64, true);
                WriteLiteral("\r\n                </p>\r\n                <a class=\"btn btn-black\"");
                EndContext();
                BeginWriteAttribute("href", " href=\"", 1753, "\"", 1813, 2);
#line 41 "C:\Users\Admin\Desktop\cs295n\ASP.net-TermProject\TermProject\TermProject\Views\Home\Index.cshtml"
WriteAttributeValue("", 1760, Url.Action("HighScores","Home", ViewData["Player"]), 1760, 52, false);

#line default
#line hidden
                WriteAttributeValue("", 1812, ";", 1812, 1, true);
                EndWriteAttribute();
                BeginContext(1814, 1273, true);
                WriteLiteral(@">See the High Scores!</a>
                <!--<p class=""card-text""><small class=""text-muted"">Last updated 3 mins ago</small></p>-->
            </div>
        </div>
        <div class=""card text-center"" style=""width: 18rem;"">
            <!--<img class=""card-img-top"" src=""..."" alt=""Card image cap"">-->
            <div class=""card-body"">
                <h5 class=""card-title"">Card title</h5>
                <p class=""card-text"">This card has supporting text below as a natural lead-in to additional content.</p>
                <!--<p class=""card-text""><small class=""text-muted"">Last updated 3 mins ago</small></p>-->
            </div>
        </div>
        <div class=""card text-center"" style=""width: 18rem;"">
            <!--<img class=""card-img-top"" src=""..."" alt=""Card image cap"">-->
            <div class=""card-body"">
                <h5 class=""card-title"">Card title</h5>
                <p class=""card-text"">This is a wider card with supporting text below as a natural lead-in to additional con");
                WriteLiteral("tent. This card has even longer content than the first to show that equal height action.</p>\r\n                <!--<p class=\"card-text\"><small class=\"text-muted\">Last updated 3 mins ago</small></p>-->\r\n            </div>\r\n        </div>\r\n    </div>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3094, 9, true);
            WriteLiteral("\r\n</html>");
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
