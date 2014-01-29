var AssignUserPermission = function ($) {

	return {
		updateForm: null,
		grid: null,

		updateParent: function (type) {
			var $this = $(this),
				pid = $this.attr('pid');

			if (pid && pid.length > 0) {
				var thisIsChecked = $this.is(':checked'),
					parent = $('#func' + type + pid);

				if (thisIsChecked && !parent.is(':checked')) {
					parent.attr('checked', true);
				}

				if (!thisIsChecked) {
					var siblings = $('[name=func' + type + '][pid=' + parent.val() + ']:checked');

					if (siblings.length === 0) {
						parent.attr('checked', false);

						if (type === 'Read') {
							parent.parents('tr').find('[name=funcHost]').attr('checked', false);
						}
					}
				}

				AssignUserPermission.updateParent.call(parent.get(0), type);
			}
		},

		// disabled parent if has child is disabled
		disabledParent: function () {
			AssignUserPermission.grid.find('tr').each(function () {
				var $this = $(this),
				chkHost = $this.find('[name=funcHost][type=hidden]'),
				chkRead = $this.find('[name=funcRead][type=hidden]');

				if (chkRead !== undefined) {
					var parent = $('#funcRead' + chkRead.attr('pid'));
					if (parent.is(':checked') && parent.attr('pdisabled') === undefined) {
						parent.attr('pdisabled', '1');
						parent.after('<input type="checkbox" checked="checked" disabled="disabled"/>');
						parent.hide();
					}
				}

				if (chkHost !== undefined) {
					var parent = $('#funcHost' + chkHost.attr('pid'));
					if (parent.is(':checked') && parent.attr('pdisabled') === undefined) {
						parent.attr('pdisabled', '1');
						parent.after('<input type="checkbox" checked="checked" disabled="disabled"/>');
						parent.hide();
					}
				}
			});
		}
	};

} (jQuery);

$(document).ready(function () {
	AssignUserPermission.grid = $('.grid:first');
	AssignUserPermission.updateForm = $('form:last');
	AssignUserPermission.disabledParent();

	AssignUserPermission.updateForm.submit(function () {
		AssignUserPermission.grid.find('tr').each(function () {
			var $this = $(this),
				chkHost = $this.find('[name=funcHost]'),
				chkRead = $this.find('[name=funcRead]');

			if (chkHost.is(':checked') || chkHost.attr('checked') === "checked") {
				chkRead.removeAttr('name');
			} else {
				chkHost.removeAttr('name');
			}
		});
	});

	AssignUserPermission.grid.find('[type=checkbox]').change(function () {
		var $this = $(this),
			thisIsChecked = $this.is(':checked'),
			row = $this.parents('tr'),
			table = row.parents('table');

		if ($this.attr('name') === 'funcHost') {
			// host checked -> read checked
			if (thisIsChecked) {
				AssignUserPermission.updateParent.call(row.find('[type=checkbox][name=funcRead]').attr('checked', true).get(0), 'Read');
				table.find('[type=checkbox][name=funcRead][pid=' + $this.val() + ']:not(:disabled)').attr('checked', true).change();
			}

			// parent changed -> children changed
			table.find('[type=checkbox][name=funcHost][pid=' + $this.val() + ']:not(:disabled)').attr('checked', thisIsChecked).change();

			// child checked -> parent checked
			// last child unchecked -> parent unchecked
			AssignUserPermission.updateParent.call($this.get(0), 'Host');
		} else { // funcRead
			// read unchecked -> host unchecked
			if (!thisIsChecked) {
				AssignUserPermission.updateParent.call(row.find('[type=checkbox][name=funcHost]:not(:disabled)').attr('checked', false).get(0), 'Host');
			}

			// parent changed -> children changed
			table.find('[type=checkbox][name=funcRead][pid=' + $this.val() + ']:not(:disabled)').attr('checked', thisIsChecked).change();

			// child checked -> parent checked
			// last child unchecked -> parent unchecked
			AssignUserPermission.updateParent.call($this.get(0), 'Read');
		}
	});

	var hasAssignPermission = $('#HasAssignPermission').val().toLowerCase();
	if (hasAssignPermission === "false") {
		$('input[type=checkbox]').attr('disabled', true);
		$('#btnAssignPermission').hide();
	}
});