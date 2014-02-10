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
		}
	};

} (jQuery));

$(document).ready(function () {
	AssignSite.form = $('form:first');
	AssignSite.grid = $('.grid');
	AssignSite.userId = $('#UserId').val();
	AssignSite.hasAssignSiteRight = $('#HasAssignSiteRight').val().toLowerCase();

	if (AssignSite.hasAssignSiteRight === 'true') {
		// disables dropdownlist of inactive sites and/or
		// enables dropdownlist of active sites
		var toggleSite = function () {
			var $this = $(this), // this --> checkbox
				thisId = $this.attr('id').substr(8);

			if ($this.is(':checked') && !$this.is(':disabled')) {
				$('#ddlSiteLevel_' + thisId).attr('disabled', false);
			} else {
				$('#ddlSiteLevel_' + thisId).attr('disabled', true);
			}
		};

		AssignSite.grid.find('.chbSite').each(function () {
			toggleSite.call(this);
		}).click(function () {
			toggleSite.call(this);
		});

		AssignSite.form.submit(function () {
			AssignSite.AssignSiteToUser(AssignSite.userId);
			return false;
		});
	}
});