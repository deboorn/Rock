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
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Rock.Transactions
{
    /// <summary>
    /// Helpful base for transactions that operate on groups of items all at once.
    /// So even though you might add 100 instances to the RockQueue, the
    /// <see cref="Execute(IList{T})"/> method would only be called once and it
    /// would be passed all 100 instance values.
    /// </summary>
    /// <typeparam name="T">The type of the value used by the transaction.</typeparam>
    internal abstract class AggregateTransaction<T> : ITransaction, IQueuedTransaction
    {
        /// <summary>
        /// The set of items that have been queued up for processing.
        /// </summary>
        private static readonly ConcurrentQueue<T> _items = new ConcurrentQueue<T>();

        private readonly T _item;

        public AggregateTransaction( T item )
        {
            _item = item;
        }

        /// <inheritdoc/>
        public void WillEnqueue()
        {
            _items.Enqueue( _item );
        }

        /// <inheritdoc/>
        public void Execute()
        {
            List<T> items = new List<T>();

            // Get all the currently queued up items and then clear the queue.
            while ( _items.TryDequeue( out var item ) )
            {
                items.Add( item );
            }

            if ( items.Count > 0 )
            {
                Execute( items );
            }
        }

        /// <summary>
        /// Executes this instance with all the items that were found in the queue.
        /// </summary>
        /// <param name="items">The items that need to be processed.</param>
        protected abstract void Execute( IList<T> items );
    }
}
