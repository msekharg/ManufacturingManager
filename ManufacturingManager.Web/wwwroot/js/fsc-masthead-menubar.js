// FSC Menubar Masthead v1.1.1
/*
*   This content is licensed according to the W3C Software License at
*   https://www.w3.org/Consortium/Legal/2015/copyright-software-and-document
*
*   File:   Menubar.js
*
*   Desc:   Menubar widget that implements ARIA Authoring Practices
*
*   Author: Jon Gunderson, Ku Ja Eun and Nicholas Hoyt
*/

/*
*   @constructor Menubar
*
*   @desc
*       Wrapper object for a menubar (with nested submenus of links)
*
*   @param domNode
*       The DOM element node that serves as the menubar container. Each
*       child element of menubarNode that represents a menubaritem must
*       be an A element
*/

var Menubar = function (domNode) {

    var elementChildren,
        msgPrefix = 'Menubar constructor argument menubarNode ';

    // Check whether menubarNode is a DOM element
    if (!domNode instanceof Element) {
        throw new TypeError(msgPrefix + 'is not a DOM Element.');
    }
    // Check whether menubarNode has descendant elements
    if (domNode.childElementCount === 0) {
        throw new Error(msgPrefix + 'has no element children.');
    }

    // Check whether menubarNode has A elements
    e = domNode.firstElementChild;
    while (e) {
        var menubarItem = e.firstElementChild;
        if (e && menubarItem && menubarItem.tagName !== 'A') {
            throw new Error(msgPrefix + 'has child elements are not A elements.');
        }
        e = e.nextElementSibling;
    }

    this.domNode = domNode;
    this.menubarItems = []; // See Menubar init method
    this.firstChars = []; // See Menubar init method
    this.firstItem = null; // See Menubar init method
    this.lastItem = null; // See Menubar init method
    this.hasFocus = false; // See MenubarItem handleFocus, handleBlur
    this.hasHover = false; // See Menubar handleMouseover, handleMouseout
};

/*
*   @method Menubar.prototype.init
*
*   @desc
*       Adds ARIA role to the menubar node
*       Traverse menubar children for A elements to configure each A element as a ARIA menuitem
*       and populate menuitems array. Initialize firstItem and lastItem properties.
*/
Menubar.prototype.init = function () {
    var menubarItem, childElement, menuElement, textContent, numItems;

    //this.domNode.setAttribute('role', 'menubar');
    this.domNode.setAttribute('role', 'navigation');
    mainNavBar = '#' + this.domNode.id;
    // Traverse the element children of menubarNode: configure each with
    // menuitem role behavior and store reference in menuitems array.
    elem = this.domNode.firstElementChild;

    while (elem) {
        var menuElement = elem.firstElementChild;

        if (elem && menuElement && menuElement.tagName === 'A') {
            menubarItem = new MenubarItem(menuElement, this);
            menubarItem.init();
            this.menubarItems.push(menubarItem);
            //textContent = menuElement.textContent.trim();
            //this.firstChars.push(textContent.substring(0, 1).toLowerCase());
        }

        elem = elem.nextElementSibling;
    }

    // Use populated menuitems array to initialize firstItem and lastItem.
    numItems = this.menubarItems.length;
    if (numItems > 0) {
        this.firstItem = this.menubarItems[0];
        this.lastItem = this.menubarItems[numItems - 1];
    }
    //this.firstItem.domNode.tabIndex = 0;
};

/* FOCUS MANAGEMENT METHODS */

Menubar.prototype.setFocusToItem = function (newItem) {

    var flag = false;
    for (var i = 0; i < this.menubarItems.length; i++) {
        var mbi = this.menubarItems[i];
        if (mbi.domNode.tabIndex == 0) {
            flag = mbi.domNode.getAttribute('aria-expanded') === 'true';
        }

        mbi.domNode.tabIndex = 0;
        if (mbi.popupMenu) {
            mbi.popupMenu.close();
        }
    }
    console.log("mbi", mbi);
    newItem.domNode.focus();
    //newItem.domNode.tabIndex = 0;
    if (flag && newItem.popupMenu) {
        newItem.popupMenu.open();
    }
};

