﻿// <copyright>
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Rock.Data;
using Rock.Tasks;
using Rock.Web.Cache;

namespace Rock.Model
{
    public partial class GroupMember
    {
        /// <summary>
        /// Save hook implementation for <see cref="GroupMember"/>.
        /// </summary>
        /// <seealso cref="Rock.Data.EntitySaveHook{TEntity}" />
        internal class SaveHook : EntitySaveHook<GroupMember>
        {
            private List<HistoryItem> HistoryChanges { get; set; }

            /// <summary>
            /// Called before the save operation is executed.
            /// </summary>
            protected override void PreSave()
            {
                var rockContext = ( RockContext ) this.RockContext;
                string errorMessage;
                if ( State != EntityContextState.Deleted
                     && Entity.IsArchived == false
                     && Entity.GroupMemberStatus != GroupMemberStatus.Inactive )
                {
                    if ( !Entity.ValidateGroupMembership( rockContext, out errorMessage ) )
                    {
                        var ex = new GroupMemberValidationException( errorMessage );
                        ExceptionLogService.LogException( ex );
                        throw ex;
                    }
                }

                var updateGroupMemberMsg = new UpdateGroupMember.Message
                {
                    State = State,
                    GroupId = Entity.GroupId,
                    PersonId = Entity.PersonId,
                    GroupMemberStatus = Entity.GroupMemberStatus,
                    GroupMemberRoleId = Entity.GroupRoleId,
                    IsArchived = Entity.IsArchived
                };

                if ( Entity.Group != null )
                {
                    updateGroupMemberMsg.GroupTypeId = Entity.Group.GroupTypeId;
                }

                // If this isn't a new group member, get the previous status and role values
                if ( State == EntityContextState.Modified )
                {
                        updateGroupMemberMsg.PreviousGroupMemberStatus = ( GroupMemberStatus ) OriginalValues["GroupMemberStatus"].ToStringSafe().ConvertToEnum<GroupMemberStatus>();
                        updateGroupMemberMsg.PreviousGroupMemberRoleId = OriginalValues["GroupRoleId"].ToStringSafe().AsInteger();
                        updateGroupMemberMsg.PreviousIsArchived = OriginalValues["IsArchived"].ToStringSafe().AsBoolean();
                }

                // If this isn't a deleted group member, get the group member guid
                if ( State != EntityContextState.Deleted )
                {
                    updateGroupMemberMsg.GroupMemberGuid = Entity.Guid;
                }

                updateGroupMemberMsg.Send();

                int? oldPersonId = null;
                int? newPersonId = null;

                int? oldGroupId = null;
                int? newGroupId = null;

                switch ( State )
                {
                    case EntityContextState.Added:
                        {
                            oldPersonId = null;
                            newPersonId = Entity.PersonId;

                            oldGroupId = null;
                            newGroupId = Entity.GroupId;

                            if ( !Entity.DateTimeAdded.HasValue )
                            {
                                Entity.DateTimeAdded = RockDateTime.Now;
                            }

                            // if this is a new record, but is saved with IsActive=False, set the InactiveDateTime if it isn't set already
                            if ( Entity.GroupMemberStatus == GroupMemberStatus.Inactive )
                            {
                                Entity.InactiveDateTime = Entity.InactiveDateTime ?? RockDateTime.Now;
                            }

                            break;
                        }

                    case EntityContextState.Modified:
                        {
                            oldPersonId = OriginalValues["PersonId"].ToStringSafe().AsIntegerOrNull();
                            newPersonId = Entity.PersonId;

                            oldGroupId = OriginalValues["GroupId"].ToStringSafe().AsIntegerOrNull();
                            newGroupId = Entity.GroupId;

                            var originalStatus = OriginalValues["GroupMemberStatus"].ToStringSafe().ConvertToEnum<GroupMemberStatus>();

                            // IsActive was modified, set the InactiveDateTime if it changed to Inactive, or set it to NULL if it changed to Active
                            if ( originalStatus != Entity.GroupMemberStatus )
                            {
                                if ( Entity.GroupMemberStatus == GroupMemberStatus.Inactive )
                                {
                                    // if the caller didn't already set InactiveDateTime, set it to now
                                    Entity.InactiveDateTime = Entity.InactiveDateTime ?? RockDateTime.Now;
                                }
                                else
                                {
                                    Entity.InactiveDateTime = null;
                                }
                            }

                            break;
                        }

                    case EntityContextState.Deleted:
                        {
                            oldPersonId = Entity.PersonId;
                            newPersonId = null;

                            oldGroupId = Entity.GroupId;
                            newGroupId = null;

                            break;
                        }
                }

                Group group = Entity.Group;
                if ( group == null )
                {
                    group = new GroupService( rockContext ).Get( Entity.GroupId );
                }

                if ( group != null )
                {
                    string oldGroupName = group.Name;
                    if ( oldGroupId.HasValue && oldGroupId.Value != group.Id )
                    {
                        var oldGroup = new GroupService( rockContext ).Get( oldGroupId.Value );
                        if ( oldGroup != null )
                        {
                            oldGroupName = oldGroup.Name;
                        }
                    }

                    HistoryChanges = new List<HistoryItem>();
                    if ( newPersonId.HasValue )
                    {
                        HistoryChanges.Add( new HistoryItem()
                        {
                            PersonId = newPersonId.Value,
                            Caption = group.Name,
                            GroupId = group.Id,
                            Group = group
                        } );
                    }

                    if ( oldPersonId.HasValue )
                    {
                        HistoryChanges.Add( new HistoryItem()
                        {
                            PersonId = oldPersonId.Value,
                            Caption = oldGroupName,
                            GroupId = oldGroupId
                        } );
                    }

                    if ( newPersonId.HasValue && newGroupId.HasValue &&
                        ( !oldPersonId.HasValue || oldPersonId.Value != newPersonId.Value || !oldGroupId.HasValue || oldGroupId.Value != newGroupId.Value ) )
                    {
                        // New Person in group
                        var historyItem = HistoryChanges.First( h => h.PersonId == newPersonId.Value && h.GroupId == newGroupId.Value );

                        historyItem.PersonHistoryChangeList.AddChange( History.HistoryVerb.AddedToGroup, History.HistoryChangeType.Record, $"'{group.Name}' Group" );
                        History.EvaluateChange( historyItem.PersonHistoryChangeList, $"{historyItem.Caption} Role", ( int? ) null, Entity.GroupRole, Entity.GroupRoleId, rockContext );
                        History.EvaluateChange( historyItem.PersonHistoryChangeList, $"{historyItem.Caption} Note", string.Empty, Entity.Note );
                        History.EvaluateChange( historyItem.PersonHistoryChangeList, $"{historyItem.Caption} Status", null, Entity.GroupMemberStatus );
                        History.EvaluateChange( historyItem.PersonHistoryChangeList, $"{historyItem.Caption} Communication Preference", null, Entity.CommunicationPreference );
                        History.EvaluateChange( historyItem.PersonHistoryChangeList, $"{historyItem.Caption} Guest Count", ( int? ) null, Entity.GuestCount );

                        var addedMemberPerson = Entity.Person ?? new PersonService( rockContext ).Get( Entity.PersonId );

                        // add the Person's Name as ValueName and Caption (just in case the groupmember record is deleted later)
                        historyItem.GroupMemberHistoryChangeList.AddChange( History.HistoryVerb.AddedToGroup, History.HistoryChangeType.Record, $"{addedMemberPerson?.FullName}" ).SetCaption( $"{addedMemberPerson?.FullName}" );
                        History.EvaluateChange( historyItem.GroupMemberHistoryChangeList, $"Role", ( int? ) null, Entity.GroupRole, Entity.GroupRoleId, rockContext );
                        History.EvaluateChange( historyItem.GroupMemberHistoryChangeList, $"Note", string.Empty, Entity.Note );
                        History.EvaluateChange( historyItem.GroupMemberHistoryChangeList, $"Status", null, Entity.GroupMemberStatus );
                        History.EvaluateChange( historyItem.GroupMemberHistoryChangeList, $"Communication Preference", null, Entity.CommunicationPreference );
                        History.EvaluateChange( historyItem.GroupMemberHistoryChangeList, $"Guest Count", ( int? ) null, Entity.GuestCount );
                    }

                    if ( newPersonId.HasValue && oldPersonId.HasValue && oldPersonId.Value == newPersonId.Value &&
                         newGroupId.HasValue && oldGroupId.HasValue && oldGroupId.Value == newGroupId.Value )
                    {
                        // Updated same person in group
                        var historyItem = HistoryChanges.First( h => h.PersonId == newPersonId.Value && h.GroupId == newGroupId.Value );
                        History.EvaluateChange( historyItem.PersonHistoryChangeList, $"{historyItem.Caption} Role", OriginalValues["GroupRoleId"].ToStringSafe().AsIntegerOrNull(), Entity.GroupRole, Entity.GroupRoleId, rockContext );
                        History.EvaluateChange( historyItem.PersonHistoryChangeList, $"{historyItem.Caption} Note", OriginalValues["Note"].ToStringSafe(), Entity.Note );
                        History.EvaluateChange( historyItem.PersonHistoryChangeList, $"{historyItem.Caption} Status", OriginalValues["GroupMemberStatus"].ToStringSafe().ConvertToEnum<GroupMemberStatus>(), Entity.GroupMemberStatus );
                        History.EvaluateChange( historyItem.PersonHistoryChangeList, $"{historyItem.Caption} Communication Preference", OriginalValues["CommunicationPreference"].ToStringSafe().ConvertToEnum<CommunicationType>(), Entity.CommunicationPreference );
                        History.EvaluateChange( historyItem.PersonHistoryChangeList, $"{historyItem.Caption} Guest Count", OriginalValues["GuestCount"].ToStringSafe().AsIntegerOrNull(), Entity.GuestCount );
                        History.EvaluateChange( historyItem.PersonHistoryChangeList, $"{historyItem.Caption} Archived", OriginalValues["IsArchived"].ToStringSafe().AsBoolean(), Entity.IsArchived );

                        // If the groupmember was Archived, make sure it is the first GroupMember History change (since they get summarized when doing a HistoryLog and Timeline
                        bool origIsArchived = OriginalValues["IsArchived"].ToStringSafe().AsBoolean();

                        if ( origIsArchived != Entity.IsArchived )
                        {
                            var memberPerson = Entity.Person ?? new PersonService( rockContext ).Get( Entity.PersonId );
                            if ( Entity.IsArchived == true )
                            {
                                // GroupMember changed to Archived
                                historyItem.GroupMemberHistoryChangeList.AddChange( History.HistoryVerb.RemovedFromGroup, History.HistoryChangeType.Record, $"{memberPerson?.FullName}" ).SetCaption( $"{memberPerson?.FullName}" );
                            }
                            else
                            {
                                // GroupMember changed to Not archived
                                historyItem.GroupMemberHistoryChangeList.AddChange( History.HistoryVerb.AddedToGroup, History.HistoryChangeType.Record, $"{memberPerson?.FullName}" ).SetCaption( $"{memberPerson?.FullName}" );
                            }
                        }

                        History.EvaluateChange( historyItem.GroupMemberHistoryChangeList, $"Role", OriginalValues["GroupRoleId"].ToStringSafe().AsIntegerOrNull(), Entity.GroupRole, Entity.GroupRoleId, rockContext );
                        History.EvaluateChange( historyItem.GroupMemberHistoryChangeList, $"Note", OriginalValues["Note"].ToStringSafe(), Entity.Note );
                        History.EvaluateChange( historyItem.GroupMemberHistoryChangeList, $"Status", OriginalValues["GroupMemberStatus"].ToStringSafe().ConvertToEnum<GroupMemberStatus>(), Entity.GroupMemberStatus );
                        History.EvaluateChange( historyItem.GroupMemberHistoryChangeList, $"Communication Preference", OriginalValues["CommunicationPreference"].ToStringSafe().ConvertToEnum<CommunicationType>(), Entity.CommunicationPreference );
                        History.EvaluateChange( historyItem.GroupMemberHistoryChangeList, $"Guest Count", OriginalValues["GuestCount"].ToStringSafe().AsIntegerOrNull(), Entity.GuestCount );
                    }

                    if ( oldPersonId.HasValue && oldGroupId.HasValue &&
                        ( !newPersonId.HasValue || newPersonId.Value != oldPersonId.Value || !newGroupId.HasValue || newGroupId.Value != oldGroupId.Value ) )
                    {
                        // Removed a person/groupmember in group
                        var historyItem = HistoryChanges.First( h => h.PersonId == oldPersonId.Value && h.GroupId == oldGroupId.Value );

                        historyItem.PersonHistoryChangeList.AddChange( History.HistoryVerb.RemovedFromGroup, History.HistoryChangeType.Record, $"{oldGroupName} Group" );

                        var deletedMemberPerson = Entity.Person ?? new PersonService( rockContext ).Get( Entity.PersonId );

                        historyItem.GroupMemberHistoryChangeList.AddChange( History.HistoryVerb.RemovedFromGroup, History.HistoryChangeType.Record, $"{deletedMemberPerson?.FullName}" ).SetCaption( $"{deletedMemberPerson?.FullName}" );
                    }

                    // process universal search indexing if required
                    var groupType = GroupTypeCache.Get( group.GroupTypeId );
                    if ( groupType != null && groupType.IsIndexEnabled )
                    {
                        var processEntityTypeIndexMsg = new ProcessEntityTypeIndex.Message
                        {
                            EntityTypeId = groupType.Id,
                            EntityId = group.Id
                        };

                        processEntityTypeIndexMsg.Send();
                    }
                }

                base.PreSave();
            }

