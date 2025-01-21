// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.InspectCode
{
    /// <summary>
    /// Possible output format for InspectCode.
    /// </summary>
    public enum InspectCodeFormat
    {
        /// <summary>
        /// Output in HTML format.
        /// </summary>
        Html = 1,

        /// <summary>
        /// Output in text format.
        /// </summary>
        Text = 2,

        /// <summary>
        /// Output in XML format.
        /// </summary>
        Xml = 3
    }
}