Menubar.prototype.setFocusToFirstItem = function (flag) {
    this.setFocusToItem(this.firstItem);
};

Menubar.prototype.setFocusToLastItem = function (flag) {
    this.setFocusToItem(this.lastItem);
};

Menubar.prototype.setFocusToPreviousItem = function (currentItem) {
    var index;

    if (currentItem === this.firstItem) {
        newItem = this.lastItem;
    }
    else {
        index = this.menubarItems.indexOf(currentItem);
        newItem = this.menubarItems[index - 1];
    }
    this.setFocusToItem(newItem);
};

Menubar.prototype.setFocusToNextItem = function (currentItem) {
    var index;

    if (currentItem === this.lastItem) {
        newItem = this.firstItem;
    }
    else {
        index = this.menubarItems.indexOf(currentItem);
        newItem = this.menubarItems[index + 1];
    }
    this.setFocusToItem(newItem);
};

/*
*   @constructor MenubarItem
*
*   @desc
*       Object that configures menu item elements by setting tabIndex
*       and registering itself to handle pertinent events.
*
*       While menuitem elements handle many keydown events, as well as
*       focus and blur events, they do not maintain any state variables,
*       delegating those responsibilities to its associated menu object.
*
*       Consequently, it is only necessary to create one instance of
*       MenubarItem from within the menu object; its configure method
*       can then be called on each menuitem element.
*
*   @param domNode
*       The DOM element node that serves as the menu item container.
*       The menuObj PopupMenu is responsible for checking that it has
*       requisite metadata, e.g. role="menuitem".
*
*   @param menuObj
*       The PopupMenu object that is a delegate for the menu DOM element
*       that contains the menuitem element.
*/
var MenubarItem = function (domNode, menuObj) {

    this.menubar = menuObj;
    this.domNode = domNode;
    this.popupMenu = false;
    this.hasFocus = false;
    this.hasHover = false;
    this.isMenubarItem = true;
    this.keyCode = Object.freeze({
        'TAB': 9,
        'RETURN': 13,
        'ESC': 27,
        'SPACE': 32,
        'PAGEUP': 33,
        'PAGEDOWN': 34,
        'END': 35,
        'HOME': 36,
        'LEFT': 37,
        'UP': 38,
        'RIGHT': 39,
        'DOWN': 40
    });
};

