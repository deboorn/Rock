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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;
using Rock.Tests.Shared;

namespace Rock.Tests.Integration.TestData
{
    public static partial class TestDataHelper
    {
        #region Person Data

        /// <summary>
        /// Get a test person record.
        /// </summary>
        /// <param name="personGuid">A Guid string, select from the set of values defined in TestGuids.TestPeople.</param>
        /// <returns></returns>
        public static Person GetTestPerson( string personGuid )
        {
            var rockContext = new RockContext();

            var guid = new Guid( personGuid );

            var person = new PersonService( rockContext ).Queryable()
                .FirstOrDefault( x => x.Guid == guid );

            Assert.That.IsNotNull( person, "Test person not found in current database." );

            return person;
        }

        private static List<PersonIdPersonAliasId> _PersonIdToAliasIdMap = null;

        /// <summary>
        /// Gets a list of the Id and AliasId of all Person records in the database
        /// </summary>
        /// <param name="dataContext"></param>
        /// <returns></returns>
        public static List<PersonIdPersonAliasId> GetPersonIdWithAliasIdList()
        {
            if ( _PersonIdToAliasIdMap == null )
            {
                var aliasService = new PersonAliasService( new RockContext() );

                _PersonIdToAliasIdMap = aliasService.Queryable()
                    .Where( x => !x.Person.IsSystem )
                    .GroupBy( x => x.PersonId )
                    .Select( a => new PersonIdPersonAliasId
                    {
                        PersonId = a.Key,
                        PersonAliasId = a.FirstOrDefault().Id
                    } )
                    .ToList();
            }

            return _PersonIdToAliasIdMap;
        }

        /// <summary>
        /// A hardcoded Device Id of 2 (Main Campus Checkin)
        /// </summary>
        public const int KioskDeviceId = 2;

        public class PersonIdPersonAliasId
        {
            public int PersonId { get; set; }
            public int PersonAliasId { get; set; }
        }

        #endregion

        private static Random randomGenerator = new Random();
        public static DateTime GetRandomDateInRange( DateTime minDate, DateTime maxDate )
        {
            var range = ( maxDate - minDate ).Days;

            return minDate.AddDays( randomGenerator.Next( range ) );
        }

        /// <summary>
        /// Returns an ordered list of random dates within a specified range.
        /// </summary>
        /// <param name="minDate"></param>
        /// <param name="maxDate"></param>
        /// <param name="dateCount"></param>
        /// <returns></returns>
        public static List<DateTime> GetRandomDateTimesInRange( DateTime minDate, DateTime maxDate, int dateCount )
        {
            var totalSeconds = Convert.ToInt32( ( maxDate - minDate ).TotalSeconds );

            var dateList = new List<DateTime>();
            while ( dateCount > 0 )
            {
                dateList.Add( minDate.AddSeconds( randomGenerator.Next( totalSeconds ) ) );
                dateCount--;
            }

            dateList.Sort();

            return dateList;
        }

        public static Dictionary<int, DateTime> GetAnalyticsSourceDateTestData()
        {
            return new Dictionary<int, DateTime>
            {
                {20100131,  Convert.ToDateTime("1/31/2010")},
                {20101231,  Convert.ToDateTime("12/31/2010")},
                {20101201,  Convert.ToDateTime("12/1/2010")},
                {20100101,  Convert.ToDateTime("1/1/2010")},
                {20160229,  Convert.ToDateTime("02/29/2016")},
            };
        }

        public static DateTime GetAnalyticsSourceMinDateForYear( RockContext rockContext, int year )
        {
            if ( !rockContext.AnalyticsSourceDates.AsQueryable().Any() )
            {
                var analyticsStartDate = new DateTime( RockDateTime.Today.AddYears( -150 ).Year, 1, 1 );
                var analyticsEndDate = new DateTime( RockDateTime.Today.AddYears( 101 ).Year, 1, 1 ).AddDays( -1 );
                Rock.Model.AnalyticsSourceDate.GenerateAnalyticsSourceDateData( 1, false, analyticsStartDate, analyticsEndDate );
            }

            return rockContext.Database.SqlQuery<DateTime>( $"SELECT MIN([Date]) FROM AnalyticsSourceDate WHERE CalendarYear = {year}" ).First();
        }

