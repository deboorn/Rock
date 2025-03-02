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
    /// FollowingEventType View Model
    /// </summary>
    public partial class FollowingEventTypeBag : EntityBagBase
    {
        /// <summary>
        /// Gets or sets the user defined description of the FollowingEvent.
        /// </summary>
        /// <value>
        /// A System.String representing the user defined description of the FollowingEvent.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets how an entity should be formatted when included in the event notification to follower.
        /// </summary>
        /// <value>
        /// The item notification lava.
        /// </value>
        public string EntityNotificationFormatLava { get; set; }

        /// <summary>
        /// Gets or sets the event MEF component identifier.
        /// </summary>
        /// <value>
        /// The event entity type identifier.
        /// </value>
        public int? EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the followed entity type identifier.
        /// </summary>
        /// <value>
        /// The followed entity type identifier.
        /// </value>
        public int? FollowedEntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [include non public requests].
        /// </summary>
        /// <value>
        ///   true if [include non public requests]; otherwise, false.
        /// </value>
        public bool IncludeNonPublicRequests { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        /// <value>
        ///   true if this instance is active; otherwise, false.
        /// </value>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this event is required. If not, followers will be able to optionally select if they want to be notified of this event
        /// </summary>
        /// <value>
        /// true if this instance is notice required; otherwise, false.
        /// </value>
        public bool IsNoticeRequired { get; set; }

        /// <summary>
        /// Gets or sets the last check.
        /// </summary>
        /// <value>
        /// The last check.
        /// </value>
        public DateTime? LastCheckDateTime { get; set; }

        /// <summary>
        /// Gets or sets the (internal) Name of the FollowingEvent. This property is required.
        /// </summary>
        /// <value>
        /// A System.String representing the (internal) name of the FollowingEvent.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [send on weekends].
        /// </summary>
        /// <value>
        ///   true if [send on weekends]; otherwise, false.
        /// </value>
        public bool SendOnWeekends { get; set; }

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
