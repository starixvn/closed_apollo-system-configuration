var ExchRate = function ($) {
    return {
        initView: function () {
            $('.btn-edit').click(function () {
                $.ajax({
                    type: 'GET',
                    url: NAP.resolveClientUrl("ExchangeRate/Update/" + $(this).attr("data-id")),
                    dataType: 'html',
                    success: function (data) {
                        if (data) {
                            NAP.popup("Edit", data);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert(textStatus + ': ' + errorThrown);
                    }
                });
            });

            $('.btn-add').click(function () {
                $.ajax({
                    type: 'GET',
                    url: NAP.resolveClientUrl("ExchangeRate/insert"),
                    dataType: 'html',
                    success: function (data) {
                        if (data) {
                            NAP.popup("Add new", data);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert(textStatus + ': ' + errorThrown);
                    }
                });
            });

            $('.btn-export').click(function () {
                var url = "ExchangeRate/ExportExchangeRate";
                location.href = NAP.resolveClientUrl(url);
            });
        },
        initInsert: function () {
            ExchRate.initInsertOrUpdate(false);
        },
        initUpdate: function () {
            ExchRate.initInsertOrUpdate(true);
            $("#ActualRate").focus();
        },
        initInsertOrUpdate: function (isUpdate) {
            $('.summary').hide();

            $("#submit").click(function () {
                ExchRate.removeErrorMsg();

                var multipliedBy = $("#MultipliedBy").val();
                var actualRate = $("#ActualRate").val();
                var minBet = $("#MinBet").val();
                var minBetLive = $("#MinBetLive").val();

                var url = "";
                if (isUpdate) {
                    url = "ExchangeRate/Update" +
                    "?exchangeId=" + $("#ExchangeId").val() +
                    "&Currency=" + encodeURIComponent($("#Currency").val()) +
                    "&description=" + encodeURIComponent($("#Description").val()) +
                    "&multipliedBy=" + (multipliedBy.length > 0 ? (parseFloat(multipliedBy, true).toString() === 'NaN' ? -1 : parseFloat(multipliedBy, true)) : multipliedBy) +
                    "&actualRate=" + (actualRate.length > 0 ? (parseFloat(actualRate, true).toString() === 'NaN' ? -1 : parseFloat(actualRate, true)) : actualRate) +
                    "&minBet=" + (minBet.length > 0 ? (parseFloat(minBet, true).toString() === 'NaN' ? -1 : parseFloat(minBet, true)) : minBet) +
                    "&minBetLive=" + (minBetLive.length > 0 ? (parseFloat(minBetLive, true).toString() === 'NaN' ? -1 : parseFloat(minBetLive, true)) : minBetLive);
                }
                else {
                    url = "ExchangeRate/insert" +
                    "?currency=" + encodeURIComponent($("#Currency").val()) +
                    "&description=" + encodeURIComponent($("#Description").val()) +
                    "&multipliedBy=" + (multipliedBy.length > 0 ? (parseFloat(multipliedBy, true).toString() === 'NaN' ? -1 : parseFloat(multipliedBy, true)) : multipliedBy) +
                    "&actualRate=" + (actualRate.length > 0 ? (parseFloat(actualRate, true).toString() === 'NaN' ? -1 : parseFloat(actualRate, true)) : actualRate) +
                    "&minBet=" + (minBet.length > 0 ? (parseFloat(minBet, true).toString() === 'NaN' ? -1 : parseFloat(minBet, true)) : minBet) +
                    "&minBetLive=" + (minBetLive.length > 0 ? (parseFloat(minBetLive, true).toString() === 'NaN' ? -1 : parseFloat(minBetLive, true)) : minBetLive);
                }

                $.ajax({
                    url: NAP.resolveClientUrl(url),
                    type: 'POST',
                    dataType: "json",
                    beforeSend: function () {
                        // lock ui
                    },
                    success: function (data) {
                        $('.summary').show();
                        if (data) {
                            if (data.ErrorCode == 0) {
                                var successMsg = isUpdate ? "Update Success!!!" : "Insert Success!!!";
                                ExchRate.showSuccessMsg(successMsg, 'ExchangeRate/Index', 5000);
                            } else {
                                if (data.MsgCurrency.length > 0) {
                                    ExchRate.showErrorMsg(data.MsgCurrency, $("#Currency"));
                                }
                                if (data.MsgActualRate.length > 0) {
                                    ExchRate.showErrorMsg(data.MsgActualRate, $("#ActualRate"));
                                }
                                if (data.MsgMinBet.length > 0) {
                                    ExchRate.showErrorMsg(data.MsgMinBet, $("#MinBet"));
                                }
                                if (data.MsgMinBetLive.length > 0) {
                                    ExchRate.showErrorMsg(data.MsgMinBetLive, $("#MinBetLive"));
                                }
                                if (data.MsgMultipliedBy.length > 0) {
                                    ExchRate.showErrorMsg(data.MsgMultipliedBy, $("#MultipliedBy"));
                                }
                            }
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert(textStatus + ': ' + errorThrown);
                    }
                });
            });

            var minBet = document.getElementById("MinBet");
            minBet.onkeyup = function (event) { Fanex.FormatNumber(event, true, 15); };
            minBet.onkeypress = Fanex.HandleKeyDownInputNumber;

            var minBetLive = document.getElementById("MinBetLive");
            minBetLive.onkeyup = function (event) { Fanex.FormatNumber(event, true, 15); };
            minBetLive.onkeypress = Fanex.HandleKeyDownInputNumber;

            var multipliedBy = document.getElementById("MultipliedBy");
            multipliedBy.onkeyup = function (event) { Fanex.FormatNumber(event, true, 9); };
            multipliedBy.onkeypress = Fanex.HandleKeyDownInputNumber;

            var baseRate = document.getElementById("ActualRate");
            baseRate.onkeyup = function (event) { Fanex.FormatNumberAndDot(event, true); };
            baseRate.onkeypress = Fanex.HandleKeyDownInputNumberAndDot;

            if (isUpdate) {
                minBet.value = ''.FormatNumber(minBet.value);
                minBetLive.value = ''.FormatNumber(minBetLive.value);
                multipliedBy.value = ''.FormatNumber(multipliedBy.value);
                baseRate.value = ''.FormatNumber5Decimal(baseRate.value);
            }

            $("#MinBet").focus(function () {
                if ($("#MinBet").val() == 0) {
                    $("#MinBet").val("");
                }
            });

            $("#MinBetLive").focus(function () {
                if ($("#MinBetLive").val() == 0) {
                    $("#MinBetLive").val("");
                }
            });

            $("#ActualRate").focus(function () {
                if ($("#ActualRate").val() == 0) {
                    $("#ActualRate").val("");
                }
            });

            $("#MultipliedBy").focus(function () {
                if ($("#MultipliedBy").val() == 0) {
                    $("#MultipliedBy").val("");
                }
            });
        },
        showErrorMsg: function (msg, htmlObject) {
            $('.validation-summary-valid').addClass('validation-summary-errors');
            var msgWrp = $('.validation-summary-valid').find('ul:first');
            msgWrp.append("<li>" + NAP.htmlDecode(msg) + "</li>");
            htmlObject.addClass("input-validation-error");
        },
        removeErrorMsg: function (msg) {
            $('input').removeClass('input-validation-error');
            var msgWrp = $('.validation-summary-valid').removeClass('validation-summary-errors');
            msgWrp.find('ul:first').html("");
        },
        showSuccessMsg: function (msg, url, msec) {
            var msgWrp = $('.validation-summary-valid').addClass('success').find('ul:first');
            msgWrp.append("<li>" + NAP.htmlDecode(msg) + "</li>");
            NAP.showSuccessMsgCountDown(url, msec / 1000);
        }
    };
} (jQuery);