        public static DateTime GetAnalyticsSourceMaxDateForYear( RockContext rockContext, int year )
        {
            return rockContext.Database.SqlQuery<DateTime>( $"SELECT MAX([Date]) FROM AnalyticsSourceDate WHERE CalendarYear = {year}" ).First();
        }

        #region Attribute Helpers
        
        /// <summary>
        /// Sets the value of an existing <see cref="AttributeValue"/> and saves it to the database or creates a new database record if one doesn't already exist.
        /// </summary>
        /// <param name="attributeGuid">The parent <see cref="Rock.Model.Attribute"/> unique identifier.</param>
        /// <param name="entityId">The ID of the entity - if any - to which this <see cref="AttributeValue"/> belongs.</param>
        /// <param name="value">The value to be set.</param>
        /// <param name="previousValue">If a <see cref="AttributeValue"/> already exists in the database, it's current value will be returned, so you can set it back after the current tests complete.</param>
        /// <param name="newAttributeValueGuid">If a <see cref="AttributeValue"/> doesn't already exist in the database, the <see cref="Guid"/> of the newly-created record will be returned, so you can delete it after the current tests complete.</param>
        public static void SetAttributeValue( Guid attributeGuid, int? entityId, string value, out string previousValue, out Guid newAttributeValueGuid )
        {
            using ( var rockContext = new RockContext() )
            {
                previousValue = null;
                newAttributeValueGuid = Guid.Empty;

                var attributeId = AttributeCache.GetId( attributeGuid );
                if ( !attributeId.HasValue )
                {
                    return;
                }

                var attributeValueService = new AttributeValueService( rockContext );

                var attributeValue = attributeValueService.GetByAttributeIdAndEntityId( attributeId.Value, entityId );

                if ( attributeValue == null )
                {
                    attributeValue = new AttributeValue
                    {
                        AttributeId = attributeId.Value,
                        EntityId = entityId,
                        Value = value
                    };

                    attributeValueService.Add( attributeValue );

                    // Remember this so we can delete this AttributeValue upon cleanup.
                    newAttributeValueGuid = attributeValue.Guid;
                }
                else
                {
                    // Remeber this so we can set it back upon cleanup.
                    previousValue = attributeValue.Value;
                    attributeValue.Value = value;
                }

                rockContext.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes the attribute value.
        /// </summary>
        /// <param name="attributeValueGuid">The attribute value unique identifier.</param>
        public static void DeleteAttributeValue( Guid attributeValueGuid )
        {
            using ( var rockContext = new RockContext() )
            {
                var attributeValueService = new AttributeValueService( rockContext );
                var attributeValue = attributeValueService.Get( attributeValueGuid );
                if ( attributeValue == null )
                {
                    return;
                }

                attributeValueService.Delete( attributeValue );
                rockContext.SaveChanges();
            }
        }

        #endregion

        #region Campus

        public static string MainCampusGuidString = "76882AE3-1CE8-42A6-A2B6-8C0B29CF8CF8";
        public static string SecondaryCampusGuidString = "089844AF-6310-4C20-9434-A845F982B0C5";
        public static string SecondaryCampusName = "Stepping Stone";

        public static Campus GetOrAddCampusSteppingStone( RockContext rockContext )
        {
            // Add a new campus
            var campusService = new CampusService( rockContext );

            var campus2 = campusService.Get( SecondaryCampusGuidString.AsGuid() );

            if ( campus2 == null )
            {
                campus2 = new Campus();

                campusService.Add( campus2 );
            }

            campus2.Name = SecondaryCampusName;
            campus2.Guid = SecondaryCampusGuidString.AsGuid();
            campus2.IsActive = true;
            campus2.CampusStatusValueId = DefinedValueCache.GetId( SystemGuid.DefinedValue.CAMPUS_STATUS_OPEN.AsGuid() );
            campus2.CampusTypeValueId = DefinedValueCache.GetId( SystemGuid.DefinedValue.CAMPUS_TYPE_PHYSICAL.AsGuid() );

            rockContext.SaveChanges();

            return campus2;
        }

        #endregion

        private static RockContext GetActiveRockContext( RockContext rockContext )
        {
            return rockContext ?? new RockContext();
        }

    }
}
