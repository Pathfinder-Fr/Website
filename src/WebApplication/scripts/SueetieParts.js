
function ShowEdit(_this) {
    // Turns off doubleclick functionality in the wiki.
    window.contentEdit = true;
    $($(_this.parentNode).children()[0]).show('fast');
    $(_this).fadeTo('slow', 0.01);
    // Assign the content of the textarea.
    $($($(_this.parentNode).children()[0]).children()[1]).children()[0].value = $(_this.parentNode).children()[1].innerHTML;
    // Add the clear up function to onunload. This stops us from posting html back.
    RemoveContentPartOnSubmit(_this.parentNode.id);
    $(".draggable").draggable({ handle: '.topmenu', opacity: 0.8, stop: function() {
        // Set focus to the editor. This stops the NicEditor menu from remaining transparent.
        if ($(_this.parentNode).find('.nicEdit-main').length > 0) {
            $(_this.parentNode).find('.nicEdit-main')[0].focus();
        }
    }
    });

    return false;
}

function HideEdit(_this) {
    // Turns on doubleclick functionality in the wiki.
    window.contentEdit = false;
    // If the window is maximized then restore it.
    if ($($(_this.parentNode).children()[0]).children()[1].maximized == true) {
        MenuMaximize($($(_this.parentNode).children()[0]).children()[1]);
    }
    $($(_this.parentNode.parentNode).children()[1]).fadeTo('fast', 1);
    $(_this.parentNode).hide('fast');
    if ($(_this).children()[3].className == 'links') {
        // In HTML Mode reset everything.
        $($(_this).children()[3]).children()[2].childNodes[0].checked = false;
        $($(_this).children()[3]).children()[2].childNodes[0].disabled = false;
        $($(_this.parentNode).children()[1]).children()[0].style.height = '';
    }
    return false;
}

function SaveEdit(_this) {
    // Update the content shown on the page with the contents of the textbox.
    if ($($(_this.parentNode).children()[1]).children()[1].className == '') {
        // This should never be hit anymore. Will be removed after testing.
        $(_this.parentNode.parentNode).children()[1].innerHTML = $($($(_this.parentNode).children()[1]).children()[1]).children()[0].innerHTML;
    }
    else {
        // HTML mode
        $(_this.parentNode.parentNode).children()[1].innerHTML = $(_this).children()[0].value;
    }
}

function ClientCallback(arg, ctx) {// No longer implemented.
}

function ResizeTextBox(_this) {
    // Enlarges the textbox to fill the space created by the removal of the editor.
    // This needs to be dynamic.
    if ($(_this.parentNode.parentNode.parentNode).children()[0].style.height == '') {
        $(_this.parentNode.parentNode.parentNode).children()[0].style.height = '201px';
    }
    else {
        $(_this.parentNode.parentNode.parentNode).children()[0].style.height = '';
    }
}

function RemoveContentPartOnSubmit(id) {
    // Adds a function call to the onsubmit event whilst preserving those previously added.
    if (id == '') {
        return;
    }
    var prevFunc = document.forms[0].onsubmit;
    document.forms[0].onsubmit = function() { RemoveContentPart(id, prevFunc) };
}

function RemoveContentPart(_id, _prevFunc) {
    // Removes a content part from the page.
    if (typeof _prevFunc == 'function') {
        _prevFunc();
    }
    var elem = document.getElementById(_id);
    if (elem) {
        elem.parentNode.removeChild(elem);
    }
}

function GetEscapedContent(_this) {

    if ($(_this.parentNode.parentNode).children()[1].className == '') {
        // Editor mode - Never gets hit anymore. 
        // We always remove the editor before saving/canceling/closing.
        // It makes it simpler to deal with just one state. Remove after testing.
        alert('Editor mode in getescapedcontent got hit');
        return $($(_this.parentNode.parentNode).children()[1]).children()[0].innerHTML;
    }
    // HTML mode
    var html = escape($(_this.parentNode.parentNode).children()[0].value);

    // Remove all text boxes from the form so that page validation does not fail.
    for (x = 0; x < document.forms[0].length; x++) {
        if (document.forms[0][x].className == 'textbox') {
            document.forms[0][x].value = '';
        }
    }
    return html;
}

