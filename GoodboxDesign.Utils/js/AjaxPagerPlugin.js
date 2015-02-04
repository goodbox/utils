
; (function ($) {

	$.ajaxPager = function (element, options) {

		var defaults = {
			pagesForward: 3,
			currentPage: 0,
			totalPages: 0,
			clickHandler: null,
			justNextPrev: false
		};

		var plugin = this;

		plugin.settings = {}

		var $element = $(element),
             element = element;

		plugin.init = function () {
			plugin.settings = $.extend({}, defaults, options);
			// code goes here
			if (plugin.settings.clickHandler) {
				$element.find('a.ajaxPager').bind('click', plugin.settings.clickHandler);
			}
		}

		plugin.render = function (s) {
			if (s != null) {
				plugin.settings = $.extend({}, defaults, s);
			}
			var pageIndex = 0;
			var s = '';
			if (!plugin.settings.justNextPrev) {
				for (pageIndex = plugin.settings.currentPage - 1; pageIndex > plugin.settings.currentPage - plugin.settings.pagesForward && pageIndex >= 1; pageIndex--) {
					s = '<li><a class="ajaxPager" data-page="' + (pageIndex - 1) + '" href="javascript:;">' + pageIndex + '</a></li>' + s;
				}
				if (pageIndex >= 1) {
					s = '<li class="disabled"><a href="#">...</a></li>' + s;
					s = '<li><a class="ajaxPager" data-page="0" href="javascript:;">1</a></li>' + s;
				}
				if (plugin.settings.totalPages > 1) {
					s += '<li class="active"><a href="javascript:;">' + plugin.settings.currentPage + '</a></li>';
				}
				for (pageIndex = plugin.settings.currentPage + 1; pageIndex < plugin.settings.currentPage + plugin.settings.pagesForward && pageIndex <= plugin.settings.totalPages; pageIndex++) {
					s += '<li><a class="ajaxPager" data-page="' + (pageIndex - 1) + '" href="javascript:;">' + pageIndex + '</a></li>';
				}
				if (pageIndex <= plugin.settings.totalPages) {
					s += '<li class="disabled"><a href="javascript:;">...</a></li>';
					s += '<li><a class="ajaxPager" data-page="' + (plugin.settings.totalPages - 1) + '" href="javascript:;">' + plugin.settings.totalPages + '</a></li>';
				}
			}
			if (plugin.settings.currentPage > 1) {
				s = '<li><a class="ajaxPager" data-page="' + (plugin.settings.currentPage - 2) + '" href="javascript:;">&laquo;&nbsp;Prev</a></li>' + s;
			} else {
				s = '<li class="disabled"><a href="javascript:;">&laquo;&nbsp;Prev</a></li>' + s;
			}
			if (plugin.settings.currentPage < plugin.settings.totalPages) {
				s += '<li><a class="ajaxPager" data-page="' + (plugin.settings.currentPage) + '" href="javascript:;">Next&nbsp;&raquo;</a></li>';
			}
			else {
				s += '<li class="disabled"><a href="javascript:;">Next&nbsp;&raquo;</a></li>';
			}

			s = '<div class="pagination"><ul>' + s;
			s += '</ul></div>';

			$element.html(s);

			if (plugin.settings.clickHandler) {
				$element.find('a.ajaxPager').bind('click', plugin.settings.clickHandler);
			}
		}

		plugin.init();

	}

	$.fn.ajaxPager = function (options) {

		return this.each(function () {
			if (undefined == $(this).data('ajaxPager')) {
				var plugin = new $.ajaxPager(this, options);
				$(this).data('ajaxPager', plugin);
			}
		});

	}

})(jQuery);