MenubarItem.prototype.init = function () {

    if (this.domNode.parentNode.getAttribute('class', 'submenu')) {
        this.domNode.setAttribute('aria-haspopup', 'true');
        this.domNode.setAttribute('aria-expanded', 'false');
    }

    if (this.domNode.parentNode.tagName === 'LI') {
        this.domNode.parentNode.setAttribute('role', 'none');
    }

    this.domNode.addEventListener('keydown', this.handleKeydown.bind(this));
    this.domNode.addEventListener('click', this.handleClick.bind(this));
    this.domNode.addEventListener('focus', this.handleFocus.bind(this));
    this.domNode.addEventListener('blur', this.handleBlur.bind(this));

    // Initialize pop up menus

    var nextElement = this.domNode.nextElementSibling;

    if (nextElement && nextElement.tagName === 'UL') {
        this.popupMenu = new PopupMenu(nextElement, this);
        this.popupMenu.init();

        var dropdownTier2 = this.popupMenu.menuitems;
        // console.log("dropdownTier2", dropdownTier2.length);
        $.each(dropdownTier2, function (index) {

            var thisMenu = "#" + this.domNode.parentElement.id;
            //console.log("thisMenu", thisMenu);
            var menuNum = (index * 28.1) + 6;
            function pos_to_neg(num) {
                return -Math.abs(num);
            }
            var menuTop = pos_to_neg(menuNum);
            var menuListItem = {}
            menuListItem.thisMenu = thisMenu;
            menuListItem.menuTop = menuTop;
            menuList.push(menuListItem);
        });
        // console.log("menuList", menuList);
    }
};
MenubarItem.prototype.handleKeydown = function (event) {

    var tgt = event.currentTarget,
        char = event.key,
        flag = false,
        clickEvent;

    function isPrintableCharacter(str) {
        return str.length === 1 && str.match(/\S/);
    }

    switch (event.keyCode) {
        case this.keyCode.SPACE:
        case this.keyCode.RETURN:
        case this.keyCode.DOWN:
            $(mainNavBar + ' .dropdown-toggle').parent().removeClass('submenu');
            $(mainNavBar + ' .dropdown-toggle').parent().addClass('dropdown');
            $(mainNavBar + ' .dropdown ').removeClass('open');
            $(mainNavBar + ' .dropdown > ul').removeAttr('style');
            $(mainNavBar + ' .dropdown  a').attr('aria-expanded', 'false');
            if (this.popupMenu) {
                this.popupMenu.open();
                this.domNode.setAttribute('aria-expanded', 'true');
                this.popupMenu.setFocusToFirstItem();
                flag = true;
            }
            break;

        case this.keyCode.LEFT:
            $(mainNavBar + ' .dropdown-toggle').parent().removeClass('submenu');
            $(mainNavBar + ' .dropdown-toggle').parent().addClass('dropdown');
            $(mainNavBar + ' .dropdown ').removeClass('open');
            $(mainNavBar + ' .dropdown > ul').removeAttr('style');
            $(mainNavBar + ' .dropdown  a').attr('aria-expanded', 'false');
            this.menubar.setFocusToPreviousItem(this);
            flag = true;
            break;

        case this.keyCode.RIGHT:
            this.menubar.setFocusToNextItem(this);
            flag = true;
            break;

        case this.keyCode.UP:
            if (this.popupMenu) {
                this.popupMenu.open();
                this.popupMenu.setFocusToLastItem();
                flag = true;
            }
            break;

        case this.keyCode.HOME:
        case this.keyCode.PAGEUP:
            this.menubar.setFocusToFirstItem();
            flag = true;
            break;

        case this.keyCode.END:
        case this.keyCode.PAGEDOWN:
            this.menubar.setFocusToLastItem();
            flag = true;
            break;
    }

    if (flag) {
        event.stopPropagation();
        event.preventDefault();
    }
};

MenubarItem.prototype.handleClick = function (event) {
};

MenubarItem.prototype.handleFocus = function (event) {
    this.menubar.hasFocus = true;
};

MenubarItem.prototype.handleBlur = function (event) {
    this.menubar.hasFocus = false;
};

/*
*   @constructor PopupMenu
*
*   @desc
*       Wrapper object for a simple popup menu (without nested submenus)
*
*   @param domNode
*       The DOM element node that serves as the popup menu container. Each
*       child element of domNode that represents a menuitem must have a
*       'role' attribute with value 'menuitem'.
*
*   @param controllerObj
*       The object that is a wrapper for the DOM element that controls the
*       menu, e.g. a button element, with an 'aria-controls' attribute that
*       references this menu's domNode. See MenuButton.js
*
*       The controller object is expected to have the following properties:
*       1. domNode: The controller object's DOM element node, needed for
*          retrieving positioning information.
*       2. hasHover: boolean that indicates whether the controller object's
*          domNode has responded to a mouseover event with no subsequent
*          mouseout event having occurred.
*/
var mainNavBar;
var dropdownList = [];
var menuList = [];
var currentMenuTop;
var currentMenuId;
$(document).ready(function () {
    // on close of bootstrap dropdown clear equalize, top and open-submenus
    $(" .dropdown").on('hide.bs.dropdown', function (e) {

        $(".submenu").removeClass('open-submenu');
        $(".submenu ul").removeClass('equalize');
        $(mainNavBar + " .dropdown ul").removeAttr('style');
    });
    $("#skipLink").click(function () {
        if ($('#pageContent').length == 0) {
            $(".main-container input,  .main-container textarea, .main-container a").first().focus();
        } else {
            $("#pageContent").focus();
        }
    });
});

