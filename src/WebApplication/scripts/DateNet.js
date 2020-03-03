(function($) {
    /*
    * dateNet 1.0.0
    *
    * Copyright (c) 2009 Arash Karimzadeh (arashkarimzadeh.com)
    * Licensed under the MIT (MIT-LICENSE.txt)
    * http://www.opensource.org/licenses/mit-license.php
    *
    * Date: May 15 2009
    */
    $.dateNet = function(date) {
        if (date === null || date === undefined) return null;
        if (/\/Date\([0-9\+]*\)\//.test(date))
            return eval('new ' + date.split('/')[1]);
        return '/Date(' + date.getTime() + ')/';
    }
})(jQuery);
