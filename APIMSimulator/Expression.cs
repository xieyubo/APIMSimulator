﻿using APIMSimulator.Abstract;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace APIMSimulator;

internal class Expression
{
    private MethodInfo? _method;
    private string _body;

    public Expression(string body)
    {
        _body = body;

        body = body.Trim();

        bool isCode = false;
        var className = $"APIMSimulator_AutoGenerated_{Guid.NewGuid().ToString("N")}";
        if (body.StartsWith("@("))
        {
            body = body.Substring(1);
            isCode = true;
        }
        else if (body.StartsWith("@{"))
        {
            body = $"new Func<object>(() => {body.Substring(1)})()";
            isCode = true;
        }

        if (isCode)
        {
            var compilation = Compile(className, body);
            _method = GenerateCode(compilation).GetTypes().Single(t => t.Name == className).GetMethod("Execute")!;
        }
    }

    public object? Execute(IContext context)
    {
        return _method == null ? _body : _method.Invoke(null, new[] { context });
    }

    private static CSharpCompilation Compile(string className, string body)
    {
        var source = @$"
        using APIMSimulator.Abstract;
        using Newtonsoft.Json;
        using Newtonsoft.Json.Linq;
        using System;
        using System.Linq;
        using System.Security.Cryptography;
        using System.Text;

        namespace APIMSimulator_AutoGenerated;

        public class {className}
        {{
            public static object Execute(IContext context)
            {{
                return {body};
            }}
        }}
        ";

        var references = new[]
        {
            "APIMSimulator.Abstract",
            "netstandard",
            "Newtonsoft.Json",
            "System.Collections",
            "System.ComponentModel",
            "System.ComponentModel.TypeConverter",
            "System.Linq",
            "System.Linq.Expressions",
            "System.ObjectModel",
            "System.Private.CoreLib",
            "System.Runtime",
            "System.Security.Cryptography",
        }.Select(t => Assembly.Load(new AssemblyName(t)).Location).ToArray();

        var compilation = CSharpCompilation.Create(
            assemblyName: null,
            syntaxTrees: new[] { CSharpSyntaxTree.ParseText(source) },
            references: references.Distinct().Select(t => MetadataReference.CreateFromFile(t)),
            options: new CSharpCompilationOptions(
                outputKind: OutputKind.DynamicallyLinkedLibrary
            )
        );
        return compilation;
    }

    private static Assembly GenerateCode(CSharpCompilation compilation)
    {
        using var peStream = new MemoryStream();
        var result = compilation.Emit(peStream);
        if (!result.Success)
        {
            var error = String.Join("\n", result.Diagnostics.Select(s => s.GetMessage()).ToArray());
            throw new Exception($"Compile policy failed: {error}");
        }

        peStream.Position = 0;
        return Assembly.Load(peStream.ToArray());
    }
}
