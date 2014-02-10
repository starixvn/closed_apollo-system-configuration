/* global NAP: false */

"use strict";

var MailingList = (function ($) {
	return {
		ListFunctionId: null,
		FunctionId: null,
		SiteId: null,
		Regenerate: null,

		init: function () {
			// $('.summary').hide();

			MailingList.ListFunctionId = $("#ListFunctionId");
			MailingList.FunctionId = $("#FunctionId");
			MailingList.SiteId = $("#SiteId");
			MailingList.Regenerate = $("#btn-regen");

			MailingList.Regenerate.click(function () {
				var msg = "Are you sure you want to generate List Key from List Name?";
				NAP.headlineMsgBox.confirm(msg, function () {
					MailingList.getMailingListKey();
				}, { modal: true, closeBtn: false });
			});

			MailingList.SiteId.change(function () {
				var environment = "";
				if ($("#Environment").val() === undefined) {
					environment = $('input[name=Environment]:checked').val();
				}
				else {
					environment = $("#Environment").val();
				}

				MailingList.getFunctionBySite(environment, $("#SiteId").val());
			});

			$('input[type=radio]').click(function () {
				var environment = $(this).val();
				var siteId = $('#SiteId').val();
				MailingList.getFunctionBySite(environment, siteId);
				MailingList.getMailFunctionByEnvironment(environment);
			});

			$(".input-to").keydown(function (e) {
				var keyCode = e.which || e.keyCode;
				var object = $(this);
				var objectRemove;

				if (keyCode === 8) {
					if (object.val().length === 0) {
						objectRemove = object.parent().prev();
						if (objectRemove.hasClass("search-choice-focus")) {
							objectRemove.remove();
						}
						else {
							objectRemove.addClass("search-choice-focus");
						}
					}
				}
				else {
					objectRemove = object.parent().prev();
					objectRemove.removeClass("search-choice-focus");

					if (keyCode === 13 || keyCode === 188 || keyCode === 32 || keyCode === 59 || keyCode === 186) {
						MailingList.autoCompleteEmail(object);
						e.preventDefault();
					}
				}

				var width = 25 + (object.val().length * 7); // 25: is width default input
				object.width(width);
			});

			$(".input-to").keyup(function () {
				var object = $(this);
				var width = 25 + (object.val().length * 7); // 25: is width default input
				object.width(width);
				object.parent().parent().removeClass("input-validation-error");
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

			$("#btnSubmit").click(function () {
				var toList = "";
				var ccList = "";
				var bccList = "";
				var mailError = "";

				$('.input-to').each(function () {
					var object = $(this);
					MailingList.autoCompleteEmail(object);
				});

				$("#to-list > ul > .search-choice").each(function () {
					var object = $(this);
					if (!object.hasClass("invalid-email")) {
						toList += $.trim(object.children("span").first().html()) + "; ";
					}
					else {
						//mailError += $.trim(object.children("span").first().html()) + "; ";
						toList += $.trim(object.children("span").first().html()) + "; ";
					}
				});

				$("#cc-list > ul > .search-choice").each(function () {
					var object = $(this);
					if (!object.hasClass("invalid-email")) {
						ccList += $.trim(object.children("span").first().html()) + "; ";
					}
					else {
						mailError += $.trim(object.children("span").first().html()) + "; ";
						ccList += $.trim(object.children("span").first().html()) + "; ";
					}
				});

				$("#bcc-list > ul > .search-choice").each(function () {
					var object = $(this);
					if (!object.hasClass("invalid-email")) {
						bccList += $.trim(object.children("span").first().html()) + "; ";
					}
					else {
						mailError += $.trim(object.children("span").first().html()) + "; ";
						bccList += $.trim(object.children("span").first().html()) + "; ";
					}
				});

				toList = toList.length > 0 ? toList.substr(0, (toList.length - 2)) : "";
				ccList = ccList.length > 0 ? ccList.substr(0, (ccList.length - 2)) : "";
				bccList = bccList.length > 0 ? bccList.substr(0, (bccList.length - 2)) : "";
				mailError = mailError.length > 0 ? mailError.substr(0, (mailError.length - 2)) : "";

				$("#ToList").val(toList);
				$("#CCList").val(ccList);
				$("#BCCList").val(bccList);
			});

			MailingList.checkValidateMailTo();
		},
		/*jshint ignore:start */
		validateEmail: function (email) {
			var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
			return re.test(email);
		},
		/*jshint ignore:end */
		autoCompleteEmail: function (object) {
			var value = $.trim(object.val());
			if (value.length <= 0) {
				return;
			}

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
		},
		getFunctionBySite: function (environment, siteId) {
			var url = NAP.resolveClientUrl('MailingList/GetFunctionBySite?siteId=' + siteId + '&environment=' + environment);

			$.ajax({
				url: url,
				type: 'POST',
				dataType: 'json',
				cache: false,
				async: false,
				success: function (result) {
					MailingList.FunctionId.html('');

					if (result.length > 0) {
						MailingList.FunctionId.attr('disabled', false);
						MailingList.FunctionId.append($('#DropdownListTpl').render(result));
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
		getMailFunctionByEnvironment: function (environment) {
			var url = NAP.resolveClientUrl('MailingList/GetMailFunctionByEnvironment?environment=' + environment);

			$.ajax({
				url: url,
				type: 'POST',
				dataType: 'JSON',
				cache: false,
				async: false,
				success: function (result) {
					MailingList.ListFunctionId.html('');

					if (result.length > 0) {
						MailingList.ListFunctionId.attr('disabled', false);
						MailingList.ListFunctionId.append($('#DropdownListTpl').render(result));
					}
					else {
						MailingList.ListFunctionId.attr('disabled', true);
					}
				},
				error: function (xhr, status, err) {
					NAP.handleAjaxError(xhr, status, err);
				}
			});
		},
		getMailingListKey: function () {
			var listId = $('#ListId').val();
			var listName = $('#ListName').val();

			$.ajax({
				url: NAP.resolveClientUrl("MailingList/GetMailingListKey" + "?listId=" + listId + "&listName=" + listName),
				type: 'POST',
				contentType: 'application/json',
				dataType: 'json',
				cache: false,
				beforeSend: function () {
					//NAP.loadingBox.show();
				},
				complete: function () {
					//NAP.loadingBox.close();
				},
				success: function (result) {
					var template;

					switch (result) {
						case 0:
							$('#error').append("");
							template = '<div data-valmsg-summary="true" class="validation-summary-errors">' +
								'<ul><li>' + listName + ' already exists. Please choose another!</li></ul>' + '</div>';
							$('#error').append(template);
							$('#ListName').addClass('input-validation-error');
							break;

						case -1:
							break;

						case -2:
							$('#error').append("");
							template = '<div data-valmsg-summary="true" class="validation-summary-errors">' +
								'<ul><li>' + result + ' already exists. Please choose another!</li></ul>' + '</div>';
							$('#error').append(template);
							$('#ListName').addClass('input-validation-error');
							break;

						default:
							$('#txtListKey').removeAttr('disabled');
							$('#ListKey').val(result);
							$('#txtListKey').val(result);
							$('#txtListKey').attr('disabled', 'disabled');
					}
				},
				error: function (xhr, status, err) {
					NAP.handleAjaxError(xhr, status, err);
				}
			});
		},
		checkValidateMailTo: function () {
			var isValid = true;

			if ($("#to-list").children().find(".search-choice").length === 0 &&
				$("#to-list").children().find(".search-field").children().val().length === 0 &&
				$("#ToList").hasClass("input-validation-error")) {
				isValid = false;
			}

			if (!isValid) {
				$("#ToList").parent().addClass("input-validation-error");
			}
			else {
				$("#ToList").parent().removeClass("input-validation-error");
			}
		}
	};
} (jQuery));

$(document).ready(function () {
	MailingList.init();
});

