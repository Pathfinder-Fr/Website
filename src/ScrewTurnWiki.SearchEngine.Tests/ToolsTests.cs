using System;
using NUnit.Framework;

namespace ScrewTurn.Wiki.SearchEngine.Tests
{
    [TestFixture]
    public class ToolsTests : TestsBase
    {
        [Test]
        public void IsSplitChar()
        {
            foreach (var c in ",.;:-\"'!?^=()<>\\|/[]{}«»*°§%&#@~©®±")
            {
                Assert.IsTrue(Tools.IsSplitChar(c), "Char is a split char");
            }
            foreach (var c in "abcdefghijklmnopqrstuvwxyz0123456789òçàùèéì€$£")
            {
                Assert.IsFalse(Tools.IsSplitChar(c), "Char is not a split char");
            }
        }

        [Test]
        public void RemoveDiacriticsAndPunctuation()
        {
            var testPhrase = "Wow, thìs thing sèems really cool!";
            var testWord = "Wòrd";

            Assert.AreEqual("wow this thing seems really cool", Tools.RemoveDiacriticsAndPunctuation(testPhrase, false),
                "Wrong normalized phrase");
            Assert.AreEqual("word", Tools.RemoveDiacriticsAndPunctuation(testWord, true), "Wrong normalized word");
        }

        [Test]
        public void RemoveStopWords()
        {
            WordInfo[] input =
            {
                new WordInfo("I", 0, 0, WordLocation.Content), new WordInfo("like", 7, 1, WordLocation.Content),
                new WordInfo("the", 15, 2, WordLocation.Content), new WordInfo("cookies", 22, 3, WordLocation.Content)
            };
            WordInfo[] expectedOutput =
            {
                new WordInfo("I", 0, 0, WordLocation.Content), new WordInfo("like", 7, 1, WordLocation.Content),
                new WordInfo("cookies", 22, 3, WordLocation.Content)
            };

            var output = Tools.RemoveStopWords(input, new[] {"the", "in", "of"});

            Assert.AreEqual(expectedOutput.Length, output.Length, "Wrong output length");

            for (var i = 0; i < output.Length; i++)
            {
                Assert.AreEqual(expectedOutput[i].Text, output[i].Text, "Wrong word text at index " + i);
                Assert.AreEqual(expectedOutput[i].FirstCharIndex, output[i].FirstCharIndex,
                    "Wrong word position at index " + i);
            }
        }

        [Test]
        public void SkipSplitChars()
        {
            Assert.AreEqual(0, Tools.SkipSplitChars(0, "hello"));
            Assert.AreEqual(1, Tools.SkipSplitChars(0, " hello"));
            Assert.AreEqual(7, Tools.SkipSplitChars(6, "Hello! How are you?"));
        }

        [Test]
        public void Tokenize()
        {
            var input = "Hello, there!";
            WordInfo[] expectedOutput =
            {
                new WordInfo("Hello", 0, 0, WordLocation.Content),
                new WordInfo("there", 7, 1, WordLocation.Content)
            };

            var output = Tools.Tokenize(input, WordLocation.Content);

            Assert.AreEqual(expectedOutput.Length, output.Length, "Wrong output length");

            for (var i = 0; i < output.Length; i++)
            {
                Assert.AreEqual(expectedOutput[i].Text, output[i].Text, "Wrong word text at index " + i);
                Assert.AreEqual(expectedOutput[i].FirstCharIndex, output[i].FirstCharIndex,
                    "Wrong first char index at " + i);
                Assert.AreEqual(expectedOutput[i].WordIndex, output[i].WordIndex, "Wrong word index at " + i);
            }
        }

        [Test]
        public void Tokenize_OneWord()
        {
            var input = "todo";
            WordInfo[] expectedOutput = {new WordInfo("todo", 0, 0, WordLocation.Content)};

            var output = Tools.Tokenize(input, WordLocation.Content);

            Assert.AreEqual(expectedOutput.Length, output.Length, "Wrong output length");

            for (var i = 0; i < output.Length; i++)
            {
                Assert.AreEqual(expectedOutput[i].Text, output[i].Text, "Wrong word text at index " + i);
                Assert.AreEqual(expectedOutput[i].FirstCharIndex, output[i].FirstCharIndex,
                    "Wrong first char index at " + i);
                Assert.AreEqual(expectedOutput[i].WordIndex, output[i].WordIndex, "Wrong word index at " + i);
            }
        }

        [Test]
        public void Tokenize_OneWordWithSplitChar()
        {
            var input = "todo.";
            WordInfo[] expectedOutput = {new WordInfo("todo", 0, 0, WordLocation.Content)};

            var output = Tools.Tokenize(input, WordLocation.Content);

            Assert.AreEqual(expectedOutput.Length, output.Length, "Wrong output length");

            for (var i = 0; i < output.Length; i++)
            {
                Assert.AreEqual(expectedOutput[i].Text, output[i].Text, "Wrong word text at index " + i);
                Assert.AreEqual(expectedOutput[i].FirstCharIndex, output[i].FirstCharIndex,
                    "Wrong first char index at " + i);
                Assert.AreEqual(expectedOutput[i].WordIndex, output[i].WordIndex, "Wrong word index at " + i);
            }
        }
    }
}