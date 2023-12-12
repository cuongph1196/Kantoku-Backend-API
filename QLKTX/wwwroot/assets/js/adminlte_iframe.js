
/**
 * --------------------------------------------
 * AdminLTE IFrame.js
 * License MIT
 * --------------------------------------------
 */
/**
 * Constants
 * ====================================================
 */

var NAME$7 = 'IFrame';
var DATA_KEY$7 = 'lte.iframe';
var JQUERY_NO_CONFLICT$7 = $__default["default"].fn[NAME$7];
var SELECTOR_DATA_TOGGLE$1 = '[data-widget="iframe"]';
var SELECTOR_DATA_TOGGLE_CLOSE = '[data-widget="iframe-close"]';
var SELECTOR_DATA_TOGGLE_SCROLL_LEFT = '[data-widget="iframe-scrollleft"]';
var SELECTOR_DATA_TOGGLE_SCROLL_RIGHT = '[data-widget="iframe-scrollright"]';
var SELECTOR_DATA_TOGGLE_FULLSCREEN = '[data-widget="iframe-fullscreen"]';
var SELECTOR_CONTENT_WRAPPER = '.content-wrapper';
var SELECTOR_CONTENT_IFRAME = SELECTOR_CONTENT_WRAPPER + " iframe";
var SELECTOR_TAB_NAV = SELECTOR_CONTENT_WRAPPER + ".iframe-mode .nav";
var SELECTOR_TAB_NAVBAR_NAV = SELECTOR_CONTENT_WRAPPER + ".iframe-mode .navbar-nav";
var SELECTOR_TAB_NAVBAR_NAV_ITEM = SELECTOR_TAB_NAVBAR_NAV + " .nav-item";
var SELECTOR_TAB_NAVBAR_NAV_LINK = SELECTOR_TAB_NAVBAR_NAV + " .nav-link";
var SELECTOR_TAB_CONTENT = SELECTOR_CONTENT_WRAPPER + ".iframe-mode .tab-content";
var SELECTOR_TAB_EMPTY = SELECTOR_TAB_CONTENT + " .tab-empty";
var SELECTOR_TAB_LOADING = SELECTOR_TAB_CONTENT + " .tab-loading";
var SELECTOR_TAB_PANE = SELECTOR_TAB_CONTENT + " .tab-pane";
var SELECTOR_SIDEBAR_MENU_ITEM = '.main-sidebar .nav-item > a.nav-link';
var SELECTOR_SIDEBAR_SEARCH_ITEM = '.sidebar-search-results .list-group-item';
var SELECTOR_HEADER_MENU_ITEM = '.main-header .nav-item a.nav-link';
var SELECTOR_HEADER_DROPDOWN_ITEM = '.main-header a.dropdown-item';
var CLASS_NAME_IFRAME_MODE$1 = 'iframe-mode';
var CLASS_NAME_FULLSCREEN_MODE = 'iframe-mode-fullscreen';
var Default$7 = {
    onTabClick: function onTabClick(item) {
        return item;
    },
    onTabChanged: function onTabChanged(item) {
        return item;
    },
    onTabCreated: function onTabCreated(item) {
        return item;
    },
    autoIframeMode: true,
    autoItemActive: true,
    autoShowNewTab: true,
    autoDarkMode: false,
    allowDuplicates: false,
    allowReload: true,
    loadingScreen: true,
    useNavbarItems: true,
    scrollOffset: 40,
    scrollBehaviorSwap: false,
    iconMaximize: 'fa-expand',
    iconMinimize: 'fa-compress'
};
/**
 * Class Definition
 * ====================================================
 */

