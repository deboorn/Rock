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

import { PublicAttributeBag } from "@Obsidian/ViewModels/Utility/publicAttributeBag";

/** FinancialScheduledTransaction View Model */
export type FinancialScheduledTransactionBag = {
    /** Gets or sets the attributes. */
    attributes?: Record<string, PublicAttributeBag> | null;

    /** Gets or sets the attribute values. */
    attributeValues?: Record<string, string> | null;

    /** Gets or sets the authorized person alias identifier. */
    authorizedPersonAliasId: number;

    /** Gets or sets the date to remind user to update scheduled transaction. */
    cardReminderDate?: string | null;

    /** Gets or sets the created by person alias identifier. */
    createdByPersonAliasId?: number | null;

    /** Gets or sets the created date time. */
    createdDateTime?: string | null;

    /**
     * Gets or sets the end date for this transaction schedule. Transactions will cease to occur on or before this date.  This property is nullable for ongoing 
     * schedules or for schedules that will end after a specified number of payments/transaction occur (in the Rock.Model.FinancialScheduledTransaction.NumberOfPayments property).
     */
    endDate?: string | null;

    /** Gets or sets the gateway identifier. */
    financialGatewayId?: number | null;

    /** Gets or sets the financial payment detail identifier. */
    financialPaymentDetailId?: number | null;

    /** Gets or sets the foreign currency code value identifier. */
    foreignCurrencyCodeValueId?: number | null;

    /**
     * Gets or sets the payment gateway's payment schedule key/identifier.  This is the value that uniquely identifies the payment schedule on 
     * with the payment gateway.
     */
    gatewayScheduleId?: string | null;

    /** Gets or sets the identifier key of this entity. */
    idKey?: string | null;

    /** Gets or sets the inactivate date time. */
    inactivateDateTime?: string | null;

    /** Gets or sets a flag indicating if this scheduled transaction is active. */
    isActive: boolean;

    /** Gets or sets the date that user was last reminded to update scheduled transaction. */
    lastRemindedDate?: string | null;

    /**
     * Gets or sets the date and time of the last status update. In other words,
     * the date and time the gateway was last queried for the status of the scheduled profile/transaction.
     */
    lastStatusUpdateDateTime?: string | null;

    /** Gets or sets the modified by person alias identifier. */
    modifiedByPersonAliasId?: number | null;

    /** Gets or sets the modified date time. */
    modifiedDateTime?: string | null;

    /** Gets or sets the date of the next payment in this schedule. */
    nextPaymentDate?: string | null;

    /**
     * Gets or sets the maximum number of times that this payment should repeat in this schedule.  If there is not a set number of payments, this value will be null. 
     * This property is overridden by the schedule's Rock.Model.FinancialScheduledTransaction.EndDate.
     */
    numberOfPayments?: number | null;

    /**
     * The JSON for Rock.Model.FinancialScheduledTransaction.PreviousGatewayScheduleIds. If this is null,
     * there are no PreviousGatewayScheduleIds.
     */
    previousGatewayScheduleIdsJson?: string | null;

    /** Gets or sets the source type value identifier. */
    sourceTypeValueId?: number | null;

    /** Gets or sets the start date for this schedule. The first transaction will occur on or after this date. */
    startDate?: string | null;

    /**
     * The status of the scheduled transactions provided by the payment gateway (i.e. Active, Cancelled, etc).
     * If the gateway doesn't have a status field, this will be null;
     * The payment gateway component maps this based on the Rock.Model.FinancialScheduledTransaction.StatusMessage.
     */
    status?: number | null;

    /**
     * Gets or sets the raw scheduled transaction status message returned from the Gateway
     * If the gateway doesn't have a status field, this will be null;
     */
    statusMessage?: string | null;

    /** Gets or sets a summary of the scheduled transaction. This would store any comments made. */
    summary?: string | null;

    /** Gets or sets the transaction code used for this scheduled transaction. */
    transactionCode?: string | null;

    /**
     * Gets or sets the DefinedValueId of the transaction frequency Rock.Model.DefinedValue that represents the frequency that this 
     * transaction will occur.
     */
    transactionFrequencyValueId: number;

    /** Gets or sets the transaction type value identifier. */
    transactionTypeValueId?: number | null;
};
