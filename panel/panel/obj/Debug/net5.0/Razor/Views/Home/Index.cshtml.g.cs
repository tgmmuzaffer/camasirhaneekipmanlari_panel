#pragma checksum "C:\Users\MONSTER\source\repos\camasirhaneekipmanlari_panel\panel\panel\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2ceea3c51b1f8d76c72946c85a1b25bb6b7f09cb"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
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
#nullable restore
#line 1 "C:\Users\MONSTER\source\repos\camasirhaneekipmanlari_panel\panel\panel\Views\_ViewImports.cshtml"
using panel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\MONSTER\source\repos\camasirhaneekipmanlari_panel\panel\panel\Views\Home\Index.cshtml"
using panel.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2ceea3c51b1f8d76c72946c85a1b25bb6b7f09cb", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a1ac34d65bb9f7065f48b2668cd46dafa8521c5a", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<Log>>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "C:\Users\MONSTER\source\repos\camasirhaneekipmanlari_panel\panel\panel\Views\Home\Index.cshtml"
  
    int i = 1;

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"container mt-3\">\r\n    <h1>Log</h1>\r\n");
            WriteLiteral("\r\n\r\n");
#nullable restore
#line 17 "C:\Users\MONSTER\source\repos\camasirhaneekipmanlari_panel\panel\panel\Views\Home\Index.cshtml"
       if (Model != null)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"            <div class=""card mb-4"">
                <div class=""card-header"">
                    <i class=""fas fa-table me-1""></i>
                    Loglar
                </div>
                <div class=""card-body"">
                    <table id=""datatablesSimple"">
                        <thead>
                            <tr>
                                <th>No</th>
                                <th>Kayıt</th>
                                <th>Tarih</th>

                            </tr>
                        </thead>
                        <tbody>
");
#nullable restore
#line 35 "C:\Users\MONSTER\source\repos\camasirhaneekipmanlari_panel\panel\panel\Views\Home\Index.cshtml"
                             foreach (var item in Model)
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <tr>\r\n                                    <td>");
#nullable restore
#line 38 "C:\Users\MONSTER\source\repos\camasirhaneekipmanlari_panel\panel\panel\Views\Home\Index.cshtml"
                                   Write(i);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                    <td>");
#nullable restore
#line 39 "C:\Users\MONSTER\source\repos\camasirhaneekipmanlari_panel\panel\panel\Views\Home\Index.cshtml"
                                   Write(item.Message);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                    <td>");
#nullable restore
#line 40 "C:\Users\MONSTER\source\repos\camasirhaneekipmanlari_panel\panel\panel\Views\Home\Index.cshtml"
                                   Write(item.TimeStamp);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                </tr>\r\n");
#nullable restore
#line 42 "C:\Users\MONSTER\source\repos\camasirhaneekipmanlari_panel\panel\panel\Views\Home\Index.cshtml"
                                i++;
                            }

#line default
#line hidden
#nullable disable
            WriteLiteral("                        </tbody>\r\n                    </table>\r\n                </div>\r\n            </div>\r\n");
#nullable restore
#line 48 "C:\Users\MONSTER\source\repos\camasirhaneekipmanlari_panel\panel\panel\Views\Home\Index.cshtml"
        }
        else
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <div class=\"card mb-4\">\r\n                <div class=\"card-body\">\r\n                    Üzgünüm kayıt bulunamadı.\r\n                </div>\r\n            </div>\r\n");
#nullable restore
#line 56 "C:\Users\MONSTER\source\repos\camasirhaneekipmanlari_panel\panel\panel\Views\Home\Index.cshtml"
        }
    

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<Log>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