var IFrame = /*#__PURE__*/function () {
    function IFrame(element, config) {
        this._config = config;
        this._element = element;

        this._init();
    } // Public


    var _proto = IFrame.prototype;

    _proto.onTabClick = function onTabClick(item) {
        this._config.onTabClick(item);
    };

    _proto.onTabChanged = function onTabChanged(item) {
        this._config.onTabChanged(item);
    };

    _proto.onTabCreated = function onTabCreated(item) {
        this._config.onTabCreated(item);
    };

    _proto.createTab = function createTab(title, link, uniqueName, autoOpen) {
        var _this = this;

        var tabId = "panel-" + uniqueName;
        var navId = "tab-" + uniqueName;

        if (this._config.allowDuplicates) {
            tabId += "-" + Math.floor(Math.random() * 1000);
            navId += "-" + Math.floor(Math.random() * 1000);
        }

        var newNavItem = "<li class=\"nav-item\" role=\"presentation\"><a href=\"#\" class=\"btn-iframe-close\" data-widget=\"iframe-close\" data-type=\"only-this\"><i class=\"fas fa-times\"></i></a><a class=\"nav-link\" data-toggle=\"row\" id=\"" + navId + "\" href=\"#" + tabId + "\" role=\"tab\" aria-controls=\"" + tabId + "\" aria-selected=\"false\">" + title + "</a></li>";
        $__default["default"](SELECTOR_TAB_NAVBAR_NAV).append(unescape(escape(newNavItem)));
        var newTabItem = "<div class=\"tab-pane fade\" id=\"" + tabId + "\" role=\"tabpanel\" aria-labelledby=\"" + navId + "\"><iframe src=\"" + link + "\"></iframe></div>";
        $__default["default"](SELECTOR_TAB_CONTENT).append(unescape(escape(newTabItem)));

        if (autoOpen) {
            if (this._config.loadingScreen) {
                var $loadingScreen = $__default["default"](SELECTOR_TAB_LOADING);
                $loadingScreen.fadeIn();
                $__default["default"](tabId + " iframe").ready(function () {
                    if (typeof _this._config.loadingScreen === 'number') {
                        _this.switchTab("#" + navId);

                        setTimeout(function () {
                            $loadingScreen.fadeOut();
                        }, _this._config.loadingScreen);
                    } else {
                        _this.switchTab("#" + navId);

                        $loadingScreen.fadeOut();
                    }
                });
            } else {
                this.switchTab("#" + navId);
            }
        }

        this.onTabCreated($__default["default"]("#" + navId));
    };

    _proto.openTabSidebar = function openTabSidebar(item, autoOpen) {
        if (autoOpen === void 0) {
            autoOpen = this._config.autoShowNewTab;
        }

        var $item = $__default["default"](item).clone();

        if ($item.attr('href') === undefined) {
            $item = $__default["default"](item).parent('a').clone();
        }

        $item.find('.right, .search-path').remove();
        var title = $item.find('p').text();

        if (title === '') {
            title = $item.text();
        }

        var link = $item.attr('href');

        if (link === '#' || link === '' || link === undefined) {
            return;
        }

        var uniqueName = unescape(link).replace('./', '').replace(/["#&'./:=?[\]]/gi, '-').replace(/(--)/gi, '');
        var navId = "tab-" + uniqueName;

        if (!this._config.allowDuplicates && $__default["default"]("#" + navId).length > 0) {
            return this.switchTab("#" + navId, this._config.allowReload);
        }

        if (!this._config.allowDuplicates && $__default["default"]("#" + navId).length === 0 || this._config.allowDuplicates) {
            this.createTab(title, link, uniqueName, autoOpen);
        }
    };

    _proto.switchTab = function switchTab(item, reload) {
        var _this2 = this;

        if (reload === void 0) {
            reload = false;
        }

        var $item = $__default["default"](item);
        var tabId = $item.attr('href');
        $__default["default"](SELECTOR_TAB_EMPTY).hide();

        if (reload) {
            var $loadingScreen = $__default["default"](SELECTOR_TAB_LOADING);

            if (this._config.loadingScreen) {
                $loadingScreen.show(0, function () {
                    $__default["default"](tabId + " iframe").attr('src', $__default["default"](tabId + " iframe").attr('src')).ready(function () {
                        if (_this2._config.loadingScreen) {
                            if (typeof _this2._config.loadingScreen === 'number') {
                                setTimeout(function () {
                                    $loadingScreen.fadeOut();
                                }, _this2._config.loadingScreen);
                            } else {
                                $loadingScreen.fadeOut();
                            }
                        }
                    });
                });
            } else {
                $__default["default"](tabId + " iframe").attr('src', $__default["default"](tabId + " iframe").attr('src'));
            }
        }

        $__default["default"](SELECTOR_TAB_NAVBAR_NAV + " .active").tab('dispose').removeClass('active');

        this._fixHeight();

        $item.tab('show');
        $item.parents('li').addClass('active');
        this.onTabChanged($item);

        if (this._config.autoItemActive) {
            this._setItemActive($__default["default"](tabId + " iframe").attr('src'));
        }
    };

    _proto.removeActiveTab = function removeActiveTab(type, element) {
        if (type == 'all') {
            $__default["default"](SELECTOR_TAB_NAVBAR_NAV_ITEM).remove();
            $__default["default"](SELECTOR_TAB_PANE).remove();
            $__default["default"](SELECTOR_TAB_EMPTY).show();
        } else if (type == 'all-other') {
            $__default["default"](SELECTOR_TAB_NAVBAR_NAV_ITEM + ":not(.active)").remove();
            $__default["default"](SELECTOR_TAB_PANE + ":not(.active)").remove();
        } else if (type == 'only-this') {
            var $navClose = $__default["default"](element);
            var $navItem = $navClose.parent('.nav-item');
            var $navItemParent = $navItem.parent();
            var navItemIndex = $navItem.index();
            var tabId = $navClose.siblings('.nav-link').attr('aria-controls');
            $navItem.remove();
            $__default["default"]("#" + tabId).remove();

            if ($__default["default"](SELECTOR_TAB_CONTENT).children().length == $__default["default"](SELECTOR_TAB_EMPTY + ", " + SELECTOR_TAB_LOADING).length) {
                $__default["default"](SELECTOR_TAB_EMPTY).show();
            } else {
                var prevNavItemIndex = navItemIndex - 1;
                this.switchTab($navItemParent.children().eq(prevNavItemIndex).find('a.nav-link'));
            }
        } else {
            var _$navItem = $__default["default"](SELECTOR_TAB_NAVBAR_NAV_ITEM + ".active");

            var _$navItemParent = _$navItem.parent();

            var _navItemIndex = _$navItem.index();

            _$navItem.remove();

            $__default["default"](SELECTOR_TAB_PANE + ".active").remove();

            if ($__default["default"](SELECTOR_TAB_CONTENT).children().length == $__default["default"](SELECTOR_TAB_EMPTY + ", " + SELECTOR_TAB_LOADING).length) {
                $__default["default"](SELECTOR_TAB_EMPTY).show();
            } else {
                var _prevNavItemIndex = _navItemIndex - 1;

                this.switchTab(_$navItemParent.children().eq(_prevNavItemIndex).find('a.nav-link'));
            }
        }
    };

    _proto.toggleFullscreen = function toggleFullscreen() {
        if ($__default["default"]('body').hasClass(CLASS_NAME_FULLSCREEN_MODE)) {
            $__default["default"](SELECTOR_DATA_TOGGLE_FULLSCREEN + " i").removeClass(this._config.iconMinimize).addClass(this._config.iconMaximize);
            $__default["default"]('body').removeClass(CLASS_NAME_FULLSCREEN_MODE);
            $__default["default"](SELECTOR_TAB_EMPTY + ", " + SELECTOR_TAB_LOADING).height('100%');
            $__default["default"](SELECTOR_CONTENT_WRAPPER).height('100%');
            $__default["default"](SELECTOR_CONTENT_IFRAME).height('100%');
        } else {
            $__default["default"](SELECTOR_DATA_TOGGLE_FULLSCREEN + " i").removeClass(this._config.iconMaximize).addClass(this._config.iconMinimize);
            $__default["default"]('body').addClass(CLASS_NAME_FULLSCREEN_MODE);
        }

        $__default["default"](window).trigger('resize');

        this._fixHeight(true);
    } // Private
        ;

    _proto._init = function _init() {
        var usingDefTab = $__default["default"](SELECTOR_TAB_CONTENT).children().length > 2;

        this._setupListeners();

        this._fixHeight(true);

        if (usingDefTab) {
            var $el = $__default["default"]("" + SELECTOR_TAB_PANE).first(); // eslint-disable-next-line no-console

            console.log($el);
            var uniqueName = $el.attr('id').replace('panel-', '');
            var navId = "#tab-" + uniqueName;
            this.switchTab(navId, true);
        }
    };

    _proto._initFrameElement = function _initFrameElement() {
        if (window.frameElement && this._config.autoIframeMode) {
            var $body = $__default["default"]('body');
            $body.addClass(CLASS_NAME_IFRAME_MODE$1);

            if (this._config.autoDarkMode) {
                $body.addClass('dark-mode');
            }
        }
    };

    _proto._navScroll = function _navScroll(offset) {
        var leftPos = $__default["default"](SELECTOR_TAB_NAVBAR_NAV).scrollLeft();
        $__default["default"](SELECTOR_TAB_NAVBAR_NAV).animate({
            scrollLeft: leftPos + offset
        }, 250, 'linear');
    };

    _proto._setupListeners = function _setupListeners() {
        var _this3 = this;

        $__default["default"](window).on('resize', function () {
            setTimeout(function () {
                _this3._fixHeight();
            }, 1);
        });

        if ($__default["default"](SELECTOR_CONTENT_WRAPPER).hasClass(CLASS_NAME_IFRAME_MODE$1)) {
            $__default["default"](document).on('click', SELECTOR_SIDEBAR_MENU_ITEM + ", " + SELECTOR_SIDEBAR_SEARCH_ITEM, function (e) {
                e.preventDefault();

                _this3.openTabSidebar(e.target);
            });

            if (this._config.useNavbarItems) {
                $__default["default"](document).on('click', SELECTOR_HEADER_MENU_ITEM + ", " + SELECTOR_HEADER_DROPDOWN_ITEM, function (e) {
                    e.preventDefault();

                    _this3.openTabSidebar(e.target);
                });
            }
        }

        $__default["default"](document).on('click', SELECTOR_TAB_NAVBAR_NAV_LINK, function (e) {
            e.preventDefault();

            _this3.onTabClick(e.target);

            _this3.switchTab(e.target);
        });
        $__default["default"](document).on('click', SELECTOR_TAB_NAVBAR_NAV_LINK, function (e) {
            e.preventDefault();

            _this3.onTabClick(e.target);

            _this3.switchTab(e.target);
        });
        $__default["default"](document).on('click', SELECTOR_DATA_TOGGLE_CLOSE, function (e) {
            e.preventDefault();
            var target = e.target;

            if (target.nodeName == 'I') {
                target = e.target.offsetParent;
            }

            _this3.removeActiveTab(target.attributes['data-type'] ? target.attributes['data-type'].nodeValue : null, target);
        });
        $__default["default"](document).on('click', SELECTOR_DATA_TOGGLE_FULLSCREEN, function (e) {
            e.preventDefault();

            _this3.toggleFullscreen();
        });
        var mousedown = false;
        var mousedownInterval = null;
        $__default["default"](document).on('mousedown', SELECTOR_DATA_TOGGLE_SCROLL_LEFT, function (e) {
            e.preventDefault();
            clearInterval(mousedownInterval);
            var scrollOffset = _this3._config.scrollOffset;

            if (!_this3._config.scrollBehaviorSwap) {
                scrollOffset = -scrollOffset;
            }

            mousedown = true;

            _this3._navScroll(scrollOffset);

            mousedownInterval = setInterval(function () {
                _this3._navScroll(scrollOffset);
            }, 250);
        });
        $__default["default"](document).on('mousedown', SELECTOR_DATA_TOGGLE_SCROLL_RIGHT, function (e) {
            e.preventDefault();
            clearInterval(mousedownInterval);
            var scrollOffset = _this3._config.scrollOffset;

            if (_this3._config.scrollBehaviorSwap) {
                scrollOffset = -scrollOffset;
            }

            mousedown = true;

            _this3._navScroll(scrollOffset);

            mousedownInterval = setInterval(function () {
                _this3._navScroll(scrollOffset);
            }, 250);
        });
        $__default["default"](document).on('mouseup', function () {
            if (mousedown) {
                mousedown = false;
                clearInterval(mousedownInterval);
                mousedownInterval = null;
            }
        });
    };

    _proto._setItemActive = function _setItemActive(href) {
        $__default["default"](SELECTOR_SIDEBAR_MENU_ITEM + ", " + SELECTOR_HEADER_DROPDOWN_ITEM).removeClass('active');
        $__default["default"](SELECTOR_HEADER_MENU_ITEM).parent().removeClass('active');
        var $headerMenuItem = $__default["default"](SELECTOR_HEADER_MENU_ITEM + "[href$=\"" + href + "\"]");
        var $headerDropdownItem = $__default["default"](SELECTOR_HEADER_DROPDOWN_ITEM + "[href$=\"" + href + "\"]");
        var $sidebarMenuItem = $__default["default"](SELECTOR_SIDEBAR_MENU_ITEM + "[href$=\"" + href + "\"]");
        $headerMenuItem.each(function (i, e) {
            $__default["default"](e).parent().addClass('active');
        });
        $headerDropdownItem.each(function (i, e) {
            $__default["default"](e).addClass('active');
        });
        $sidebarMenuItem.each(function (i, e) {
            $__default["default"](e).addClass('active');
            $__default["default"](e).parents('.nav-treeview').prevAll('.nav-link').addClass('active');
        });
    };

    _proto._fixHeight = function _fixHeight(tabEmpty) {
        if (tabEmpty === void 0) {
            tabEmpty = false;
        }

        if ($__default["default"]('body').hasClass(CLASS_NAME_FULLSCREEN_MODE)) {
            var windowHeight = $__default["default"](window).height();
            var navbarHeight = $__default["default"](SELECTOR_TAB_NAV).outerHeight();
            $__default["default"](SELECTOR_TAB_EMPTY + ", " + SELECTOR_TAB_LOADING + ", " + SELECTOR_CONTENT_IFRAME).height(windowHeight - navbarHeight);
            $__default["default"](SELECTOR_CONTENT_WRAPPER).height(windowHeight);
        } else {
            var contentWrapperHeight = parseFloat($__default["default"](SELECTOR_CONTENT_WRAPPER).css('height'));

            var _navbarHeight = $__default["default"](SELECTOR_TAB_NAV).outerHeight();

            if (tabEmpty == true) {
                setTimeout(function () {
                    $__default["default"](SELECTOR_TAB_EMPTY + ", " + SELECTOR_TAB_LOADING).height(contentWrapperHeight - _navbarHeight);
                }, 50);
            } else {
                $__default["default"](SELECTOR_CONTENT_IFRAME).height(contentWrapperHeight - _navbarHeight);
            }
        }
    } // Static
        ;

    IFrame._jQueryInterface = function _jQueryInterface(config) {
        if ($__default["default"](SELECTOR_DATA_TOGGLE$1).length > 0) {
            var data = $__default["default"](this).data(DATA_KEY$7);

            if (!data) {
                data = $__default["default"](this).data();
            }

            var _options = $__default["default"].extend({}, Default$7, typeof config === 'object' ? config : data);

            localStorage.setItem('AdminLTE:IFrame:Options', JSON.stringify(_options));
            var plugin = new IFrame($__default["default"](this), _options);
            $__default["default"](this).data(DATA_KEY$7, typeof config === 'object' ? config : data);

            if (typeof config === 'string' && /createTab|openTabSidebar|switchTab|removeActiveTab/.test(config)) {
                plugin[config]();
            }
        } else {
            new IFrame($__default["default"](this), JSON.parse(localStorage.getItem('AdminLTE:IFrame:Options')))._initFrameElement();
        }
    };

    return IFrame;
}();
/**
 * Data API
 * ====================================================
 */


$__default["default"](window).on('load', function () {
    IFrame._jQueryInterface.call($__default["default"](SELECTOR_DATA_TOGGLE$1));
});
/**
 * jQuery API
 * ====================================================
 */

$__default["default"].fn[NAME$7] = IFrame._jQueryInterface;
$__default["default"].fn[NAME$7].Constructor = IFrame;

$__default["default"].fn[NAME$7].noConflict = function () {
    $__default["default"].fn[NAME$7] = JQUERY_NO_CONFLICT$7;
    return IFrame._jQueryInterface;
};
