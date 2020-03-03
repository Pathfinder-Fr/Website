// -----------------------------------------------------------------------
// <copyright file="SueetieThreads.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Web;

    public class SueetieThreads
    {
        private static AsyncCallback callback = EndWrapperInvoke;
        private static DelegateWrapper wrapperInstance = InvokeWrappedDelegate;

        private static void EndWrapperInvoke(IAsyncResult ar)
        {
            wrapperInstance.EndInvoke(ar);
            ar.AsyncWaitHandle.Close();
        }

        public static void FireAndForget(Delegate d, params object[] args)
        {
            wrapperInstance.BeginInvoke(d, args, callback, null);
        }

        private static void InvokeWrappedDelegate(Delegate d, object[] args)
        {
            try
            {
                d.DynamicInvoke(args);
            }
            catch
            {
            }
        }

        public static void PerformRequestLogInsert(SueetieRequest sueetieRequest)
        {
            try
            {
                var provider = SueetieDataProvider.LoadProvider();
                provider.AddSueetieRequest(sueetieRequest);
            }
            catch (Exception ex)
            {
                SueetieLogs.LogException("Analytics Logging Error: " + HttpContext.Current.Request.RawUrl + " : " + ex.Message + " STACK TRACE: " + ex.StackTrace);
            }
        }

        private delegate void DelegateWrapper(Delegate d, object[] args);

        public delegate void RequestLogInsertDelegate(SueetieRequest sueetieRequest);
    }
}