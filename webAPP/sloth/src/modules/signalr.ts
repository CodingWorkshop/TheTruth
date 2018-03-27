export default (config: sloth.Config) => {
    var connection = new signalR.HubConnection(
        config.webApiRoot + config.signalrApi
    );

    connection.on('play', function(data: any) {
        var DisplayMessagesDiv = document.getElementById('DisplayMessages');
        DisplayMessagesDiv.innerHTML += '<br/>' + data;
    });
};
