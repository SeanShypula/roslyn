﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System.Composition;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Shared.Utilities;
using Microsoft.CodeAnalysis.Text;

namespace Microsoft.CodeAnalysis.Host
{
    [ExportWorkspaceService(typeof(ITextFactoryService), ServiceLayer.Default), Shared]
    internal class TextFactoryService : ITextFactoryService
    {
        [ImportingConstructor]
        [SuppressMessage("RoslynDiagnosticsReliability", "RS0033:Importing constructor should be [Obsolete]", Justification = "Used in test code: https://github.com/dotnet/roslyn/issues/42814")]
        public TextFactoryService()
        {
        }

        public SourceText CreateText(Stream stream, Encoding? defaultEncoding, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return EncodedStringText.Create(stream, defaultEncoding);
        }

        public SourceText CreateText(TextReader reader, Encoding? encoding, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var textReaderWithLength = reader as TextReaderWithLength;
            if (textReaderWithLength != null)
            {
                return SourceText.From(textReaderWithLength, textReaderWithLength.Length, encoding);
            }

            return SourceText.From(reader.ReadToEnd(), encoding);
        }
    }
}

