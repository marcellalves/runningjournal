﻿using RunningJournal.Api;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace RunningJournalApi.UnitTests
{
    public class SimpleWebTokenTests
    {
        [Fact]
        public void SutIsIteratorOfClaims()
        {
            var sut = new SimpleWebToken();
            Assert.IsAssignableFrom<IEnumerable<Claim>>(sut);
        }

        [Fact]
        public void SutYieldsInjectedClaims()
        {
            var expected = new[]
            {
                new Claim("foo", "bar"),
                new Claim("baz", "qux"),
                new Claim("quux", "corge"),
            };
            var sut = new SimpleWebToken(expected);
            Assert.True(expected.SequenceEqual(sut));
            Assert.True(
                expected.SequenceEqual(
                    sut.OfType<object>()));
        }

        [Theory]
        [InlineData(new string[0], "")]
        [InlineData(new[] { "foo|bar" }, "foo=bar")]
        [InlineData(new[] { "foo|bar", "baz|qux" }, "foo=bar&baz=qux")]
        public void ToStringReturnsCorrectResult(
            string[] keysAndValues,
            string expected)
        {
            // Fixture setup
            var claims = keysAndValues
                .Select(s => s.Split('|'))
                .Select(a => new Claim(a[0], a[1]))
                .ToArray();
            var sut = new SimpleWebToken(claims);
            // Exercise system
            var actual = sut.ToString();
            // Verify outcome
            Assert.Equal(expected, actual);
        }
    }
}