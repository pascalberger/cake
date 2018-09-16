// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.TFBuild.Data;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Common.Build.TFBuild
{
    /// <summary>
    /// Responsible for communicating with Team Foundation Build (Azure DevOps or TFS).
    /// </summary>
    public sealed class TFBuildProvider : ITFBuildProvider
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="TFBuildProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        public TFBuildProvider(ICakeEnvironment environment, ICakeLog log)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }
            _environment = environment;
            Environment = new TFBuildEnvironmentInfo(environment);
            Commands = new TFBuildCommands(environment, log);
        }

        /// <summary>
        /// Gets a value indicating whether the current build is running on VSTS.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on VSTS; otherwise, <c>false</c>.
        /// </value>
        [Obsolete("Use IsRunningOnAzureDevOps instead")]
        public bool IsRunningOnVSTS
            => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("TF_BUILD")) && IsHostedAgent;

        /// <summary>
        /// Gets a value indicating whether the current build is running on Azure DevOps.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on Azure DevOps; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnAzureDevOps
            => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("TF_BUILD")) && IsCloudCollectionUrl;

        /// <summary>
        /// Gets a value indicating whether the current build is running on TFS.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on TFS; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnTFS
            => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("TF_BUILD")) && !IsCloudCollectionUrl;

        /// <summary>
        /// Gets the TF Build environment.
        /// </summary>
        /// <value>
        /// The TF Build environment.
        /// </value>
        public TFBuildEnvironmentInfo Environment { get; }

        /// <summary>
        /// Gets the TF Build Commands provider.
        /// </summary>
        /// <value>
        /// The TF Build commands provider.
        /// </value>
        public ITFBuildCommands Commands { get; }

        /// <summary>
        /// Gets a value indicating whether the current build is running on a hosted build agent.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on a hosted agent; otherwise, <c>false</c>.
        /// </value>
        [Obsolete("Use IsMicrosoftHostedAgent instead")]
        private bool IsHostedAgent
            =>
                !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("AGENT_NAME")) &&
                _environment.GetEnvironmentVariable("AGENT_NAME") == "Hosted Agent";

        /// <summary>
        /// Gets a value indicating whether the current build is running on a Microsoft hosted build agent.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on a Microsoft hosted agent; otherwise, <c>false</c>.
        /// </value>
        private bool IsMicrosoftHostedAgent
            =>
                !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("SYSTEM_SERVERTYPE")) &&
                _environment.GetEnvironmentVariable("SYSTEM_SERVERTYPE") == "Hosted";

        /// <summary>
        /// Gets a value indicating whether the current build definition is on Azure DevOps.
        /// The build itself can run on a Microsoft hosted or private hosted agent.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running for a build definition on Azure DevOps; otherwise, <c>false</c>.
        /// </value>
        private bool IsCloudCollectionUrl
            =>
                !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("SYSTEM_COLLECTIONURI")) &&
                (new Uri(_environment.GetEnvironmentVariable("SYSTEM_COLLECTIONURI")).Host == "dev.azure.com" ||
                new Uri(_environment.GetEnvironmentVariable("SYSTEM_COLLECTIONURI")).Host.EndsWith("visualstudio.com"));
    }
}
