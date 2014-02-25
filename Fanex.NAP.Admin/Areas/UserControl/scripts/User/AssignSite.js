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

		UpdateSiteToUser: function (userId, siteId, level, isActive) {
			var url = NAP.resolveClientUrl('User/UpdateSiteToUser'),
				data = {
					userId: userId,
					siteId: siteId,
					level: level,
					isActive: isActive
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
				originalDisabledChecked = thisChecked.is(':disabled'),
				thisBtnApply = $this.next('.btn-apply'),
				thisBtnCancel = thisBtnApply.next('.btn-cancel');

			thisLevelLabel.hide();
			thisSelect.show();   // enables select level
			$this.hide();        // hides edit button
			thisChecked.attr('disabled', false);

			// shows cancel button and bind the click event
			thisBtnCancel.fadeIn().click(function () {
				if (thisChecked.is(':checked') !== originalChecked) {
					thisChecked.attr('checked', originalChecked);
				}

				showHide($(this), thisBtnApply);
				thisChecked.attr('disabled', originalDisabledChecked);
				var level = thisChecked.val();
				thisSelect.find('option[value=' + level + ']').attr('selected', true);
			});

			// shows apply button and bind the click event
			thisBtnApply.fadeIn().click(function () {
				var siteId = thisChecked.attr('id').substr(8),
					level = thisSelect.val(),
					isActive = thisChecked.is(':checked'),
					levelName = thisSelect.find(':selected').text();

				thisLevelLabel.text(levelName);
				thisChecked.val(level);
				AssignSite.UpdateSiteToUser(AssignSite.userId, siteId, level, isActive);
				showHide($(this), thisBtnCancel);
			});

			var showHide = function (currentBtn, otherBtn) {
				thisSelect.hide();
				thisLevelLabel.show();
				otherBtn.fadeOut();
				currentBtn.fadeOut(function () { $this.show(); });
			};
		});

		AssignSite.grid.on('mousedown', '.chbSite', function () {
			var $this = $(this),
				editField = $this.parent().next(),
				btnEdit = editField.find('.btn-edit'),
				siteName = $this.attr('name'),
				siteId = $this.attr('id').substr(8),
				level = $this.val(),
				isActive = !$this.is(':checked'),
				actionName = isActive ? 'activate' : 'deactivate',
				message = "Are you sure you want to " + actionName + " '" + siteName + "' ?";

			if (btnEdit.attr('style') !== "display: none;" && btnEdit.attr('style') !== "display: none") {
				NAP.headlineMsgBox.confirm(message, function () {
					$this.attr('checked', isActive);
					AssignSite.UpdateSiteToUser(AssignSite.userId, siteId, level, isActive);
				}, { modal: true, closeBtn: false });
			}
		});
	}
});