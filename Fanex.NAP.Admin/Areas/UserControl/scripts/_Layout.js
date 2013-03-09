var Layout = function ($) {

	return {
		filterBar: null,
		pageBody: null,
		page: null,
		frmPage: null
	};

} (jQuery);

$(document).ready(function () {
	Layout.page = $('#page');
	Layout.frmPage = $('#frm-page');

	Layout.pageBody = $('#body');
	Layout.filterBar = $('#filter-bar');

	// #region Menu
	var menuEvent = 'click';

	$('nav').on(menuEvent, '.menu li', function (e) {
		var $this = $(this),
			_SELECTED = 'selected';

		if ($this.hasClass(_SELECTED)) {
			$this.removeClass(_SELECTED).find('li').removeClass(_SELECTED);
		} else {
			$this.addClass(_SELECTED).siblings('li').removeClass(_SELECTED);
		}

		if (!$this.hasClass('has-kids')) {
			$.cookie('menu-id', $this.attr('mid'), { path: '/' });
		}

		e.stopPropagation();
	}).find('li').each(function () {
		var $this = $(this),
			menuId = $.cookie('menu-id');

		if ($this.attr('mid') == menuId) {
			$this.addClass('current').parents('li').addClass('current');
		}
	});

	//	$('nav').on(menuEvent, '.menu li', function (e) {
	//		var $this = $(this),
	//			siteUrlId = $this.attr('suid'),
	//			siteId = $this.attr('sid'),
	//			napId = $this.parents('nav').attr('nid');

	//		// open site in new tab
	//		if (siteUrlId != undefined && siteId != undefined && siteId != napId) {
	//			var url = NAP.resolveRootUrl('UserSite/Forward/' + siteUrlId);
	//			window.open(url, '_blank');
	//			return false;
	//		}

	//		// load site to iframe
	//		//		if (siteUrlId != undefined && siteId != undefined && siteId != napId && !$this.hasClass('current')) {
	//		//			var url = NAP.resolveRootUrl('UserSite/Forward/' + siteUrlId);

	//		//			Layout.page.html('');
	//		//			Layout.frmPage.show().children('iframe').attr('src', url);
	//		//			$this.addClass('current').siblings('li').removeClass('current');
	//		//		}

	//		if ($this.hasClass('has-kids')) {
	//			if ($this.hasClass('selected')) {
	//				$this.removeClass('selected').find('li').removeClass('selected');
	//			} else {
	//				$this.addClass('selected').siblings('li').removeClass('selected');
	//			}

	//			e.stopPropagation();
	//		} else {
	//			var siteNode = $this.parents('[sid]');

	//			if (siteNode.attr('sid') != napId) { // not nap admin site
	//				var siteUrl = $this.parents('[su]').attr('su'),
	//					url = $this.children('a').attr('href');

	//				Layout.frmPage.show().children('iframe').attr('src', siteUrl + url);
	//				$('nav li').removeClass('selected');
	//				return false;
	//			}
	//		}
	//	});

	$('header').on(menuEvent, '.greeting', function (e) {
		$(this).toggleClass('selected');
		e.stopPropagation();
	});

	$('body').on('click', function () {
		$('nav li, header .greeting').removeClass('selected');
	});
	// #endregion Menu

	$('.pnl-header-toggle').live('click', function () {
		var $this = $(this),
			pnlWrapper = $this.parents('.pnl-wrp'),
			pnlHeader = pnlWrapper.find('.pnl-header'),
			pnlBody = pnlWrapper.find('.pnl-body');

		$this.toggleClass('off');
		pnlHeader.toggleClass('off');
		pnlBody.slideToggle('fast');
	});
});