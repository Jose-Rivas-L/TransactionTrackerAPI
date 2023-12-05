async function getAllNotifications() {
    const response = await fetch('/Notifications/All');
    const data = await response.json();

    const allNotificationsElement = document.getElementById('allNotifications');
    allNotificationsElement.innerHTML = '';

    if (data.success) {
        data.result.forEach(notification => {
            const listItem = document.createElement('li');
            listItem.textContent = `ID: ${notification.id}, Request ID: ${notification.requestId}, Document: ${notification.document}`;
            allNotificationsElement.appendChild(listItem);
        });
    } else {
        allNotificationsElement.innerHTML = 'Error retrieving notifications';
    }
}

async function getNotificationById() {
    const notificationId = document.getElementById('notificationId').value;

    const response = await fetch(`/Notifications/ById?IdNotification=${notificationId}`);
    const data = await response.json();

    const notificationResultElement = document.getElementById('notificationResult');
    notificationResultElement.innerHTML = '';

    if (data.success) {
        const notification = data.result[0];
        notificationResultElement.innerHTML = `ID: ${notification.id}, Request ID: ${notification.requestId}, Document: ${notification.document}`;
    } else {
        notificationResultElement.innerHTML = 'Error retrieving notification';
    }
}
