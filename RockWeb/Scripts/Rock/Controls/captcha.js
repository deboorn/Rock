﻿(function ($) {
    'use strict';
    window.Rock = window.Rock || {};
    Rock.controls = Rock.controls || {};

    Rock.controls.captcha = (function () {
        var exports = {
            initialize: function (options) {
                if (!options.id) {
                    throw 'id is required';
                }
                if (!options.key) {
                    throw 'key is required';
                }

                $('#' + options.id).data('key', options.key);
                this.options = options;

                // Ensure that js for recaptcha is added to page.
                // If the Captcha control was added in a partial postback, we'll have to add it manually here
                if (!$('#captchaScriptId').length) {
                    // by default, jquery adds a cache-busting parameter on dynamically added script tags. set the ajaxSetup cache:true to prevent this
                    $.ajaxSetup({ cache: true });
                    const apiSource = "https://challenges.cloudflare.com/turnstile/v0/api.js?onload=onloadTurnstileCallback";
                    $('head').prepend("<script src='" + apiSource + "' async defer></script>");
                }

                // Partial postbacks sometimes cause the page to reload, and in the instance where the widget has already been rendered, the turnstile api will not re-render the widget
                // so if the turnstile code has already been injected, then use it to re-render the page
                if (typeof (turnstile) !== 'undefined') {
                    const widgetId = turnstile.render(`#${options.id}`, {
                        sitekey: options.key,
                        callback: function onloadTurnstileCallback(token) {

                            let retryCount = 3;
                            const hfToken = document.querySelector('.js-captchaToken');
                            // The callback is sometimes triggered before the element is loaded on the page, hence the retry after a second to try and give it time to load.
                            if (!hfToken) {
                                if (retryCount > 0) {
                                    retryCount--;
                                    setTimeout(() => onloadTurnstileCallback(token), 1000);
                                }
                            } else {
                                hfToken.value = token;
                            }
                        },
                    });
                }
            },
            onloadInitialize: function () {
                // Perform final initialization on all captchas.
                if (typeof (grecaptcha) !== 'undefined' && typeof (grecaptcha.render) !== 'undefined') {
                    $('.js-captcha').each(function () {
                        var $captcha = $(this);
                        var $validator = $captcha.closest('.form-group').find('.js-captcha-validator');

                        if ($captcha.data('captcha-id') == undefined) {
                            var verifyCallback = function (response) {
                                ValidatorValidate(window[ $validator.prop('id') ]);
                            };
                            var expiredCallback = function (response) {
                                ValidatorValidate(window[ $validator.prop('id') ]);
                            };

                            var id = grecaptcha.render($captcha.prop('id'), {
                                'sitekey': $captcha.data('key'),
                                'callback': verifyCallback,
                                'expired-callback': expiredCallback
                            });

                            $captcha.data('captcha-id', id);
                        }
                    });
                }
            },
            clientValidate: function (validator, args) {
                if (typeof (turnstile) !== 'undefined') {
                    var $captcha = $(validator).closest('.form-group').find('.js-captcha');
                    var required = $captcha.data('required') == true;

                    const widget = document.getElementById(this.options.id);
                    const widgetId = turnstile.render(widget);
                    const widgetResponse = turnstile.getResponse(widgetId);

                    var isValid = !required || widgetResponse !== null;

                    if (isValid) {
                        $captcha.closest('.form-group').removeClass('has-error');
                        args.IsValid = true;
                    }
                    else {
                        $captcha.closest('.form-group').addClass('has-error');
                        args.IsValid = false;
                        validator.errormessage = $captcha.data('required-error-message');
                    }
                }
            },
            options: {

            }
        };

        return exports;
    }());
}(jQuery));