//equalize each menu height and set top position for tier3 and tier4
var equalizeMenus = function (item, top, tier) {
    var maxHeight = 0;
    var divs = $('.equalize');
    $.each(divs, function () {
        if ($(this).parent('li').hasClass('dropdown') || $(this).parent('li').hasClass('open-submenu')) {
            var divId = $(this).parent()[0].id;
            var height = $(this).height();
            if (maxHeight < height)
                maxHeight = height;
        }
    });
    divs.height(maxHeight);
    if (tier == 'tier3') {
        $(item).css("top", top);
    }
};
//set top position of menus
var menuPosition = function (menuLink) {
    var menuId = "#" + menuLink;
    var menuLength = menuList.length;
    for (var i = 0; i < menuLength; i++) {
        if (menuId == menuList[i].thisMenu) {
            $(menuId + '> .nav-submenu').css('top', menuList[i].menuTop);
        }
    }
};

var PopupMenu = function (domNode, controllerObj, popupMenuItemObj) {
    var elementChildren,
        msgPrefix = 'PopupMenu constructor argument domNode ';

    if (typeof popupMenuItemObj !== 'object') {
        popupMenuItemObj = false;
    }

    // Check whether domNode is a DOM element
    if (!domNode instanceof Element) {
        throw new TypeError(msgPrefix + 'is not a DOM Element.');
    }
    // Check whether domNode has child elements
    if (domNode.childElementCount === 0) {
        throw new Error(msgPrefix + 'has no element children.');
    }


    // Check whether domNode descendant elements have A elements
    var childElement = domNode.firstElementChild;
    while (childElement) {
        var menuitem = childElement.firstElementChild;
        if (menuitem && menuitem === 'A') {
            throw new Error(msgPrefix + 'has descendant elements that are not A elements.');
        }

        childElement = childElement.nextElementSibling;
    }

    this.isMenubar = false;

    this.domNode = domNode;
    this.controller = controllerObj;
    this.popupMenuItem = popupMenuItemObj;

    this.menuitems = []; // See PopupMenu init method
    this.firstChars = []; // See PopupMenu init method

    this.firstItem = null; // See PopupMenu init method
    this.lastItem = null; // See PopupMenu init method

    this.hasFocus = false; // See MenuItem handleFocus, handleBlur
    this.hasHover = false; // See PopupMenu handleMouseover, handleMouseout

};

/*
*   @method PopupMenu.prototype.init
*
*   @desc
*       Add domNode event listeners for mouseover and mouseout. Traverse
*       domNode children to configure each menuitem and populate menuitems
*       array. Initialize firstItem and lastItem properties.
*/
PopupMenu.prototype.init = function () {
    var childElement, menuElement, menuItem, textContent, numItems, label;

    // Configure the domNode itself

    this.domNode.setAttribute('role', 'navigation');
    // commented out due to bugs
    //if (!this.domNode.getAttribute('aria-labelledby') && !this.domNode.getAttribute('aria-label') && !this.domNode.getAttribute('title')) {
    //    label = this.controller.domNode.innerHTML;
    //    this.domNode.setAttribute('aria-label', label);
    //}

    // Traverse the element children of domNode: configure each with
    // menuitem role behavior and store reference in menuitems array.
    childElement = this.domNode.firstElementChild;

    while (childElement) {
        menuElement = childElement.firstElementChild;
        if (menuElement && menuElement.tagName === 'A') {
            menuItem = new MenuItem(menuElement, this);
            menuItem.init();

            this.menuitems.push(menuItem);
            // console.log("menuItems", this.menuitems);
            textContent = menuElement.textContent.trim();
            this.firstChars.push(textContent.substring(0, 1).toLowerCase());
        }
        childElement = childElement.nextElementSibling;
    }

    // Use populated menuitems array to initialize firstItem and lastItem.
    numItems = this.menuitems.length;
    if (numItems > 0) {
        this.firstItem = this.menuitems[0];
        this.lastItem = this.menuitems[numItems - 1];
    }
};

/* EVENT HANDLERS */

PopupMenu.prototype.handleMouseover = function (event) {
    this.hasHover = true;
};

PopupMenu.prototype.handleMouseout = function (event) {
    this.hasHover = false;
    setTimeout(this.close.bind(this, false), 1);
};

/* FOCUS MANAGEMENT METHODS */

