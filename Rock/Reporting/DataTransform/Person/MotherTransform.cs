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
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;

namespace Rock.Reporting.DataTransform.Person
{
    /// <summary>
    /// Person Mother Transformation
    /// </summary>
    [Description( "Transform result to Mother" )]
    [Export( typeof( DataTransformComponent ) )]
    [ExportMetadata( "ComponentName", "Person Mother Transformation" )]
    [Rock.SystemGuid.EntityTypeGuid( "91613E17-69F8-4ADF-AEF7-A40A3C948AD1")]
    public class MotherTransform : DataTransformComponent<Rock.Model.Person>
    {
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public override string Title
        {
            get { return "Mother"; }
        }

        /// <summary>
        /// Gets the name of the transformed entity type.
        /// </summary>
        /// <value>
        /// The name of the transformed entity type.
        /// </value>
        public override string TransformedEntityTypeName
        {
            get { return "Rock.Model.Person"; }
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <param name="serviceInstance">The service instance.</param>
        /// <param name="parameterExpression">The parameter expression.</param>
        /// <param name="whereExpression">The where expression.</param>
        /// <returns></returns>
        public override Expression GetExpression( IService serviceInstance, ParameterExpression parameterExpression, Expression whereExpression )
        {
            IQueryable<int> idQuery = serviceInstance.GetIds( parameterExpression, whereExpression );
            return BuildExpression( serviceInstance, idQuery, parameterExpression );
        }

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <param name="serviceInstance">The service instance.</param>
        /// <param name="personQueryable">The person queryable.</param>
        /// <param name="parameterExpression">The parameter expression.</param>
        /// <returns></returns>
        public override Expression GetExpression( IService serviceInstance, IQueryable<Rock.Model.Person> personQueryable, ParameterExpression parameterExpression )
        {
            return BuildExpression( serviceInstance, personQueryable.Select( p => p.Id ), parameterExpression );
        }

        /// <summary>
        /// Builds the expression.
        /// </summary>
        /// <param name="serviceInstance">The service instance.</param>
        /// <param name="idQuery">The id query.</param>
        /// <param name="parameterExpression">The parameter expression.</param>
        /// <returns></returns> 
        private Expression BuildExpression( IService serviceInstance, IQueryable<int> idQuery, ParameterExpression parameterExpression )
        {
            int adultRoleId = GroupTypeCache.GetFamilyGroupType().Roles.Where( a => a.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_ADULT.AsGuid() ).Select( a => a.Id ).FirstOrDefault();
            int childRoleId = GroupTypeCache.GetFamilyGroupType().Roles.Where( a => a.Guid == Rock.SystemGuid.GroupRole.GROUPROLE_FAMILY_MEMBER_CHILD.AsGuid() ).Select( a => a.Id ).FirstOrDefault();

            var qry = new PersonService( (RockContext)serviceInstance.Context ).Queryable()
                .Where( p => p.Gender == Gender.Female && p.Members.Where( a => a.GroupRoleId == adultRoleId )
                    .Any( a => a.Group.Members
                    .Any( c => c.GroupRoleId == childRoleId && idQuery.Contains( c.PersonId ) ) ) );

            return FilterExpressionExtractor.Extract<Rock.Model.Person>( qry, parameterExpression, "p" );
        }
    }
}