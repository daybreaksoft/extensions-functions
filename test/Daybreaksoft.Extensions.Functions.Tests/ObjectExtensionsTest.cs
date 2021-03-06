﻿using System;
using System.Collections.Generic;
using Daybreaksoft.Extensions.Functions.Tests.Requires;
using Xunit;

namespace Daybreaksoft.Extensions.Functions.Tests
{
    public class ObjectExtensionsTest
    {
        [Fact]
        public void TestCopyValue()
        {
            var m1 = new Model1
            {
                P1 = "P1",
                P2 = "P2",
                P3 = "P3",
                P4 = "P4",
                P5 = "P5",
                P6 = 6,
                PP = "P7",
                P8 = new Model3
                {
                    P1 = "P3-P1"
                },
                P9 = new List<Model4> {
                    new Model4{ P1 = "1-P4-P1"},
                    new Model4{ P1 = "2-P4-P1"}
                },
                P10 = 10
            };

            // Verifies that pass a null object
            Assert.Throws<ArgumentNullException>(() =>
            {
                m1.CopyValueTo(null);
            });

            // Verifies that set value between different type
            var m2 = new Model2
            {
                P1 = -10
            };
            Assert.Throws<InvalidTypeConvertedException>(() =>
            {
                m1.CopyValueTo(m2);
            });

            // Verifies that successful
            var m3 = new Model3
            {
                p5 = "-p5"
            };
            m1.CopyValueTo(m3);
            Assert.Equal("P1", m3.P1);
            Assert.Equal("P2", m3.P2);
            Assert.Equal("P3", m3.P3);
            Assert.Equal("-p5", m3.p5);
            Assert.Equal(6, m3.P6);
            Assert.Null(m3.P7);

            // Verifies that ignore case
            m1.CopyValueTo(target: m3, stringComparison: StringComparison.CurrentCultureIgnoreCase);
            Assert.Equal("P5", m3.p5);

            // Verifies that ignore property
            m3 = new Model3();
            m1.CopyValueTo(target: m3, ignorePropertyNames:new string[] { "P1", "P2" });
            Assert.Null(m3.P1);
            Assert.Null(m3.P2);

            // Verifies that ignore property
            m3 = new Model3();
            var map = new Dictionary<string, string>();
            map.Add("PP", "P7");
            m1.CopyValueTo(target: m3, propertyMap: map);
            Assert.Equal("P7", m3.P7);

            // Verifies that throw MultipleResultException if ignore case
            var m4 = new Model4();
            Assert.Throws<MultipleResultException>(() =>
            {
                m1.CopyValueTo(target: m4, stringComparison: StringComparison.CurrentCultureIgnoreCase);
            });

            // Verifies that copy value type and ref type
            var m5 = new Model5();
            m1.CopyValueTo(m5, ignoreRefType: false);
            Assert.True(m1.P8 == m5.P8);
            Assert.True(m1.P9 == m5.P9);
            Assert.True(m1.P10.ToString() == m5.P10.ToString());

            // Verifies that ignore ref type
            m5 = new Model5();
            m1.CopyValueTo(m5);
            Assert.Null(m5.P8);
            Assert.Null(m5.P9);
            Assert.Equal(0, m5.P10);

            // Verifies that force property names
            m5 = new Model5();
            m1.CopyValueTo(m5, forcePropertyNames: new string[] {"P10"});
            Assert.Null(m5.P8);
            Assert.Null(m5.P9);
            Assert.Equal(10, m5.P10);
        }
    }
}