PopupMenu.prototype.setFocusToController = function (command, flag) {

    if (typeof command !== 'string') {
        command = '';
    }
    if (this.controller.close) {
        this.popupMenuItem.domNode.focus();
        this.close();

        if (command === 'next') {
            this.controller.hasFocus = false;
            this.controller.close();
            //added if statement to set focus on parent node when arrow right is done on last tier
            if (this.popupMenu) {
                this.controller.controller.menubar.setFocusToNextItem(this.controller.controller, flag);
            }
        }
    }
    else {
        if (command === 'previous') {
            this.controller.menubar.setFocusToPreviousItem(this.controller, flag);
        }
        else if (command === 'next') {
            this.controller.menubar.setFocusToNextItem(this.controller, flag);
        }
        else {
            this.controller.domNode.focus();
        }
    }
};

PopupMenu.prototype.setFocusToFirstItem = function () {
    this.firstItem.domNode.focus();
};

PopupMenu.prototype.setFocusToLastItem = function () {
    this.lastItem.domNode.focus();
};

PopupMenu.prototype.setFocusToPreviousItem = function (currentItem) {
    var index;

    if (currentItem === this.firstItem) {
        this.lastItem.domNode.focus();
    }
    else {
        index = this.menuitems.indexOf(currentItem);
        this.menuitems[index - 1].domNode.focus();
    }
};

PopupMenu.prototype.setFocusToNextItem = function (currentItem) {
    var index;

    if (currentItem === this.lastItem) {
        this.firstItem.domNode.focus();
    }
    else {
        index = this.menuitems.indexOf(currentItem);
        this.menuitems[index + 1].domNode.focus();
    }
};

PopupMenu.prototype.setFocusByFirstCharacter = function (currentItem, char) {
    var start, index, char = char.toLowerCase();

    //Get start index for search based on position of currentItem
    start = this.menuitems.indexOf(currentItem) + 1;
    if (start === this.menuitems.length) {
        start = 0;
    }

    //Check remaining slots in the menu
    index = this.getIndexFirstChars(start, char);

    // If not found in remaining slots, check from beginning
    if (index === -1) {
        index = this.getIndexFirstChars(0, char);
    }

    // If match was found...
    if (index > -1) {
        this.menuitems[index].domNode.focus();
    }
};

PopupMenu.prototype.getIndexFirstChars = function (startIndex, char) {
    for (var i = startIndex; i < this.firstChars.length; i++) {
        if (char === this.firstChars[i]) {
            return i;
        }
    }
    return -1;
};

/* MENU DISPLAY METHODS */
PopupMenu.prototype.open = function () {
    // Get position and bounding rectangle of controller object's DOM node
    var thisMenu = this.domNode;
    var rect = this.controller.domNode.getBoundingClientRect();

    // Set CSS properties

    if (!this.controller.isMenubarItem || !$(mainNavBar + ' .dropdown')) {
        this.domNode.parentNode.setAttribute('class', 'submenu open-submenu');
        //this.domNode.style.left = (rect.width -2) + 'px';
        //var parentItem = this.domNode.parentNode;
        var menuId = this.domNode.parentNode.getAttribute('id');
        var tierId = "#" + this.domNode.parentNode.getAttribute('id');
        var menuParent = this.domNode.parentNode;
        $(menuParent).siblings().removeClass('open-submenu');
        //equalize and menu flush top for tier2
        if ($(tierId).parent().hasClass('flyouts')) {
            $('.equalize').removeAttr('style');
            $(thisMenu).addClass(' equalize');
            equalizeMenus(null, null, 'tier2'); //this must fire before menuPosition because it removes style attribute 
            menuPosition(menuId);
            currentMenuId = $(menuId + '> .nav-submenu');
            currentMenuTop = parseInt($(thisMenu).css('top'), 10);
        }
        //equalize for tier3 and tier4
        if (!$(tierId).parent().hasClass('flyouts')) {
            $(thisMenu).addClass(' equalize');
            equalizeMenus(currentMenuId, currentMenuTop, 'tier3');
        }
    }
    else {
        this.domNode.style.display = 'block';
        this.domNode.style.position = 'absolute';
        //this.domNode.style.top = (rect.height - 2) + 'px';
        this.domNode.style.zIndex = 100;
        this.domNode.parentNode.setAttribute('class', 'dropdown open');
    }

};

