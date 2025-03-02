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
    /// Communication View Model
    /// </summary>
    public partial class CommunicationBag : EntityBagBase
    {
        /// <summary>
        /// Gets or sets a JSON string containing any additional merge fields for the Communication.
        /// </summary>
        /// <value>
        /// A Json formatted System.String that contains any additional merge fields for the Communication.
        /// </value>
        public string AdditionalMergeFieldsJson { get; set; }

        /// <summary>
        /// Gets or sets a comma separated list of BCC'ed email addresses.
        /// </summary>
        /// <value>
        /// A comma separated list of BCC'ed email addresses.
        /// </value>
        public string BCCEmails { get; set; }

        /// <summary>
        /// Gets or sets a comma separated list of CC'ed email addresses.
        /// </summary>
        /// <value>
        /// A comma separated list of CC'ed email addresses.
        /// </value>
        public string CCEmails { get; set; }

        /// <summary>
        /// Gets or sets the Rock.Model.CommunicationTemplate that was used to compose this communication
        /// </summary>
        /// <value>
        /// The communication template identifier.
        /// </value>
        public int? CommunicationTemplateId { get; set; }

        /// <summary>
        /// Gets or sets the communication type value identifier.
        /// </summary>
        /// <value>
        /// The communication type value identifier.
        /// </value>
        public int CommunicationType { get; set; }

        /// <summary>
        /// Gets or sets a comma-delimited list of enabled LavaCommands
        /// </summary>
        /// <value>
        /// The enabled lava commands.
        /// </value>
        public string EnabledLavaCommands { get; set; }

        /// <summary>
        /// Option to prevent communications from being sent to people with the same email/SMS addresses.
        /// This will mean two people who share an address will not receive a personalized communication, only one of them will.
        /// </summary>
        /// <value>
        ///   true if [exclude duplicate recipient address]; otherwise, false.
        /// </value>
        public bool ExcludeDuplicateRecipientAddress { get; set; }

        /// <summary>
        /// Gets or sets from email address.
        /// </summary>
        /// <value>
        /// From email address.
        /// </value>
        public string FromEmail { get; set; }

        /// <summary>
        /// Gets or sets from name.
        /// </summary>
        /// <value>
        /// From name.
        /// </value>
        public string FromName { get; set; }

        /// <summary>
        /// Gets or sets the future send date for the communication. This allows a user to schedule when a communication is sent 
        /// and the communication will not be sent until that date and time.
        /// </summary>
        /// <value>
        /// A System.DateTime value that represents the FutureSendDate for the communication.  If no future send date is provided, this value will be null.
        /// </value>
        public DateTime? FutureSendDateTime { get; set; }

        /// <summary>
        /// Gets or sets the is bulk communication.
        /// </summary>
        /// <value>
        /// The is bulk communication.
        /// </value>
        public bool IsBulkCommunication { get; set; }

        /// <summary>
        /// Gets or sets the list that email is being sent to.
        /// </summary>
        /// <value>
        /// The list group identifier.
        /// </value>
        public int? ListGroupId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the message meta data.
        /// </summary>
        /// <value>
        /// The message meta data.
        /// </value>
        public string MessageMetaData { get; set; }

        /// <summary>
        /// Gets or sets the name of the Communication
        /// </summary>
        /// <value>
        /// A System.String that represents the name of the communication.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the push data.
        /// </summary>
        /// <value>
        /// The push data.
        /// </value>
        public string PushData { get; set; }

        /// <summary>
        /// Gets or sets the push image file identifier.
        /// </summary>
        /// <value>
        /// The push image file identifier.
        /// </value>
        public int? PushImageBinaryFileId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string PushMessage { get; set; }

        /// <summary>
        /// Gets or sets the push open action.
        /// </summary>
        /// <value>
        /// The push open action.
        /// </value>
        public int? PushOpenAction { get; set; }

        /// <summary>
        /// Gets or sets the push open message.
        /// </summary>
        /// <value>
        /// The push open message.
        /// </value>
        public string PushOpenMessage { get; set; }

        /// <summary>
        /// Gets or sets push sound.
        /// </summary>
        /// <value>
        /// Push sound.
        /// </value>
        public string PushSound { get; set; }

        /// <summary>
        /// Gets or sets the push notification title.
        /// </summary>
        /// <value>
        /// Push notification title.
        /// </value>
        public string PushTitle { get; set; }

        /// <summary>
        /// Gets or sets the reply to email address.
        /// </summary>
        /// <value>
        /// The reply to email address.
        /// </value>
        public string ReplyToEmail { get; set; }

        /// <summary>
        /// Gets or sets the date and time stamp of when the Communication was reviewed.
        /// </summary>
        /// <value>
        /// A System.DateTime representing the date and time that the Communication was reviewed.
        /// </value>
        public DateTime? ReviewedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the note that was entered by the reviewer.
        /// </summary>
        /// <value>
        /// A System.String representing a note that was entered by the reviewer.
        /// </value>
        public string ReviewerNote { get; set; }

        /// <summary>
        /// Gets or sets the reviewer person alias identifier.
        /// </summary>
        /// <value>
        /// The reviewer person alias identifier.
        /// </value>
        public int? ReviewerPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets if communication is targeted to people in all selected segments or any selected segments.
        /// </summary>
        /// <value>
        /// The segment criteria.
        /// </value>
        public int SegmentCriteria { get; set; }

        /// <summary>
        /// Gets or sets the segments that list is being filtered to (comma-delimited list of dataview guids).
        /// </summary>
        /// <value>
        /// The segments.
        /// </value>
        public string Segments { get; set; }

        /// <summary>
        /// Gets or sets the datetime that communication was sent. This also indicates that communication shouldn't attempt to send again.
        /// </summary>
        /// <value>
        /// The send date time.
        /// </value>
        public DateTime? SendDateTime { get; set; }

        /// <summary>
        /// Gets or sets the sender Rock.Model.PersonAlias identifier.
        /// </summary>
        /// <value>
        /// The sender person alias identifier.
        /// </value>
        public int? SenderPersonAliasId { get; set; }

        /// <summary>
        /// Gets or sets the system phone number identifier used for SMS sending.
        /// </summary>
        /// <value>
        /// The system phone number identifier used for SMS sending.
        /// </value>
        public int? SmsFromSystemPhoneNumberId { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string SMSMessage { get; set; }

        /// <summary>
        /// Gets or sets the status of the Communication.
        /// </summary>
        /// <value>
        /// A Rock.Model.CommunicationStatus enum value that represents the status of the Communication.
        /// </value>
        public int Status { get; set; }

        /// <summary>
        /// Gets or sets the name of the Communication
        /// </summary>
        /// <value>
        /// A System.String that represents the name of the communication.
        /// </value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the Rock.Model.Communication.SystemCommunication that this communication is associated with.
        /// </summary>
        /// <value>
        /// The system communication.
        /// </value>
        public int? SystemCommunicationId { get; set; }

        /// <summary>
        /// Gets or sets the URL from where this communication was created (grid)
        /// </summary>
        /// <value>
        /// The URL referrer.
        /// </value>
        public string UrlReferrer { get; set; }

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
