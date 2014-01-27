var ManageMailingList = function ($) {
	return {
		Form: null,
		Grid: null,

		init: function () {
			ManageMailingList.Grid = $('.grid');
			ManageMailingList.Form = $('form:first');

			ManageMailingList.Grid.on('mousedown', '.chk-active-item', function (e) {
				var $this = $(this),
				listName = $this.attr('dataname'),
				active = !$this.is(':checked'),
				action = active ? 'activate' : 'deactivate',
				msg = "Are you sure you want to " + action + " '" + listName + "' ?";

				NAP.headlineMsgBox.confirm(msg, function () {
					var mailingListId = $this.attr('dataid');

					$this.attr('checked', active).change();

					$.ajax({
						url: NAP.resolveClientUrl("MailingList/Activate" + "?id=" + mailingListId + "&active=" + active),
						type: 'POST',
						contentType: 'application/json',
						dataType: 'json',
						cache: false,
						beforeSend: function () {
							NAP.loadingBox.show();
						},
						complete: function () {
							NAP.loadingBox.close();
						},
						success: function (result) {
							if (result == 'OK') {
								var msg = active ? "Activation succeeded!" : "Deactivation succeeded!";

								NAP.headlineMsgBox.showInfo(msg, { closeBtn: false });
								if (active) {
									$this.attr('title', $this.attr('title').replace('Activate', 'Deactivate'));
								} else {
									$this.attr('title', $this.attr('title').replace('Deactivate', 'Activate'));
								}
							}
						},
						error: function (xhr, status, err) {
							NAP.handleAjaxError(xhr, status, err);
						}
					});
				}, { modal: true, closeBtn: false });
			});

			ManageMailingList.Grid.on('mousedown', '.chk-internal-item', function (e) {
				var $this = $(this),
				listName = $this.attr('dataname'),
				isInternal = !$this.is(':checked'),
				action = isInternal ? 'set' : 'unset',
				msg = "Are you sure to " + action + " internal for '" + listName + "' ?";

				NAP.headlineMsgBox.confirm(msg, function () {
					var mailingListId = $this.attr('dataid');

					$this.attr('checked', isInternal).change();

					$.ajax({
						url: NAP.resolveClientUrl("MailingList/Internal" + "?id=" + mailingListId + "&isInternal=" + isInternal),
						type: 'POST',
						contentType: 'application/json',
						dataType: 'json',
						cache: false,
						beforeSend: function () {
							NAP.loadingBox.show();
						},
						complete: function () {
							NAP.loadingBox.close();
						},
						success: function (result) {
							if (result == 'OK') {
								var msg = isInternal ? "Set internal succeeded!" : "Unset internal succeeded!";

								NAP.headlineMsgBox.showInfo(msg, { closeBtn: false });
								if (isInternal) {
									$this.attr('title', $this.attr('title').replace('Set', 'Unset'));
								} else {
									$this.attr('title', $this.attr('title').replace('Unset', 'Set'));
								}
							}
						},
						error: function (xhr, status, err) {
							NAP.handleAjaxError(xhr, status, err);
						}
					});
				}, { modal: true, closeBtn: false });
			});
		}
	};
} (jQuery);

$(document).ready(function () {
    ManageMailingList.init();
});

