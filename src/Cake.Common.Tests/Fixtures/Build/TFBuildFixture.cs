// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.TFBuild;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Testing;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Build
{
    internal sealed class TFBuildFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public FakeLog Log { get; set; }

        public TFBuildFixture()
        {
            Environment = Substitute.For<ICakeEnvironment>();
            Environment.WorkingDirectory.Returns("C:\\build\\CAKE-CAKE-JOB1");
            Environment.GetEnvironmentVariable("TF_BUILD").Returns((string)null);
            Log = new FakeLog();
        }

        public void IsRunningOnAzureDevOpsWithMicrosoftHostedAgent()
        {
            Environment.GetEnvironmentVariable("TF_BUILD").Returns("True");
            Environment.GetEnvironmentVariable("SYSTEM_COLLECTIONURI").Returns("https://dev.azure.com/cake-build/");
            Environment.GetEnvironmentVariable("SYSTEM_SERVERTYPE").Returns("Hosted");
        }

        public void IsRunningOnAzureDevOpsWithPrivateHostedAgent()
        {
            Environment.GetEnvironmentVariable("TF_BUILD").Returns("True");
            Environment.GetEnvironmentVariable("SYSTEM_COLLECTIONURI").Returns("https://dev.azure.com/cake-build/");
            Environment.GetEnvironmentVariable("SYSTEM_SERVERTYPE").Returns((string)null);
        }

        public void IsRunningOnTFS()
        {
            Environment.GetEnvironmentVariable("TF_BUILD").Returns("True");
            Environment.GetEnvironmentVariable("SYSTEM_COLLECTIONURI").Returns("https://tfs.mycompany.com/tfs");
            Environment.GetEnvironmentVariable("SYSTEM_SERVERTYPE").Returns((string)null);
        }

        public TFBuildProvider CreateTFBuildService()
        {
            return new TFBuildProvider(Environment, Log);
        }
    }
}