PopupMenu.prototype.close = function (force) {
    var thisMenu = this.domNode;

    var controllerHasHover = this.controller.hasHover;

    if (!this.controller.isMenubarItem) {
        controllerHasHover = false;
    }
    //close dropdown on close
    if (force || (!this.hasFocus && !this.hasHover && !controllerHasHover && !$('.flyouts'))) {
        this.domNode.style.zIndex = 0;
        //this.controller.domNode.setAttribute('aria-expanded', 'false');
        if (!this.popupMenuItem) {
            // $(mainNavBar + ' .dropdown-toggle').parent().removeClass('submenu');
            $(mainNavBar + ' .dropdown-toggle').parent().removeClass('submenu');
            $(mainNavBar + ' .dropdown-toggle').parent().addClass('dropdown');
            $(mainNavBar + ' .dropdown-menu').removeAttr('style');
            $(".submenu").removeClass('open-submenu');

        }

    }
};

/*
*   @constructor MenuItem
*
*   @desc
*       Wrapper object for a simple menu item in a popup menu
*
*   @param domNode
*       The DOM element node that serves as the menu item container.
*       The menuObj PopupMenu is responsible for checking that it has
*       requisite metadata, e.g. role="menuitem".
*
*   @param menuObj
*       The object that is a wrapper for the PopupMenu DOM element that
*       contains the menu item DOM element. See PopupMenu.js
*/
var MenuItem = function (domNode, menuObj) {

    if (typeof popupObj !== 'object') {
        popupObj = false;
    }

    this.domNode = domNode;
    this.menu = menuObj;
    this.popupMenu = false;
    this.isMenubarItem = false;

    this.keyCode = Object.freeze({
        'TAB': 9,
        'RETURN': 13,
        'ESC': 27,
        'SPACE': 32,
        'PAGEUP': 33,
        'PAGEDOWN': 34,
        'END': 35,
        'HOME': 36,
        'LEFT': 37,
        'UP': 38,
        'RIGHT': 39,
        'DOWN': 40
    });
};

MenuItem.prototype.init = function () {
    //this.domNode.tabIndex = -1;

    var menuItemParent = this.domNode.parentNode;
    if (!this.domNode.getAttribute('role')) {
        this.domNode.setAttribute('role', 'menuitem');
    }
    if ($(menuItemParent).hasClass('submenu')) {
        this.domNode.setAttribute('aria-haspopup', 'true');
        this.domNode.setAttribute('aria-expanded', 'false');
    }

    if (this.domNode.parentNode.tagName === 'LI') {
        this.domNode.parentNode.setAttribute('role', 'none');
    }

    this.domNode.addEventListener('keydown', this.handleKeydown.bind(this));
    this.domNode.addEventListener('click', this.handleClick.bind(this));
    this.domNode.addEventListener('focus', this.handleFocus.bind(this));
    this.domNode.addEventListener('blur', this.handleBlur.bind(this));
    this.domNode.addEventListener('mouseover', this.handleMouseover.bind(this));
    this.domNode.addEventListener('mouseout', this.handleMouseout.bind(this));

    // Initialize flyout menu

    var nextElement = this.domNode.nextElementSibling;

    if (nextElement && nextElement.tagName === 'UL') {
        this.popupMenu = new PopupMenu(nextElement, this.menu, this);
        this.popupMenu.init();
    }

};

MenuItem.prototype.isExpanded = function () {
    return this.domNode.getAttribute('aria-expanded') === 'true';
};

/* EVENT HANDLERS */

