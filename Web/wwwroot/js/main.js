var user = zdy.user.get();

var main = zdy.module().import(function () {

    this.openedPages = ko.observableArray();

    this.activePage = ko.observable({ MenuName: "" });

    this.openPage = function (page, e) {
        window.openWindow(page, true);
        return false;
    };

    this.closePage = function (page, e) {
        window.closeWindow(page);
        return false;
    };

    this.user = ko.observable(user);

    this.logout = function () {
        zdy.confirm("注销账号？").done(function () {
            $.when(zdy.token.set(""), zdy.user.set("")).done(function () {
                window.location = "/Index";
            });
        });
    };

    this.searchText = ko.observable();

    this.handleSearch = function () {
        zdy.alert.info("查询功能尚未完善");
    };

}).bind();

$(document).ready(function () {
    $(".kt-menu__link[data-page]").on("click", function () {
        window.openWindow($(this).data("page"), true);
        return false;
    });

    $('.search-form input').keypress(function (e) {
        if (e.which === 13) {
            main.handleSearch();
        }
    });

    window.gotoPage("/Dashboard");
});

window.setInterval(function () {
    var $avtivePage = $('.kt-container div.page:visible');
    var $avtiveIframe = $avtivePage.find("iframe");
    if ($avtiveIframe.length > 0) {
        if ($avtiveIframe[0].style.visibility !== "hidden") {
            var $contentHeight = $avtiveIframe.contents().height();
            $avtiveIframe.height($contentHeight);
        }
    }
}, 200);

window.openWindow = function (page, isRefresh) {
    var pageId = page.PageId;
    var pageTitle = page.MenuName;
    var pageSrc = page.PageSrc;

    main.openedPages.remove(function (item) {
        return item.PageId === page.PageId;
    });

    main.openedPages.unshift(page);

    main.activePage(page);

    var $pageContainer = $("#page-container");

    if (isRefresh === true) {
        $pageContainer.find("#" + page.PageId).remove();
    }

    var $page = $pageContainer.find("#" + pageId);

    if ($page.length === 0) {
        window.showLoading();
        $page = $('<div class="page" id="{pageId}"><iframe src="{pageSrc}" style="width:100%; min-height:1000px; border:none; visibility:hidden;" scrolling="no" frameborder="0"></iframe></div>'.replace("{pageId}", pageId).replace("{pageSrc}", pageSrc));
        $page.find('iframe').on("load", function () {
            this.style.visibility = "";
            window.hideLoading();
        });
        $pageContainer.append($page);
    }

    $pageContainer.find('div.page').hide();

    $page.show();

    window.setActivePage(pageSrc);

    window.location = "/Index#" + pageSrc;
};

window.showLoading = function () {
    KTApp.blockPage({
        overlayColor: '#000000',
        type: 'v2',
        state: 'success',
        message: 'Please wait...'
    });
};

window.hideLoading = function () {
    setTimeout(function () {
        KTApp.unblockPage();
    }, 500);
};

window.onhashchange = function (e) {
    var url = e.newURL.split("#")[1];
    if (url !== undefined) {
        var data = main.openedPages().find(function (page) {
            return String(page.PageSrc).toLowerCase() === url.toLowerCase();
        });
        if (data === undefined) {
            var $link = $(".kt-menu__link[href='" + url + "']");
            if ($link.length > 0) {
                data = $link.data("page");
            }
        }
        if (data !== undefined) {
            window.openWindow(data);
        }
    }
};

window.closeWindow = function (page) {
    if (main.openedPages().length === 1) {
        return false;
    }

    main.openedPages.remove(function (item) {
        return item.PageId === page.PageId;
    });

    $("#" + page.PageId).remove();

    var candidatePage = main.openedPages()[0];

    window.openWindow(candidatePage);
};

window.gotoIndex = function () {
    window.location = "/Index";
};

window.setActivePage = function (url) {
    var $link = $(".kt-menu__link[href='" + url + "']");

    if ($link.length > 0) {
        $(".kt-menu__item--active").removeClass('kt-menu__item--active');
        $(".kt-menu__item--open").removeClass("kt-menu__item--open");

        var item = $link.closest('.kt-menu__item')[0];

        var parents = KTUtil.parents(item, '.kt-menu__item--submenu') || [];
        for (var i = 0, len = parents.length; i < len; i++) {
            KTUtil.addClass(KTUtil.get(parents[i]), 'kt-menu__item--open');
        }

        KTUtil.addClass(KTUtil.get(item), 'kt-menu__item--active');
    }
};

window.findPage = function (url) {
    if (url === undefined || url === "") {
        return false;
    }
    var info = url.split("?");
    var href = info[0];
    var $link = $(".kt-menu__link[href='" + href + "']");
    if ($link.length > 0) {
        var data = $link.data("page");
        if (data !== undefined) {
            data.PageSrc = url;

            return data;
        }
    }
    return this.undefined;
};

window.gotoPage = function (url) {
    var data = window.findPage(url);
    if (data) {
        window.openWindow(data, true);
    }
};

window.closePage = function (url) {
    var data = window.findPage(url);
    if (data) {
        window.closeWindow(data, true);
    }
};