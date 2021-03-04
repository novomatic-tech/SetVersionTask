﻿namespace SetVersionTask
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class VersionUpdateRuleTests
    {
        [TestCase("1")]
        [TestCase("")]
        [TestCase("1.")]
        public void RulesMustHaveAtLeastTwoParts(string rule)
        {
            try
            {
                new VersionUpdateRule(rule);
            }
            catch (ArgumentException)
            {
                // expected
            }
            catch (FormatException)
            {
                // also expected
            }
        }

        [TestCase("1.2.3.4.5")]
        [TestCase("......")]
        [TestCase("1.2.3.4.")]
        public void RulesMustHaveNoMoreThanFourParts(string rule)
        {
            Assert.Throws<ArgumentException>(() => new VersionUpdateRule(rule));
        }
        
        [TestCase("1.2.3.4", "0.0.0.0")]
        [TestCase("1.2.3.4", "0.0.*")]
        [TestCase("1.2.3", "0.0.0.0")]
        public void RulesCanDirectlySetVersion(string rule, string input)
        {
            var r = new VersionUpdateRule(rule);
            var updated = r.Update(input);
            Assert.AreEqual(rule, updated);
        }

        [TestCase("=.=.=.=", "1.2.3.4", "1.2.3.4")]
        [TestCase("=.=.=.=", "0.0.*", "0.0.*")]
        [TestCase("=.=.=", "0.0.0.0", "0.0.0")]
        public void RulesCanCopyInput(string rule, string input, string expected)
        {
            var r = new VersionUpdateRule(rule);
            var updated = r.Update(input);
            Assert.AreEqual(expected, updated);
        }

        [TestCase("+.+.+.+", "1.2.3.4", "2.3.4.5")]
        [TestCase("=.=.+", "0.0.*", "0.0.1")]
        [TestCase("=.=.+", "2.3.4.5", "2.3.5")]
        public void RulesCanIncrement(string rule, string input, string expected)
        {
            var r = new VersionUpdateRule(rule);
            var updated = r.Update(input);
            Assert.AreEqual(expected, updated);
        }    
    }
}
