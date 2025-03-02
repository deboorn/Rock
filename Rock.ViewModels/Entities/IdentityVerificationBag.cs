//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// <copyright>
// Copyright by the Spark Development Network
//
// Licensed under the Rock Community License (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.rockrms.com/license
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//

using System;
using System.Linq;

using Rock.ViewModels.Utility;

namespace Rock.ViewModels.Entities
{
    /// <summary>
    /// IdentityVerification View Model
    /// </summary>
    public partial class IdentityVerificationBag : EntityBagBase
    {
        /// <summary>
        /// Gets or sets the failed match attempt count.
        /// </summary>
        /// <value>
        /// The failed match attempt count.
        /// </value>
        public int? FailedMatchAttemptCount { get; set; }

        /// <summary>
        /// Gets or sets the identity verification code identifier.
        /// </summary>
        /// <value>
        /// The identity verification code identifier.
        /// </value>
        public int IdentityVerificationCodeId { get; set; }

        /// <summary>
        /// Gets or sets the issue date time.
        /// </summary>
        /// <value>
        /// The issue date time.
        /// </value>
        public DateTime IssueDateTime { get; set; }

        /// <summary>
        /// Gets or sets the reference number.
        /// </summary>
        /// <value>
        /// The reference number.
        /// </value>
        public string ReferenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the request ip address.
        /// </summary>
        /// <value>
        /// The request ip address.
        /// </value>
        public string RequestIpAddress { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>
        /// The created date time.
        /// </value>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the modified date time.
        /// </summary>
        /// <value>
        /// The modified date time.
        /// </value>
        public DateTime? ModifiedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the created by person alias identifier.
        /// </summary>
        /// <value>
        /// The created by person alias identifier.
        /// </value>
        public int? CreatedByPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the modified by person alias identifier.
        /// </summary>
        /// <value>
        /// The modified by person alias identifier.
        /// </value>
        public int? ModifiedByPersonAliasId { get; set; }

    }
}
