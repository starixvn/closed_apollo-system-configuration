var MailingList = function ($) {
    return {
        ListFunctionId: null,
        FunctionId: null,
        SiteId: null,

        init: function () {
            $('.summary').hide();

            MailingList.ListFunctionId = $("#ListFunctionId")
            MailingList.FunctionId = $("#FunctionId");
            MailingList.SiteId = $("#SiteId");

            MailingList.SiteId.change(function () {
                var environment = "";
                if ($("#Environment").val() == undefined) {
                    environment = $('input[name=Environment]:checked').val();
                }
                else {
                    environment = $("#Environment").val();
                }

                MailingList.getFunctionBySite(environment, $("#SiteId").val());
                MailingList.getMailFunctionByEnvironment(environment, $("#SiteId").val());
            });

            $('input[type=radio]').click(function () {
                var environment = $(this).val();
                var siteId = $('#SiteId').val();
                MailingList.getFunctionBySite(environment, siteId);
                MailingList.getMailFunctionByEnvironment(environment, siteId);
            });

            $(".input-to").keydown(function (e) {
                var keyCode = e.which || e.keyCode;
                var object = $(this);

                if (keyCode == 8) {
                    if (object.val().length == 0) {
                        var objectRemove = object.parent().prev();
                        if (objectRemove.hasClass("search-choice-focus")) {
                            objectRemove.remove();
                        }
                        else {
                            objectRemove.addClass("search-choice-focus");
                        }
                    }
                }
                else {
                    var objectRemove = object.parent().prev();
                    objectRemove.removeClass("search-choice-focus");

                    if (keyCode == 13 || keyCode == 188) {
                        MailingList.autoCompleteEmail(object);
                        e.preventDefault();
                    }
                }

                var width = 25 + (object.val().length * 7); // 25: is width default input
                object.width(width);
            });

            $(".input-to").keyup(function (e) {
                var object = $(this);
                var width = 25 + (object.val().length * 7); // 25: is width default input
                object.width(width);
            });

            $(".input-to").blur(function () {
                var object = $(this);
                var objectRemove = object.parent().prev();
                objectRemove.removeClass("search-choice-focus");
            });

            $(".search-choice-close").live("click", function () {
                var object = $(this);
                object.parent().remove();
            });

            $(".chosen-choices").click(function () {
                $(this).children(".search-field").first().children("input").first().focus();
            });

            $("#btnSubmit").click(function (e) {
                var toList = "";
                var ccList = "";
                var bccList = "";
                // Site.removeErrorMsg();

                $('.input-to').each(function () {
                    var object = $(this);
                    MailingList.autoCompleteEmail(object);
                });

                $("#to-list > ul > .search-choice").each(function () {
                    var object = $(this);
                    if (!object.hasClass("invalid-email")) {
                        toList += $.trim(object.children("span").first().html()) + "; ";
                    }
                });

                $("#cc-list > ul > .search-choice").each(function () {
                    var object = $(this);
                    if (!object.hasClass("invalid-email")) {
                        ccList += $.trim(object.children("span").first().html()) + "; ";
                    }
                });

                $("#bcc-list > ul > .search-choice").each(function () {
                    var object = $(this);
                    if (!object.hasClass("invalid-email")) {
                        bccList += $.trim(object.children("span").first().html()) + "; ";
                    }
                });

                toList = toList.length > 0 ? toList.substr(0, (toList.length - 2)) : "";
                ccList = ccList.length > 0 ? ccList.substr(0, (ccList.length - 2)) : "";
                bccList = bccList.length > 0 ? bccList.substr(0, (bccList.length - 2)) : "";

                console.log(toList);

                $("#ToList").val(toList);
                $("#CCList").val(ccList);
                $("#BCCList").val(bccList);

                // e.preventDefault();
                //                var url = "MailingList/AddNew?"
                //                + "tolist=" + encodeURIComponent(toList)
                //                + "&cclist=" + encodeURIComponent(ccList)
                //                + "&bcclist=" + encodeURIComponent(bccList);

                //                $.ajax({
                //                    url: NAP.resolveClientUrl(url),
                //                    type: 'POST',
                //                    dataType: "json",
                //                    beforeSend: function () {
                //                        // lock ui
                //                    },
                //                    success: function (data) {
                //                        $('.summary').show();
                //                        if (data) {
                //                            if (data.ErrorCode == 0) {
                //                                var successUpdate = data.ErrorMsg;
                //                                Site.showSuccessMsg(successUpdate, 'MailingList/Index', 1000);
                //                            } else {
                //                                Site.showErrorMsg(data.ErrorMsg);
                //                            }
                //                        }
                //                    },
                //                    error: function (jqXHR, textStatus, errorThrown) {
                //                        alert(textStatus + ': ' + errorThrown);
                //                    }
                //                });
            });
        },
        validateEmail: function (email) {
            var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            return re.test(email);
        },
        autoCompleteEmail: function (object) {
            var value = $.trim(object.val());
            if (value.length > 0) {
                var parent = object.parent();

                var tempEmail = value.split(';');
                for (var i = 0; i < tempEmail.length; i++) {

                    var arrEmail = tempEmail[i].split(',');
                    for (var j = 0; j < arrEmail.length; j++) {
                        var emailItem = $.trim(arrEmail[j]);
                        if (emailItem.length > 0) {

                            var email = '<li class="search-choice"><span>' + emailItem + '</span><a class="search-choice-close"></a></li>';

                            if (!MailingList.validateEmail(emailItem)) {
                                email = '<li class="search-choice invalid-email"><span>' + emailItem + '</span><a class="search-choice-close"></a></li>';
                            }

                            parent.before(email);
                        }
                    }
                }

                object.val("");
                object.width(25);
                object.focus();
            }
        },
        getFunctionBySite: function (environment, siteId) {
            var url = NAP.resolveClientUrl('MailingList/GetFunctionBySite?siteId=' + siteId + '&environment=' + environment);

            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'html',
                cache: false,
                async: false,
                success: function (result) {
                    var content = eval(result);

                    MailingList.FunctionId.html('');

                    if (content.length > 0) {
                        MailingList.FunctionId.attr('disabled', false);
                        MailingList.FunctionId.append($('#DropdownListTpl').render(content));
                    }
                    else {
                        MailingList.FunctionId.attr('disabled', true);
                    }
                },
                error: function (xhr, status, err) {
                    NAP.handleAjaxError(xhr, status, err);
                }
            });
        },
        getMailFunctionByEnvironment: function (environment, siteId) {
            var url = NAP.resolveClientUrl('MailingList/GetMailFunctionByEnvironment?siteId=' + siteId + '&environment=' + environment);

            $.ajax({
                url: url,
                type: 'POST',
                dataType: 'JSON',
                cache: false,
                async: false,
                success: function (result) {
                    var content = eval(result);

                    MailingList.ListFunctionId.html('');

                    if (content.length > 0) {
                        MailingList.ListFunctionId.attr('disabled', false);
                        MailingList.ListFunctionId.append($('#DropdownListTpl').render(content));
                    }
                    else {
                        MailingList.ListFunctionId.attr('disabled', true);
                    }
                },
                error: function (xhr, status, err) {
                    NAP.handleAjaxError(xhr, status, err);
                }
            });
        }
    };
} (jQuery);

$(document).ready(function () {
    MailingList.init();
});

