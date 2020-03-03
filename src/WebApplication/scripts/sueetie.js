function CreateCalendarEventComplete(result) {
    jAlert(result, 'Success!');
}

function CreateCalendarEventFailed(result) {
    jAlert('Calendar Processing Failed For Some Very Good Reason, I\'m sure.\n\nCheck your date and time formatting. Also remember that Event Title and Start Date are required.');
}

function DeleteCalendarEventComplete(result) {
    jAlert('Calendar Event was Successfully Deleted!');
}

function DeleteCalendarEventFailed(result) {
    jAlert('Event Deletion Failed For Some Very Good Reason, I\'m sure.');
}

function UpdateCalendarEventComplete(result) {
location.reload();
}

function UpdateCalendarEventFailed(result) {
location.reload();
}
