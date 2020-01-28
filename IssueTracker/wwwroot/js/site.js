// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
; (function ($) {

    'use strict';

    $('.alert[data-auto-dismiss]').each(function (index, element) {
        var $element = $(element),
            timeout = $element.data('auto-dismiss') || 5000;

        setTimeout(function () {
            //$element.alert('close');
            $element.fadeTo(1000, 500).slideUp(500, function () {
                $element.alert('close');
            });
        }, timeout);
    });

})(jQuery);
