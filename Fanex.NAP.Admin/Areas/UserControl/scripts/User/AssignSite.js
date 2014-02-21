/* global NAP: false */

"use strict";

// NOTE: in this document
//  + assigner indicates the current-logged-in user
//  + assignee indicates the the user assigned privileges to

var AssignSite = (function ($) {

	return {
		userId: null,
		form: null,
		grid: null,
		hasAssignSiteRight: null,

		AssignSiteToUser: function (userId) {
		var value = "";

		AssignSite.grid.find('.chbSite').each(function () {
			var $this = $(this);

			if ($this.is(':checked')) {
				var siteId = $this.attr('id').substr(8);
				var level = $('#ddlSiteLevel_' + siteId).val();
				value += siteId + "," + level + ";";
			}
		});

            var url = NAP.resolveClientUrl('User/AssignSiteToUser'),
				data = {
					userId: userId,
					value: value
				};

			$.ajax({
				url: url,
				type: 'POST',
				data: data,
				dataType: 'html',
				cache: false,
				beforeSend: function () {
					NAP.loadingBox.show(null, { modal: true });
				},
				complete: function () {
					NAP.loadingBox.close();
				},
				success: function (result) {
					if (result) {
						NAP.headlineMsgBox.showInfo('Your changes were saved!', { closeBtn: false });
					}
				},
				error: function (xhr, status, err) {
					NAP.handleAjaxError(xhr, status, err);
				}
				});
			},

			btnApplyHandler: function () {
				// TODO : call ajax to apply change
			}
		};

} (jQuery));

$(document).ready(function () {
	AssignSite.grid = $('.grid');
	AssignSite.userId = $('#UserId').val();
	AssignSite.hasAssignSiteRight = $('#HasAssignSiteRight').val().toLowerCase();

	if (AssignSite.hasAssignSiteRight === 'true') {
		AssignSite.grid.on('click', '.btn-edit', function () {
			var $this = $(this),
			thisRow = $this.parents('tr'),
			thisSelect = thisRow.find('select'),
			thisLevelLabel = thisSelect.prev(),
			thisChecked = thisRow.find('.chbSite'),
			originalChecked = thisChecked.is(':checked'),
			thisBtnApply = $this.next('.btn-apply'),
			thisBtnCancel = thisBtnApply.next('.btn-cancel');

			thisLevelLabel.hide();
			thisSelect.show();   // enables select level
			$this.hide();        // hides edit button

			// shows cancel button and bind the click event, which will cancel the process
			thisBtnCancel.fadeIn().click(function () {
				if (thisChecked.is(':checked') !== originalChecked) {
					thisChecked.attr('checked', originalChecked);
				}

				thisSelect.hide();
				thisLevelLabel.show();
				thisBtnApply.fadeOut();
				$(this).fadeOut(function () { $this.show(); });
			});

			// shows apply button and bind the click event, which will updates the site url
			thisBtnApply.fadeIn().click(AssignSite.btnApplyHandler);
		});

		AssignSite.grid.on('mousedown', '.chbSite', function () {
			var $this = $(this),
			editField = $this.parent().next(),
			btnEdit = editField.find('.btn-edit'),
			siteName = $this.attr('name'),
			isActive = !$this.is(':checked'),
			actionName = isActive ? 'activate' : 'deactivate',
			message = "Are you sure you want to " + actionName + " '" + siteName + "' ?";

			if (btnEdit.attr('style') !== "display: none;" && btnEdit.attr('style') !== "display: none") {
				NAP.headlineMsgBox.confirm(message, function () {
					$this.attr('checked', isActive);
					// TODO : call ajax to change
				}, { modal: true, closeBtn: false });
			}
		});
	}
});