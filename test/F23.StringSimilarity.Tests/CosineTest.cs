﻿/*
 * The MIT License
 *
 * Copyright 2016 feature[23]
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace F23.StringSimilarity.Tests
{
    [SuppressMessage("ReSharper", "ArgumentsStyleLiteral")]
    [SuppressMessage("ReSharper", "ArgumentsStyleNamedExpression")]
    public class CosineTest
    {
        [Fact]
        public void TestSimilarity()
        {
            var instance = new Cosine();

            var result = instance.Similarity("ABC", "ABCE");

            Assert.Equal(
                expected: 0.71, 
                actual: result, 
                precision: 2 // 0.01
            );
        }

        [Fact]
        public void TestSmallString()
        {
            var instance = new Cosine(3);

            var result = instance.Similarity("AB", "ABCE");

            Assert.Equal(
                expected: 0.0, 
                actual: result, 
                precision: 5 //0.00001
            );
        }

        [Fact]
        public async Task TestLargeString()
        {
            var instance = new Cosine();

            // Read text from two files
            var string1 = await ReadResourceFileAsync("71816-2.txt");
            var string2 = await ReadResourceFileAsync("11328-1.txt");

            var result = instance.Similarity(string1, string2);

            Assert.Equal(
                expected: 0.8115, 
                actual: result,
                precision: 3 //0.001
            );
        }

        private static async Task<string> ReadResourceFileAsync(string file)
        {
            var assembly = typeof(CosineTest).GetTypeInfo().Assembly;
            var resourceName = $"{typeof(CosineTest).Namespace}.{file}";

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                Debug.Assert(stream != null, "stream != null");
                using (var reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}
