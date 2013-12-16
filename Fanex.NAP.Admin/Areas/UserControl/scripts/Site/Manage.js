
var ManageSite = function ($) {

    return {
        grid: null
    };

} (jQuery);

$(document).ready(function () {
    ManageSite.grid = $('.grid');

    // #region tooltips
    $('.tip').tipsy({ gravity: 'se' });
    $('#headline h1 .lnk').tipsy({ gravity: 'w' });
    $('.grid input[type=checkbox][sid], .grid .num-of-user, .grid .url-toggle').tipsy({ gravity: 's' });
    // #endregion

    // #region activate/deactivate functions
    ManageSite.grid.on('mousedown', 'input[type=checkbox][sid]', function (e) {
        var $this = $(this),
			siteName = $this.attr('sname'),
			isActive = !$this.is(':checked'),
			action = isActive ? 'activate' : 'deactivate',
			msg = "Are you sure you want to " + action + " '" + siteName + "' ?";

        NAP.headlineMsgBox.confirm(msg, function () {
            var siteId = $this.attr('sid');

            $this.attr('checked', isActive).change();

            $.ajax({
            	url: NAP.resolveClientUrl("Site/Activate" + "?id=" + siteId + "&isActive=" + isActive),
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
                    	var msg = isActive ? "Activation succeeded!" : "Deactivation succeeded!";

                        NAP.headlineMsgBox.showInfo(msg, { closeBtn: false });
                        if (isActive) {
                            $this.attr('original-title', $this.attr('original-title').replace('Activate', 'Deactivate'));
                        } else {

                            $this.attr('original-title', $this.attr('original-title').replace('Deactivate', 'Activate'));
                        }
                    }
                },
                error: function (xhr, status, err) {
                    NAP.handleAjaxError(xhr, status, err);
                }
            });
        }, { modal: true, closeBtn: false });
    });
    // #endregion

    ManageSite.grid.on('click', '.url-toggle', function () {
        var $this = $(this),
			altUrlList = $this.siblings('.alt-urls');

        if ($this.hasClass('expand')) {
            altUrlList.slideUp('fast');
        } else {
            altUrlList.slideDown('fast', 'easeOutBack');
        }

        $this.toggleClass('expand');
    });
});