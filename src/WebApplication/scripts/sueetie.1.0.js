
function followPostAuthor(postGuid, blnMember, followDiv) {
    SueetieService.BlogAuthorFollow(postGuid, blnMember, onFollowFaveSuccess, null, followDiv);
}

function followCommenter(postGuid, blnMember, followDiv) {
    SueetieService.BlogCommenterFollow(postGuid, blnMember, onFollowFaveSuccess, null, followDiv);
}

function favePost(postGuid, blnMember, followDiv) {
    SueetieService.BlogFavePost(postGuid, blnMember, onFollowFaveSuccess, null, followDiv);
}

function faveBlogComment(postGuid, blnMember, followDiv) {
    SueetieService.BlogFaveComment(postGuid, blnMember, onFollowFaveSuccess, null, followDiv);
}

function faveForumMessage(userID, messageID, applicationID, followDiv) {
    SueetieService.ForumFaveMessage(userID, messageID, applicationID, onFollowFaveSuccess, null, followDiv);
}

function followProfileUser(userID, profileUserID, stopFollowing, followDiv) {
    SueetieService.ProfileUserFollow(userID, profileUserID, stopFollowing, onFollowFaveSuccess, null, followDiv);
}

function onFollowFaveSuccess(result, followDiv) {
    $(followDiv).slideUp("fast", function() {
        $(this)
                .text(result)
                .slideDown("fast");
    });
    $(followDiv).click(function() {
    $(this).hide("fast");
        return false;
    });
}

function notifyPopup(className, delayBefore, delayDuring) {
    setTimeout(function() {
        $('.' + className).slideDown(2000).fadeTo(delayDuring, 1).slideUp(2000);
    }, delayBefore);
}

document.write('<script type="text/javascript" src="/scripts/sueetieparts.js"></script>'); 
