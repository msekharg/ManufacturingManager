/*export is needed to load this script dynamically  */
export function startTimeOut(idleTimeLimitSeconds, sessionWarningSeconds) {
    $(document).ready(function() {
        $(document).idleTimeout({
            idleTimeLimit: idleTimeLimitSeconds, // 180,/* <%=IdleTimeLimitSeconds %>,*/
            dialogDisplayLimit: sessionWarningSeconds, // 120,/* <%=SessionWarningSeconds%>,*/
            dialogText: 'You are about to be logged out of PCS Case Management Portal due to inactivity.',
            redirectUrl:
                '/TimeOut/GoodBye', // '<%=ResolveUrl("~/Logoff.aspx")%>', // redirect to this url. Set this value to YOUR site's logout page.
            activityEvents: 'click keypress scroll wheel mousewheel',
            sessionKeepAliveUrl: '/TimeOut/KeepSessionAlive', // '<%=ResolveUrl("~/Secure/KeepAlive.aspx")%>',
            sessionKeepAliveTimer:
                false // ping the server at this interval in seconds. 600=10 Minutes. Set to false to disable
        });
    });
}