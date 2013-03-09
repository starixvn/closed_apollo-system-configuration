var PrivilegedGroupsAndUsers = function ($) {

	return {
		groupGrid: null,
		userGrid: null,
		btnToggleGroup: null,
		btnToggleUser: null,
		btnAddGroup: null,
		btnAddUser: null,
		functionId: null,
		functionName: null,
		addUserBox: null
	};

} (jQuery);

$(document).ready(function () {
	PrivilegedGroupsAndUsers.groupGrid = $('#group-grid');
	PrivilegedGroupsAndUsers.userGrid = $('#user-grid');
	PrivilegedGroupsAndUsers.btnToggleGroup = $('#btn-toggle-group');
	PrivilegedGroupsAndUsers.btnToggleUser = $('#btn-toggle-user');
	PrivilegedGroupsAndUsers.btnAddGroup = $('#btn-add-group');
	PrivilegedGroupsAndUsers.btnAddUser = $('#btn-add-user');
	PrivilegedGroupsAndUsers.functionId = $('#fid').val();
	PrivilegedGroupsAndUsers.functionName = $('#fname').val();
	PrivilegedGroupsAndUsers.addUserBox = $('#add-user-box');
	PrivilegedGroupsAndUsers.addGroupBox = $('#add-group-box');

	// #region toggle grids
	var toggleGrid = function () {
		var $this = $(this),
			grid = $this.parents('h2').next('table').children('tbody');

		if (grid.is(':visible')) {
			grid.hide();
			$this.text('Expand list');
		} else {
			grid.slideToggle('fast', 'easeOutBack');
			$this.text('Collapse list');
		}
	};

	PrivilegedGroupsAndUsers.btnToggleGroup.live('click', toggleGrid);
	PrivilegedGroupsAndUsers.btnToggleUser.live('click', toggleGrid);
	// #endregion toggle grids

	PrivilegedGroupsAndUsers.btnAddUser.click(function () {
		NAP.popup("Add privileged users for '" + PrivilegedGroupsAndUsers.functionName + "' function",
			PrivilegedGroupsAndUsers.addUserBox.html());
	});

	PrivilegedGroupsAndUsers.btnAddGroup.click(function () {
		NAP.popup("Add privileged groups for '" + PrivilegedGroupsAndUsers.functionName + "' function",
			PrivilegedGroupsAndUsers.addGroupBox.html());
	});

	NAP.dialogPopup.on('click', '.filter-form input[type=submit]', function () {
		var $this = $(this),
			form = $this.parents('.filter-form'),
			functionId = PrivilegedGroupsAndUsers.functionId,
			groupId = form.find('[name=GroupId]').val(),
			username = form.find('[name=Username]').val(),
			groupName = form.find('[name=Groupname]').val();

		var searchName = $this.attr('name');

		if (searchName == "user") {
			$.ajax({
				url: NAP.resolveClientUrl("Function/PrivilegedUsers?fid=" + functionId + "&gid=" + groupId + "&q=" + username),
				type: 'GET',
				cache: false,
				beforeSend: function () {
					NAP.loadingBox.show();
				},
				complete: function () {
					NAP.loadingBox.close();
				},
				success: function (result) {
					if (result) {
						var userGridWrp = $('#user-grid-wrp');

						// binds user list to popup
						if (userGridWrp.length == 0) {
							userGridWrp = $('<div id="user-grid-wrp"></div>').insertAfter(form);
						}
						userGridWrp.html(result);

						// adds selected users
						var btnAddUser = userGridWrp.find('.form input[type=submit]');
						if (btnAddUser.length > 0) {
							btnAddUser.click(function () {
								var $this = $(this),
								selectedUsers = userGridWrp.find('.grid input[type=checkbox][uid]:checked'),
								readList = [], hostList = [];

								selectedUsers.each(function () {
									var $this = $(this),
									userId = $this.attr('uid'),
									policy = $this.parents('tr').find('td:last input[type=radio]:checked');

									if (policy.length == 0) {
										readList.push(userId);
									} else {
										if (policy.attr('id').indexOf('read') >= 0) {
											readList.push(userId);
										} else {
											hostList.push(userId);
										}
									}
								});

								$.ajax({
									url: NAP.resolveClientUrl("Function/AddPrivilegedUsers/" + functionId + "?readlist=" + readList + "&hostlist=" + hostList),
									type: 'GET',
									cache: false,
									beforeSend: function () {
										NAP.loadingBox.show();
										$this.attr('disabled', true);
										userGridWrp.find('.grid input[type=checkbox]').attr('disabled', true);
									},
									complete: function () {
										NAP.loadingBox.close();
									},
									success: function (result) {
										if (result.toUpperCase() == 'OK') {
											NAP.headlineMsgBox.showInfo('Your changes were saved!', { closeBtn: false });
											setTimeout('window.location.reload()', 2000);
										}
									},
									error: function (xhr, status, err) {
										NAP.handleAjaxError(xhr, status, err);
									}
								});
							});
						}

						// binds checkboxes event
						userGridWrp.on('change', '.grid input[type=checkbox]', function () {
							var $this = $(this),
							userId = $this.attr('uid'),
							thisIsChecked = $this.is(':checked');

							if (userId) {
								if (thisIsChecked) {
									userGridWrp.find('.grid input[type=checkbox]:not([uid]):first').attr('checked', true);
								}
							} else { // check-all checkbox
								userGridWrp.find('.grid input[type=checkbox][uid]').attr('checked', thisIsChecked);
							}

							if (thisIsChecked) {
								btnAddUser.attr('disabled', false);
							} else {
								if (userGridWrp.find('.grid input[type=checkbox][uid]:checked').length == 0) {
									btnAddUser.attr('disabled', true);
									userGridWrp.find('.grid input[type=checkbox]:not([uid]):first').attr('checked', false);
								}
							}
						});
					}
				},
				error: function (xhr, status, err) {
					NAP.handleAjaxError(xhr, status, err);
				}
			});
		}
		if (searchName == "group") {
			$.ajax({
				url: NAP.resolveClientUrl("Function/PrivilegedGroups?fid=" + functionId + "&q=" + groupName),
				type: 'GET',
				cache: false,
				beforeSend: function () {
					NAP.loadingBox.show();
				},
				complete: function () {
					NAP.loadingBox.close();
				},
				success: function (result) {
					if (result) {
						var groupGridWrp = $('#group-grid-wrp');

						// binds user list to popup
						if (groupGridWrp.length == 0) {
							groupGridWrp = $('<div id="group-grid-wrp"></div>').insertAfter(form);
						}
						groupGridWrp.html(result);

						// adds selected users
						var btnAddGroup = groupGridWrp.find('.form input[type=submit]');
						if (btnAddGroup.length > 0) {
							btnAddGroup.click(function () {
								var $this = $(this),
								selectedGroups = groupGridWrp.find('.grid input[type=checkbox][gid]:checked'),
								readList = [], hostList = [];

								selectedGroups.each(function () {
									var $this = $(this),
									groupId = $this.attr('gid'),
									policy = $this.parents('tr').find('td:last input[type=radio]:checked');

									if (policy.length == 0) {
										readList.push(groupId);
									} else {
										if (policy.attr('id').indexOf('read') >= 0) {
											readList.push(groupId);
										} else {
											hostList.push(groupId);
										}
									}
								});

								$.ajax({
									url: NAP.resolveClientUrl("Function/AddPrivilegedGroups/" + functionId + "?readlist=" + readList + "&hostlist=" + hostList),
									type: 'GET',
									cache: false,
									beforeSend: function () {
										NAP.loadingBox.show();
										$this.attr('disabled', true);
										groupGridWrp.find('.grid input[type=checkbox]').attr('disabled', true);
									},
									complete: function () {
										NAP.loadingBox.close();
									},
									success: function (result) {
										if (result.toUpperCase() == 'OK') {
											NAP.headlineMsgBox.showInfo('Your changes were saved!', { closeBtn: false });
											setTimeout('window.location.reload()', 2000);
										}
									},
									error: function (xhr, status, err) {
										NAP.handleAjaxError(xhr, status, err);
									}
								});
							});
						}

						// binds checkboxes event
						groupGridWrp.on('change', '.grid input[type=checkbox]', function () {
							var $this = $(this),
							groupId = $this.attr('gid'),
							thisIsChecked = $this.is(':checked');

							if (groupId) {
								if (thisIsChecked) {
									groupGridWrp.find('.grid input[type=checkbox]:not([gid]):first').attr('checked', true);
								}
							} else { // check-all checkbox
								groupGridWrp.find('.grid input[type=checkbox][gid]').attr('checked', thisIsChecked);
							}

							if (thisIsChecked) {
								btnAddGroup.attr('disabled', false);
							} else {
								if (groupGridWrp.find('.grid input[type=checkbox][gid]:checked').length == 0) {
									btnAddGroup.attr('disabled', true);
									groupGridWrp.find('.grid input[type=checkbox]:not([gid]):first').attr('checked', false);
								}
							}
						});
					}
				},
				error: function (xhr, status, err) {
					NAP.handleAjaxError(xhr, status, err);
				}
			});
		}
	});

	NAP.dialogPopup.on('keypress', '.filter-form input[type=text]', function (e) {
		if (e.keyCode == 13) {
			$(this).parents('.filter-form').find('input[type=submit]').click();
		}
	});
});