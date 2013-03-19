$(function() {
    var sessionSelectClass = "emo-sessions";
    es.EmoSessions({
        sessionSelectClass: sessionSelectClass,
        onSessionSelected: playSessionFor
    });

    var charts = new es.Charts();

    var projection;
    function playSessionFor(streamName) {
        if (projection) projection.stop();
        charts.reset();

        projection = new emoProjection({
            fromSource: fromSource,
            hub: $.connection.historicalEmotivSessionHub,
            onStateUpdated: function(state) {
                charts.update(state);
            }
        });

        projection.start();
        
        function fromSource() {
            return fromStream(streamName);
        }
    }
});