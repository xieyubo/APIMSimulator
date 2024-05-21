using Microsoft.AspNetCore.Razor.Parser;
using Microsoft.AspNetCore.Razor.Parser.Internal;
using Microsoft.AspNetCore.Razor.Parser.SyntaxTree;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using System.IO;
using System.Security;
using System.Text;

namespace APIMSimulator;

public static class PolicyUtils
{
    public static string ConvertPolicyToStandardXmlString(string policyString)
    {
        var razorParser = new RazorParser(new CSharpCodeParser(), new HtmlMarkupParser(), new TagHelperDescriptorResolver(false));
        var xmlContent = razorParser.Parse(new StringReader(policyString));
        var encodedContent = EncodeXmlContent(xmlContent.Document);
        return encodedContent;
    }

    private static string EncodeXmlContent(Block block)
    {
        var sb = new StringBuilder();
        foreach (var child in block.Children)
        {
            if (child.IsBlock)
            {
                sb.Append(EncodeXmlContent((Block)child));
            }
            else
            {
                var span = (Span)child;
                if (span.Kind == SpanKind.Code)
                {
                    sb.Append(SecurityElement.Escape(span.Content));
                }
                else
                {
                    sb.Append(span.Content);
                }
            }
        }
        return sb.ToString();
    }
}