MenuItem.prototype.handleKeydown = function (event) {
    var tgt = event.currentTarget,
        char = event.key,
        flag = false,
        clickEvent;

    function isPrintableCharacter(str) {
        return str.length === 1 && str.match(/\S/);
    }

    switch (event.keyCode) {
        case this.keyCode.SPACE:
        case this.keyCode.RETURN:
            if (this.popupMenu) {
                this.popupMenu.open();
                this.popupMenu.setFocusToFirstItem();
                this.domNode.setAttribute('aria-expanded', 'true');
            }
            else {

                // Create simulated mouse event to mimic the behavior of ATs
                // and let the event handler handleClick do the housekeeping.
                try {
                    clickEvent = new MouseEvent('click', {
                        'view': window,
                        'bubbles': true,
                        'cancelable': true
                    });
                }
                catch (err) {
                    if (document.createEvent) {
                        // DOM Level 3 for IE 9+
                        clickEvent = document.createEvent('MouseEvents');
                        clickEvent.initEvent('click', true, true);
                    }
                }
                tgt.dispatchEvent(clickEvent);
            }

            flag = true;
            break;

        case this.keyCode.ESC:
            this.menu.setFocusToController();
            this.menu.close(true);
            //close menus on escape
            var menuItem = this.menu.domNode;
            $(menuItem).parent().removeClass('open-submenu');
            $(menuItem).siblings('a').attr('aria-expanded', 'false')
            if ($(menuItem).parent().hasClass('open')) {
                $(menuItem).parent().removeClass('open');
                $(menuItem).removeAttr('style');
            }
            flag = true;
            break;

        case this.keyCode.UP:
            this.menu.setFocusToPreviousItem(this);
            flag = true;
            break;

        case this.keyCode.DOWN:
            this.menu.setFocusToNextItem(this);
            flag = true;
            break;

        case this.keyCode.LEFT:
            this.menu.setFocusToController('previous', true);
            this.menu.close(true);
            //close menus on arrow left
            var menuItem = this.menu.domNode;
            $(menuItem).parent().removeClass('open-submenu');
            $(menuItem).siblings('a').attr('aria-expanded', 'false')
            if ($(menuItem).parent().hasClass('open')) {
                $(menuItem).parent().removeClass('open');
                $(menuItem).removeAttr('style');

            }
            flag = true;
            break;

        case this.keyCode.RIGHT:
            if (this.popupMenu) {
                this.popupMenu.open();
                this.popupMenu.setFocusToFirstItem();
                this.domNode.setAttribute('aria-expanded', 'true');
            }
            else {
                this.menu.setFocusToController('next', true);
                this.menu.close(true);
            }
            flag = true;
            break;

        case this.keyCode.HOME:
        case this.keyCode.PAGEUP:
            this.menu.setFocusToFirstItem();
            flag = true;
            break;

        case this.keyCode.END:
        case this.keyCode.PAGEDOWN:
            this.menu.setFocusToLastItem();
            flag = true;
            break;

        default:
            if (isPrintableCharacter(char)) {
                this.menu.setFocusByFirstCharacter(this, char);
                flag = true;
            }
            break;
    }

    if (flag) {
        event.stopPropagation();
        event.preventDefault();
    }
};

MenuItem.prototype.handleClick = function (event) {
    this.menu.setFocusToController();
    this.menu.close(true);

};

MenuItem.prototype.handleFocus = function (event) {
    this.menu.hasFocus = true;
    if (!this.menu.controller.isMenubarItem) {
        this.menu.controller.hasFocus = true;
    }
};

MenuItem.prototype.handleBlur = function (event) {
    this.menu.hasFocus = false;
    if (!this.menu.controller.isMenubarItem) {
        this.menu.controller.hasFocus = false;
    }
    setTimeout(this.menu.close.bind(this.menu, false), 300);
};

MenuItem.prototype.handleMouseover = function (event) {
    this.menu.hasHover = true;
    this.menu.open();
    if (this.popupMenu) {
        this.popupMenu.hasHover = true;
        this.popupMenu.open();
        event.stopPropagation();
    } else {
        // close menu if mouseover link with no submenu
        $(this.menu.domNode).children().removeClass('open-submenu');
    }

};

MenuItem.prototype.handleMouseout = function (event) {
    if (this.popupMenu) {
        this.popupMenu.hasHover = false;
        this.popupMenu.close(true);
        event.stopPropagation();
    }

    this.menu.hasHover = false;
    setTimeout(this.menu.close.bind(this.menu, false), 300);
};