            /// <summary>
            /// Called after the save operation has been executed
            /// </summary>
            /// <remarks>
            /// This method is only called if <see cref="M:Rock.Data.EntitySaveHook`1.PreSave" /> returns
            /// without error.
            /// </remarks>
            protected override void PostSave()
            {
                var rockContext = ( RockContext ) this.RockContext;
                if ( HistoryChanges != null )
                {
                    foreach ( var historyItem in HistoryChanges )
                    {
                        int personId = historyItem.PersonId > 0 ? historyItem.PersonId : Entity.PersonId;

                        // if GroupId is 0, it is probably a Group that wasn't saved yet, so get the GroupId from historyItem.Group.Id instead
                        if ( historyItem.GroupId == 0 )
                        {
                            historyItem.GroupId = historyItem.Group?.Id;
                        }

                        var changes = HistoryService.GetChanges(
                            typeof( Person ),
                            Rock.SystemGuid.Category.HISTORY_PERSON_GROUP_MEMBERSHIP.AsGuid(),
                            personId,
                            historyItem.PersonHistoryChangeList,
                            historyItem.Caption,
                            typeof( Group ),
                            historyItem.GroupId,
                            Entity.ModifiedByPersonAliasId,
                            rockContext.SourceOfChange );

                        if ( changes.Any() )
                        {
                            Task.Run( async () =>
                            {
                                // Wait 1 second to allow all post save actions to complete
                                await Task.Delay( 1000 );
                                try
                                {
                                    using ( var insertRockContext = new RockContext() )
                                    {
                                        insertRockContext.BulkInsert( changes );
                                    }
                                }
                                catch ( SystemException ex )
                                {
                                    ExceptionLogService.LogException( ex, null );
                                }
                            } );
                        }

                        var groupMemberChanges = HistoryService.GetChanges(
                            typeof( GroupMember ),
                            Rock.SystemGuid.Category.HISTORY_GROUP_CHANGES.AsGuid(),
                            Entity.Id,
                            historyItem.GroupMemberHistoryChangeList,
                            historyItem.Caption,
                            typeof( Group ),
                            historyItem.GroupId,
                            Entity.ModifiedByPersonAliasId,
                            rockContext.SourceOfChange );

                        if ( groupMemberChanges.Any() )
                        {
                            Task.Run( async () =>
                            {
                                // Wait 1 second to allow all post save actions to complete
                                await Task.Delay( 1000 );
                                try
                                {
                                    using ( var insertRockContext = new RockContext() )
                                    {
                                        insertRockContext.BulkInsert( groupMemberChanges );
                                    }
                                }
                                catch ( SystemException ex )
                                {
                                    ExceptionLogService.LogException( ex, null );
                                }
                            } );
                        }
                    }
                }

                base.PostSave();

                // if this is a GroupMember record on a Family, ensure that AgeClassification, PrimaryFamily,
                // GivingLeadId, and GroupSalution is updated
                // NOTE: This is also done on Person.PostSaveChanges in case Birthdate changes
                var groupTypeFamilyRoleIds = GroupTypeCache.GetFamilyGroupType()?.Roles?.Select( a => a.Id ).ToList();
                if ( groupTypeFamilyRoleIds?.Any() == true )
                {
                    if ( groupTypeFamilyRoleIds.Contains( Entity.GroupRoleId ) )
                    {
                        PersonService.UpdatePersonAgeClassification( Entity.PersonId, rockContext );
                        PersonService.UpdatePrimaryFamily( Entity.PersonId, rockContext );
                        PersonService.UpdateGivingLeaderId( Entity.PersonId, rockContext );

                        // NOTE, make sure to do this after UpdatePrimaryFamily
                        PersonService.UpdateGroupSalutations( Entity.PersonId, rockContext );
                    }
                }
            }
        }
    }
}