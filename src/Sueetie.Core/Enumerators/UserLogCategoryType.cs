// -----------------------------------------------------------------------
// <copyright file="UserLogCategoryType.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    public enum UserLogCategoryType
    {
        Registered = 100,
        JoinedCommunity = 101,
        AccountApproved = 102,
        LoggedIn = 103,
        Following = 110,
        UnFollowing = 111,
        Friends = 112,
        BlogPost = 200,
        BlogComment = 201,
        ForumTopic = 300,
        ForumMessage = 301,
        ForumAnswer = 302,
        NewWikiPage = 400,
        WikiPageUpdated = 401,
        NewWikiMessage = 402,
        MultipurposeAlbum = 500,
        ImageAlbum = 501,
        AudioAlbum = 502,
        DocumentAlbum = 503,
        VideoAlbum = 504,
        UserMediaAlbum = 505,
        ImageUploaded = 551,
        AudioUploaded = 552,
        DocumentUploaded = 553,
        VideoUploaded = 554,
        OtherMediaUploaded = 556,
        MarketplaceProduct = 600,
        CMSPageCreated = 800,
        CMSPageUpdated = 801,
        CalendarEvent = 900
    }
}