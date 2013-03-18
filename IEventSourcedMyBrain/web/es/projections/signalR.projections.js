if (!window.es) { window.es = {}; };
es.projection = function (settings) {

    var projectionBody = settings.body;
    var onStateUpdate = settings.onStateUpdate || function () { };
    var showError = settings.showError || function () { };
    var hideError = settings.hideError || function () { };        
    var category = null;
    var hub = settings.hub;
    var connected = false;
    var subscriptions = [];
    

    return {
        start: startProjection,
        stop: stopProjection
    };

    function startProjection() {
        stopProjection();
        var processor = $initialize_hosted_projections();
        projectionBody();
        processor.initialize();
        var sources = JSON.parse(processor.get_sources());

        hub.client.handleEvent = function(event) {
            var parsedEvent = event;
            processor.process_event(parsedEvent.data,
                parsedEvent.eventStreamId,
                parsedEvent.eventType,
                category,
                parsedEvent.eventNumber,
                parsedEvent.metadata);
            var stateStr = processor.get_state();
            var stateObj = JSON.parse(stateStr);
            onStateUpdate(stateObj, stateStr);
        };

        $.connection.hub.start()
         .done(function () {
             connected = true;
             subscribeTo(sources);
         });        
    }

    function subscribeTo(sources) {
        if (sources.all_streams
        || (sources.categories != null && sources.categories.length > 1)
        || (sources.streams != null && sources.streams.length > 1)) {
            throw "Unsupported projection source to run in the web browser";
        }
        if (sources.categories != null && sources.categories.length == 1) {
            category = sources.categories[0];
            subscribe("$ce-" + category);
        } else {
            category = null;
            subscribe(sources.streams[0]);
        }
    }

    function subscribe(streamName) {
        subscriptions.push(streamName);
        hub.server.subscribeTo(streamName);         
    }

    function stopProjection() {
        if (connected)
        for (var i = 0; i < subscriptions.length; i++) {
            hub.server.unsubscribe(subscriptions[i]);
        }        
    }
}