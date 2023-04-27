﻿using System;
using NLog.Common;
using NLog.Config;
using NLog.Layouts;
using Xunit;
using Xunit.Abstractions;

namespace NLog.Targets.OpenSearch.Tests
{
    public class IntegrationTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public IntegrationTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        public class BadLogException : Exception
        {
            public object[] BadArray { get; }
            public System.Reflection.Assembly BadProperty => typeof(BadLogException).Assembly;
            public object ExceptionalBadProperty => throw new System.NotSupportedException();

            public BadLogException()
            {
                BadArray = new object[] { this };
            }
        }

        [Theory(Skip = "Integration")]
        [InlineData(true)]
        [InlineData(false)]
        public void ExceptionSerializationTest(bool hasExceptionFieldLayout)
        {
            using (var testOutputTextWriter = new TestOutputTextWriter(testOutputHelper))
            {
                InternalLogger.LogWriter = testOutputTextWriter;
                InternalLogger.LogLevel = LogLevel.Warn;

                var openSearchTarget = new OpenSearchTarget();

                if (hasExceptionFieldLayout)
                {
                    openSearchTarget.Fields.Add(new Field
                    {
                        Name = "exception",
                        Layout = Layout.FromString("${exception:format=toString,Data:maxInnerExceptionLevel=10}"),
                        LayoutType = typeof(string)
                    });
                }

                var rule = new LoggingRule("*", LogLevel.Info, openSearchTarget);

                var config = new LoggingConfiguration();
                config.LoggingRules.Add(rule);

                LogManager.ThrowExceptions = true;
                LogManager.Configuration = config;

                var logger = LogManager.GetLogger("Example");

                logger.Error(new BadLogException(), "Boom");

                LogManager.Flush();
            }
        }

        [Fact(Skip = "Integration")]        
        public void SimpleLogTest()
        {
            var openSearchTarget = new OpenSearchTarget();

            var rule = new LoggingRule("*", openSearchTarget);
            rule.EnableLoggingForLevel(LogLevel.Info);

            var config = new LoggingConfiguration();
            config.LoggingRules.Add(rule);

            LogManager.ThrowExceptions = true;
            LogManager.Configuration = config;

            var logger = LogManager.GetLogger("Example");            
            logger.Info("Hello OpenSearch");

            LogManager.Flush();
        }

        [Fact(Skip = "Integration")]
        public void SimpleJsonLayoutTest()
        {
            var openSearchTarget = new OpenSearchTarget();
            openSearchTarget.EnableJsonLayout = true;
            openSearchTarget.Layout = new JsonLayout()
            {
                MaxRecursionLimit = 10,
                IncludeAllProperties = true,
                Attributes =
                    {
                        new JsonAttribute("timestamp", "${date:universaltime=true:format=o}"),
                        new JsonAttribute("lvl", "${level}"),
                        new JsonAttribute("msg", "${message}"),
                        new JsonAttribute("logger", "${logger}"),
                        new JsonAttribute("threadid", "${threadid}", false), // Skip quotes for integer-value
                    }
            };           
            var rule = new LoggingRule("*", openSearchTarget);
            rule.EnableLoggingForLevel(LogLevel.Info);

            var config = new LoggingConfiguration();
            config.LoggingRules.Add(rule);

            LogManager.ThrowExceptions = true;
            LogManager.Configuration = config;

            var logger = LogManager.GetLogger("Example");

            logger.Info("Hello OpenSearch");

            LogManager.Flush();
        }

        [Fact(Skip = "Integration")]
        public void ExceptionTest()
        {
            var openSearchTarget = new OpenSearchTarget();

            var rule = new LoggingRule("*", openSearchTarget);
            rule.EnableLoggingForLevel(LogLevel.Error);

            var config = new LoggingConfiguration();
            config.LoggingRules.Add(rule);

            LogManager.ThrowExceptions = true;
            LogManager.Configuration = config;

            var logger = LogManager.GetLogger("Example");

            var exception = new ArgumentException("Some random error message");

            logger.Error(exception, "An exception occured");

            LogManager.Flush();
        }

        [Fact(Skip = "Integration")]
        public void ReadFromConfigTest()
        {
            LogManager.ThrowExceptions = true;
            LogManager.Configuration = new XmlLoggingConfiguration("NLog.Targets.OpenSearch.Tests.dll.config");

            var logger = LogManager.GetLogger("Example");

            logger.Info("Hello OpenSearch");

            LogManager.Flush();
        }
    }
}