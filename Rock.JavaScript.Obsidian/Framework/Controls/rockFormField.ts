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

import { standardRockFormFieldProps } from "@Obsidian/Utility/component";
import { useFormState } from "@Obsidian/Utility/form";
import { newGuid } from "@Obsidian/Utility/guid";
import { normalizeRules, validateValue } from "@Obsidian/ValidationRules";
import { computed, defineComponent, onBeforeUnmount, PropType, ref, watch } from "vue";
import RockLabel from "./rockLabel";

export default defineComponent({
    name: "RockFormField",

    inheritAttrs: false,

    components: {
        RockLabel
    },

    compilerOptions: {
        whitespace: "preserve"
    },

    props: {
        modelValue: {
            required: true
        },

        name: {
            type: String as PropType<string>,
            required: true
        },

        ...standardRockFormFieldProps
    },

    setup(props) {
        /** The reactive state of the form. */
        const formState = useFormState();

        /** The unique identifier used to identify this form field. */
        const uniqueId = `rock-${props.name}-${newGuid()}`;

        /** The internal value being tracked for the field. */
        const internalValue = ref<unknown>("");

        /** The internal rules we will be used for calculations. */
        const internalRules = computed(() => normalizeRules(props.rules));

        /** Determines if this field is marked as required. */
        const isRequired = computed((): boolean => internalRules.value.includes("required"));

        /** Holds the current error message for this form field. */
        const currentError = ref("");

        /** Any error classes to be applied to the field depending on the current state. */
        const errorClasses = computed((): string[] => {
            if (!formState || formState.submitCount < 1) {
                return [];
            }

            return currentError.value !== "" ? ["has-error"] : [];
        });

        /** The text label to display to the user which identifies this field. */
        const fieldLabel = computed((): string => {
            return props.validationTitle || props.label;
        });

        // Watch for changes to the modelValue and update our internalValue.
        watch(() => [props.modelValue, props.rules], () => {
            internalValue.value = props.modelValue;

            const errors = validateValue(internalValue.value, props.rules);

            if (errors.length > 0) {
                currentError.value = errors[0];
                formState?.setError(uniqueId, fieldLabel.value, currentError.value);
            }
            else {
                currentError.value = "";
                formState?.setError(uniqueId, fieldLabel.value, "");
            }
        }, {
            immediate: true
        });

        // If we are removed from the DOM completely, clear the error before we go.
        onBeforeUnmount(() => {
            currentError.value = "";
            formState?.setError(uniqueId, fieldLabel.value, "");
        });

        return {
            errorClasses,
            fieldLabel,
            formState,
            isRequired,
            uniqueId,
        };
    },

    template: `
<slot name="pre" />
<div v-if="label || help" class="form-group" :class="[classAttr, formGroupClasses, isRequired && !isRequiredIndicatorHidden ? 'required' : '', errorClasses]">
    <RockLabel :for="uniqueId" :help="help">
        {{label}}
    </RockLabel>
    <slot v-bind="{field: $attrs, uniqueId, errors, fieldLabel}" />
</div>
<slot v-else v-bind="{field: $attrs, uniqueId, errors, fieldLabel}" />
<slot name="post" />
`
});
