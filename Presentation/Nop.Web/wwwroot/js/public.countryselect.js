/*
** nopCommerce country select js functions
*/
+function ($)
{
    'use strict';
    if ('undefined' == typeof (jQuery))
    {
        throw new Error('jQuery JS required');
    }
    function countrySelectHandler()
    {
        var $this = $(this);
        var selectedItem = $this.val();
        var stateProvince = $($this.data('stateprovince'));

        if(stateProvince.length == 0)
          return;

        var loading = $($this.data('loading'));
        loading.show();
        $.ajax({
            cache: false,
            type: "GET",
            url: $this.data('url'),
            data: { 
              'countryId': selectedItem,
              'addSelectStateItem': "true" 
            },
            success: function (data, textStatus, jqXHR) {
                stateProvince.html('');
                $.each(data,
                    function (id, option) {
                        stateProvince.append($('<option></option>').val(option.id).html(option.name));
                    });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Failed to retrieve states.');
            },
            complete: function(jqXHR, textStatus) {
              var stateId = (typeof Billing !== "undefined") ? Billing.selectedStateId : (typeof CheckoutBilling !== "undefined") ? CheckoutBilling.selectedStateId : 0;
              var element = $('#' + stateProvince[0].id + ' option[value=' + stateId + ']').prop('selected', true);
              element.parent().trigger('change');
              loading.hide();
            }
        });
    }

    if ($(document).has('[data-trigger="country-select"]')) {
        $('select[data-trigger="country-select"]').change(countrySelectHandler);
    }

    function stateSelectHandler()
    {
        var $this = $(this);
        var selectedItem = $this.val();
        var thana = $($this.data('thana'));

        if (thana.length == 0)
          return;

        var loading = $($this.data('loading'));
        loading.show();
        $.ajax({
            cache: false,
            type: "GET",
            url: $this.data('url'),
            data: {
                'stateId': selectedItem,
                'addSelectThanaItem': "true"
            },
            success: function (data, textStatus, jqXHR) {
                thana.html('');
                $.each(data,
                    function (id, option) {
                      thana.append($('<option></option>').val(option.id).html(option.name));
                    });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Failed to retrieve thanas.');
            },
            complete: function (jqXHR, textStatus) {
                var thanaId = (typeof Billing !== "undefined") ? Billing.selectedThanaId : (typeof CheckoutBilling !== "undefined") ? CheckoutBilling.selectedThanaId : 0;
                var element = $('#' + thana[0].id + ' option[value=' + thanaId + ']').prop('selected', true);
                element.parent().trigger('change');

                loading.hide();
            }
        });
    }

    if ($(document).has('[data-trigger="state-select"]')) {
        $('select[data-trigger="state-select"]').change(stateSelectHandler);
    }

    function thanaSelectHandler()
    {
        var $this = $(this);
        var selectedItem = $this.val();
        var area = $($this.data('area'));

        if (area.length == 0)
          return;

        var loading = $($this.data('loading'));
        loading.show();
        $.ajax({
            cache: false,
            type: "GET",
            url: $this.data('url'),
            data: {
                'thanaId': selectedItem,
                'addSelectAreaItem': "true"
            },
            success: function (data, textStatus, jqXHR) {
                area.html('');
                $.each(data,
                    function (id, option) {
                      area.append($('<option></option>').val(option.id).html(option.name));
                    });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Failed to retrieve areas.');
            },
            complete: function (jqXHR, textStatus) {
                var areaId = (typeof Billing !== "undefined") ? Billing.selectedAreaId : (typeof CheckoutBilling !== "undefined") ? CheckoutBilling.selectedAreaId : 0;
                var element = $('#' + area[0].id + ' option[value=' + areaId + ']').prop('selected', true);
                element.parent().trigger('change');

                loading.hide();
            }
        });
      }

    if ($(document).has('[data-trigger="thana-select"]')) {
        $('select[data-trigger="thana-select"]').change(thanaSelectHandler);
    }

    $.fn.countrySelect = function () {
        this.each(function () {
            $(this).change(countrySelectHandler);
        });
    }
    $.fn.stateSelectSelect = function () {
        this.each(function () {
            $(this).change(stateSelectHandler);
        });
  }
    $.fn.thanaSelectSelect = function () {
        this.each(function () {
          $(this).change(thanaSelectHandler);
        });
    }
}(jQuery); 