function MenuMaximize(_this) {
    if (_this.maximized == true) {
        RestoreSizes(_this);
        $(window).scrollTo(_this.parentNode.parentNode);
    }
    else {
        StoreSizes(_this);
        _this.parentNode.parentNode.style.width = $(window).width() + 'px';
        _this.parentNode.parentNode.style.minHeight = $(window).height() + 'px';
        $(_this.parentNode.parentNode).children()[1].style.width = ($(window).width() - 16) + 'px';
        $(_this.parentNode.parentNode).children()[1].style.minHeight = ($(window).height() - 35) + 'px';
        $($(_this.parentNode.parentNode).children()[1]).children()[0].style.width = ($(window).width() - 18) + 'px';
        $($(_this.parentNode.parentNode).children()[1]).children()[1].style.width = ($(window).width() - 18) + 'px';
        _this.parentNode.parentNode.style.left = 0;
        _this.parentNode.parentNode.style.top = 0;
        $(window).scrollTo(0, 0);
        if (!$($($($(_this.parentNode.parentNode).children()[1]).children(":last-child")[0]).children()[2]).children()[0].checked) {
            $($(_this.parentNode.parentNode).children()[1]).children()[1].style.minHeight = ($(window).height() - 150) + 'px';
            $($($(_this.parentNode.parentNode).children()[1]).children()[1]).children()[0].style.width = ($(window).width() - 18) + 'px';
            $($($(_this.parentNode.parentNode).children()[1]).children()[1]).children()[0].style.minHeight = ($(window).height() - 160) + 'px';
        }
        else {
            $($(_this.parentNode.parentNode).children()[1]).children()[0].style.minHeight = ($(window).height() - 140) + 'px';
        }
        $($($($(_this.parentNode.parentNode).children()[1]).children(":last-child")[0]).children()[2]).children()[0].disabled = true;
        _this.maximized = true;
        _this.style.backgroundImage = 'url(/images/shared/nicedit/cpmenumin.png)';
    }
    if ($(_this.parentNode.parentNode).find('.nicEdit-main').length > 0)
        $(_this.parentNode.parentNode).find('.nicEdit-main')[0].focus();
}

function MenuClose(_this) {
    HideEdit($($(_this)[0].parentNode.parentNode).children()[1]);
}

function MenuAdmin(contentID) {
    window.location.href = '/admin/content_parts.aspx?ContentID=' + contentID;
}

function StoreSizes(_this) {
    _this.parentNode.parentNode.origWidth = _this.parentNode.parentNode.style.width;
    _this.parentNode.parentNode.origMinHeight = _this.parentNode.parentNode.style.minHeight;
    _this.parentNode.parentNode.origLeft = _this.parentNode.parentNode.style.left;
    _this.parentNode.parentNode.origTop = _this.parentNode.parentNode.style.top;
    _this.parentNode.parentNode.origC1Width = $(_this.parentNode.parentNode).children()[1].style.width;
    _this.parentNode.parentNode.origC1MinHeight = $(_this.parentNode.parentNode).children()[1].style.minHeight;
    _this.parentNode.parentNode.origC10Width = $($(_this.parentNode.parentNode).children()[1]).children()[0].style.width;
    _this.parentNode.parentNode.origC11Width = $($(_this.parentNode.parentNode).children()[1]).children()[1].style.width;
    if (!$($($($(_this.parentNode.parentNode).children()[1]).children(":last-child")[0]).children()[2]).children()[0].checked) {
        _this.parentNode.parentNode.origC11MinHeight = $($(_this.parentNode.parentNode).children()[1]).children()[1].style.minHeight;
        _this.parentNode.parentNode.origC110Width = $($($(_this.parentNode.parentNode).children()[1]).children()[1]).children()[0].style.width;
        _this.parentNode.parentNode.origC110MinHeight = $($($(_this.parentNode.parentNode).children()[1]).children()[1]).children()[0].style.minHeight;
    }
    else {
        _this.parentNode.parentNode.origC10MinHeight = $($(_this.parentNode.parentNode).children()[1]).children()[0].style.minHeight;
    }
}

function RestoreSizes(_this) {
    _this.parentNode.parentNode.style.width = _this.parentNode.parentNode.origWidth;
    _this.parentNode.parentNode.style.minHeight = _this.parentNode.parentNode.origMinHeight;
    _this.parentNode.parentNode.style.left = _this.parentNode.parentNode.origLeft;
    _this.parentNode.parentNode.style.top = _this.parentNode.parentNode.origTop;

    $(_this.parentNode.parentNode).children()[1].style.width = _this.parentNode.parentNode.origC1Width;
    $(_this.parentNode.parentNode).children()[1].style.minHeight = _this.parentNode.parentNode.origC1MinHeight;
    $($(_this.parentNode.parentNode).children()[1]).children()[0].style.width = _this.parentNode.parentNode.origC10Width;
    $($(_this.parentNode.parentNode).children()[1]).children()[1].style.width = _this.parentNode.parentNode.origC11Width;

    if (!$($($($(_this.parentNode.parentNode).children()[1]).children(":last-child")[0]).children()[2]).children()[0].checked) {
        $($(_this.parentNode.parentNode).children()[1]).children()[1].style.minHeight = _this.parentNode.parentNode.origC11MinHeight;
        $($($(_this.parentNode.parentNode).children()[1]).children()[1]).children()[0].style.width = _this.parentNode.parentNode.origC110Width;
        $($($(_this.parentNode.parentNode).children()[1]).children()[1]).children()[0].style.minHeight = _this.parentNode.parentNode.origC110MinHeight;
    }
    else if (typeof (_this.parentNode.parentNode.origC10MinHeight) != 'undefined') {
        $($(_this.parentNode.parentNode).children()[1]).children()[0].style.minHeight = _this.parentNode.parentNode.origC10MinHeight;
    }
    _this.maximized = false;
    $($($($(_this.parentNode.parentNode).children()[1]).children(":last-child")[0]).children()[2]).children()[0].disabled = false;
    _this.style.backgroundImage = 'url(/images/shared/nicedit/cpmenumax.png)';
}
